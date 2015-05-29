using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// Реализация QoS, 
    /// использующей алгоритм имитации отжига
    /// </summary>
    public class SimulatedAnnealingQoS : SimpleTBQoS
    {
        private Random rand;

        /// <summary>
        /// Начальная температура чем она выше,
        /// тем дольше работает алгоритм и больше вероятность
        /// правельного решения
        /// </summary>
        private double initalTemperature;

        /// <summary>
        /// текущая тампература
        /// </summary>
        private double temperature;

        /// <summary>
        /// минимальная температура
        /// </summary>
        private double minTemperature;

        /// <summary>
        /// Максимальные скорости поступления токенов в корзины
        /// </summary>
        private List<float> maxTokensPerDts;

        /// <summary>
        /// Функция находит очередные параметры для оптимазируемой функции.
        /// Тут творится какая то магия, после которой появляются новые скорости 
        /// поступления жетонов в корзины. Работа функции зависит от температуры,
        /// чем она ниже, тем ближе новый вектор к предыдущему
        /// </summary>
        /// <param name="tokensPerDts">Вектор текущих скоростей поступления токенов в корзины</param>
        /// <param name="elementNum">Элемент значение которого следует поменять</param>
        /// <returns>Вектор новых скоростей поступления токенов в корзины</returns>
        private List<float> SetNextTBSpeedValue(List<float> tokensPerDts, int elementNum)
        {
            List<float> result;                       
            result = new List<float>();
            for(int i = 0; i < tokensPerDts.Count; i++)
            {
                // генерация нового значения скорости поступления токенов i-й корзины               
                if (i == elementNum)
                {
                    // генерация новой скорости для выбраной корзины
                    float newTokensPerDt;
                    do
                    {
                        // сохраняем старое значение
                        newTokensPerDt = tokensPerDts[i];
                        // ганерируем приращение
                        float delta = maxTokensPerDts[i]*(float)rand.NextDouble()*(float)temperature;
                        
                        // к старому значению прибавляем или вычитаем это приращение
                        if(rand.Next(2)==0)
                        {
                            newTokensPerDt += delta;
                        }
                        else
                        {
                            newTokensPerDt -= delta;
                        }

                        // проверка на принадлежность поласти значений
                        if(newTokensPerDt < 0)
                        {
                            newTokensPerDt = 0;
                        }
                        if(newTokensPerDt > maxTokensPerDts[i])
                        {
                            newTokensPerDt = maxTokensPerDts[i];
                        }
                    }
                    while (tokensPerDts[i] == newTokensPerDt);
                    result.Add(newTokensPerDt);
                }
                else
                {
                    // для остальных корзин значение оставляем преждним
                    result.Add(tokensPerDts[i]);
                }
            }                   

            return result;
        }

        /// <summary>
        /// Метод понижения температуры
        /// </summary>
        /// <param name="i">номер итерации</param>
        private void DecreaseTemperature(int i)
        {
            temperature = initalTemperature * 0.1 / i;
        }

        /// <summary>
        /// вероятность принятия нового состояния 
        /// (число от 0 до 1)
        /// </summary>
        /// <param name="newState">значение целевой функции, в новом состоянии</param>
        /// <param name="oldState">значение целевой функции, в старом состоянии</param>
        /// <returns></returns>
        private double NewStateProbability(uint newState,uint oldState)
        {
            // если новое состояние лучше старого, его вероятность 1
            if (newState < oldState)
            {
                return 1.0;
            }
            // разница состояний
            uint stateDifference = newState - oldState;

            // вероятность выбора худшего результата
            double probability = Math.Exp((-1.0 * (double)stateDifference) / temperature);
            return probability;
        }

        
        /// <summary>
        /// Инициализация ограничения сверху для скорости поступления
        /// маркеров в маркерные корзины, зависит от параметром мультиплексора
        /// и приоритетов.
        /// </summary>
        private void InitMaxTokensPerDts()
        {
            maxTokensPerDts = new List<float>();

            for (int i = 0; i < buckets.Count; i++)
            {
                maxTokensPerDts.Add(
                    (buckets[i] as SimpleTokenBucket).MaxTokensCount -
                    (buckets[i] as SimpleTokenBucket).GetTokensCount()
                    );
            }               
        }

        /// <summary>
        /// Поиск оптимальных скоростей поступления
        /// маркеров в маркерные корзины методом имитации отжига.
        /// </summary>
        /// <param name="firstTokensPerDts">начальный набор состояний</param>
        /// <returns>оптимальное состояние</returns>
        protected  override List<float> OptimalTokensPerDts(List<float> firstTokensPerDts)
        {
            // инициализация параметров алгоритма
            this.initalTemperature = 10;
            this.minTemperature = 1e-4;
            List<float> oldTokensPerDts = firstTokensPerDts;
            List<float> newTokensPerDts;
            rand = new Random((int)DateTime.Now.Ticks);
            int i = 0;
            int iMax = 1000; // защита от зацикливания
            this.temperature = this.initalTemperature;
            this.InitMaxTokensPerDts();

            while((temperature > minTemperature) && (i++ < iMax))
            {
                // поэлементный обход
                for (int j = 0; j < oldTokensPerDts.Count; j++)
                {
                    // переход в новое состояние по j-му элементу
                    newTokensPerDts = this.SetNextTBSpeedValue(oldTokensPerDts, j);
                    uint newState = this.ObjectiveFunction(newTokensPerDts);
                    uint oldState = this.ObjectiveFunction(oldTokensPerDts);
                    double probability = this.NewStateProbability(newState, oldState);

                    // кидаем кубик
                    double value = rand.NextDouble();
                    // если попали в зону
                    if (value <= probability)
                    {
                        // совершаем переход
                        oldTokensPerDts = newTokensPerDts;
                    }
                }

                // понижаем температуру 
                this.DecreaseTemperature(i);
            }

            return oldTokensPerDts;
        }

        
    }
}

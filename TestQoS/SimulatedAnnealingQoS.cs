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
        /// Функция находит очередные параметры для оптимизируемой функции.
        /// Тут творится какая то магия, после которой появляются новые скорости 
        /// поступления жетонов в корзины. Работа функции зависит от температуры,
        /// чем она ниже, тем ближе новый вектор к предыдущему
        /// </summary>
        /// <param name="tokensPerDts">Вектор текущих скоростей поступления токенов в корзины</param>
        /// <param name="elementNum">Элемент значение которого следует поменять</param>
        /// <returns>Вектор новых скоростей поступления токенов в корзины</returns>
        private List<float> SetNextTBSpeedValue(List<float> tokensPerDts)
        {
            List<float> result = new List<float>();

            for (int i = 0; i < tokensPerDts.Count; i++)
            {
                int newTokensPerDt;
                do
                {
                    // генерируем приращение
                    float delta = maxTokensPerDts[i] * (float)rand.NextDouble() * (float)temperature;
                    if (Math.Abs(delta) < 1) delta = 1;
                    // к старому значению прибавляем или вычитаем это приращение
                    if (rand.Next(2) == 0)
                    {
                        newTokensPerDt = (int)(tokensPerDts[i] + delta);
                    }
                    else
                    {
                        newTokensPerDt = (int)(tokensPerDts[i] - delta);
                    }
                    
                    // проверка на принедлежность границам значений
                    if (newTokensPerDt < 0) 
                    { 
                        newTokensPerDt = 0; 
                    }
                    int maxTokensPerDt = (int)(maxTokensPerDts[i]);
                    if (newTokensPerDt > maxTokensPerDt)
                    {
                        newTokensPerDt = maxTokensPerDt; 
                    }
                }
                while (newTokensPerDt == ((int)tokensPerDts[i]));
                result.Add(newTokensPerDt);
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
        /// Поиск оптимальных скоростей поступления
        /// маркеров в маркерные корзины методом имитации отжига.
        /// </summary>
        /// <param name="firstTokensPerDts">начальный набор состояний</param>
        /// <returns>оптимальное состояние</returns>
        protected  override List<float> OptimalTokensPerDts(List<float> firstTokensPerDts)
        {
            // инициализация параметров алгоритма
            this.initalTemperature = 200;
            this.minTemperature = 0.01;
            List<float> oldTokensPerDts = firstTokensPerDts;
            List<float> newTokensPerDts;
            rand = new Random((int)DateTime.Now.Ticks);
            int i = 0;
            int iMax = 10000; // защита от зацикливания
            this.temperature = this.initalTemperature;
            this.InitMaxTokensPerDts();

            while((temperature > minTemperature) && (i++ < iMax))
            {
                // переход в новое состояние 
                newTokensPerDts = this.SetNextTBSpeedValue(oldTokensPerDts);
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
                
                // понижаем температуру 
                this.DecreaseTemperature(i);
            }

            return oldTokensPerDts;
        }      
    }
}

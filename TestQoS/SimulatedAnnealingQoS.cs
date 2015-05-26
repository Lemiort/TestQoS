using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// это пиздец, товарищи
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
        /// <returns>Вектор новых скоростей поступления токенов в корзины</returns>
        private List<float> SetNextTBSpeedValue(List<float> tokensPerDts)
        {
            List<float> result; 
            // в теории, при низких температурах поиск может зациклица 
            // по этому добавлено ограничение по количеству итераций, 
            // чем оно ниже, тем меньше значение зависит от температуры, 
            // и тем быстрее работает алгоритм
            int i = 0;
            int maxNumOfIteration = 100;

            do
            {
                result = new List<float>();
                for(int j = 0; j < tokensPerDts.Count; j++)
                {
                    // генерация нового значения скорости поступления токенов i-й
                    // корзины, отличной от предыдущей, и непревышающей максимального значения
                    float newTokensPerDt;
                    do
                    {
                        newTokensPerDt = (float)(maxTokensPerDts[j] * rand.NextDouble());
                    }
                    while (tokensPerDts[j] == newTokensPerDt);
                    result.Add(newTokensPerDt);
                }
            }
            while ((!IsPermissible(tokensPerDts, result)) && (++i < maxNumOfIteration));

            return result;
        }

        /// <summary>
        /// Проверяет достаточно ли близко новое значение к старому.
        /// </summary>
        /// <param name="oldTokensPerDts"></param>
        /// <param name="newTokensPerDts"></param>
        /// <returns></returns>
        private bool IsPermissible(List<float> oldTokensPerDts, List<float> newTokensPerDts)
        {
            float oldLen = this.VectorLength(oldTokensPerDts);
            float newLen = this.VectorLength(newTokensPerDts);
            float maxLen = this.VectorLength(maxTokensPerDts);

            if(Math.Abs(newLen - oldLen) <= maxLen*(temperature/initalTemperature))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// поиск длинны вектора
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        private float VectorLength(List<float> vector)
        {
            float sqSum = 0;

            // сумма квадратов 
            foreach(var vectorItem in vector)
            {
                sqSum += vectorItem * vectorItem;
            }

            return (float)Math.Sqrt(sqSum);
        }

        /// <summary>
        /// Метод понижения температуры
        /// TODO хз какой закое юзать правильней и оптимальней, 
        /// пока юзается линейный. сделать более вменябельный закон
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
        /// Целевая функция
        /// TODO
        /// </summary>
        /// <param name="tokensPerDts"></param>
        /// <returns></returns>
        private uint ObjectiveFunction(List<float> tokensPerDts)
        {
            return 0;
        }

        /// <summary>
        /// Инициализация ограничения сверху для скорости поступления
        /// маркеров в маркерные корзины, зависит от параметром мультиплексора
        /// и приоритетов.
        /// TODO
        /// </summary>
        private void InitMaxTokensPerDts()
        {
            maxTokensPerDts = new List<float>();
        }

        /// <summary>
        /// Поиск оптимальных скоростей поступления
        /// маркеров в маркерные корзины методом имитации отжига.
        /// </summary>
        /// <param name="firstTokensPerDts">начальный набор состояний</param>
        /// <returns>оптимальное состояние</returns>
        private List<float> OptimalTokensPerDts(List<float> firstTokensPerDts)
        {
            // инициализация параметров алгоритма
            this.initalTemperature = 1000;
            this.minTemperature = 1e-6;
            List<float> oldTokensPerDts = firstTokensPerDts;
            List<float> newTokensPerDts;
            rand = new Random((int)DateTime.Now.Ticks);
            int i = 1;
            this.temperature = this.initalTemperature;
            this.InitMaxTokensPerDts();

            while(temperature > minTemperature)
            {
    //            this.InitMaxTokensPerDts();
                newTokensPerDts = this.SetNextTBSpeedValue(oldTokensPerDts);
                double probability = this.NewStateProbability(
                this.ObjectiveFunction(newTokensPerDts),
                this.ObjectiveFunction(oldTokensPerDts));

                // кидаем кубик
                double value = rand.NextDouble();
                // если попали в зону
                if(value <= probability)
                {
                    // совершаем переход
                    oldTokensPerDts = newTokensPerDts;
                }

                // понижаем температуру 
                this.DecreaseTemperature(i++);
            }


            return oldTokensPerDts;
        }

        
    }
}

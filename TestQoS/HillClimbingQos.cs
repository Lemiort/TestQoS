using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    public class HillClimbingQos : SimpleTBQoS
    {
        private Random rand;

        /// <summary>
        /// Функция находит очередные параметры для оптимизируемой функции.
        /// </summary>
        /// <param name="tokensPerDts"></param>
        /// <returns></returns>
        private List<float> SetNextTBSpeedValue(List<float> tokensPerDts)
        {
            List<float> result = new List<float>();

            for (int i = 0; i < tokensPerDts.Count; i++)
            {
                int newTokensPerDt;
                do
                {
                    newTokensPerDt = rand.Next((int)this.maxTokensPerDts[i]);
                }
                while (newTokensPerDt == ((int)tokensPerDts[i]));
                result.Add(newTokensPerDt);
            }

            return result;
        }

        protected override List<float> OptimalTokensPerDts(List<float> firstTokensPerDts)
        {
            // инициализация
            this.InitMaxTokensPerDts();
            this.rand = new Random((int)DateTime.Now.Ticks);
            int numIterations = 1000;
            List<float> oldTokensPerDts = firstTokensPerDts;
            List<float> newTokensPerDts;

            for (int i = 0; i < numIterations; i++)
            {
                newTokensPerDts = this.SetNextTBSpeedValue(oldTokensPerDts);              
                uint oldState = this.ObjectiveFunction(oldTokensPerDts);
                uint newState = this.ObjectiveFunction(newTokensPerDts);
                if(newState < oldState)
                {
                    // совершаем переход
                    oldTokensPerDts = newTokensPerDts;
                }
            }

            return oldTokensPerDts;
        }
    }
}

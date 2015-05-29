using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// Реализация QoS, использующей стратегию среднего значения
    /// </summary>
    public class AverageStrategyQos: SimpleTBQoS
    {
        protected override List<float> OptimalTokensPerDts(List<float> firstTokensPerDts)
        {
            List<float> result = new List<float>(); ;
            for (int i = 0; i < buckets.Count; i++)
            {
               
                result.Add(
                (generatorAnalyzers[i] as SimpleAnalyzer).
                GetAveragePassedPacketsSize() * (float)1.2);
                float t1 = (generatorAnalyzers[i] as SimpleAnalyzer).
                GetAveragePassedPacketsSize();
            }

            return result;
        }
    }
}

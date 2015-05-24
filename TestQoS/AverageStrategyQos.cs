using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    class AverageStrategyQos: SimpleTBQoS
    {
        public override void MakeTick()
        {
            /*******************************************************/
            /************Создание всех объектов*********************/
            /*******************************************************/
            //время квантования
            if (generators == null)
            {
                throw new NullReferenceException();
            }
            /*******************************************************/
            /*******************************************************/



            /*******************************************************/
            /*********************Основной цикл*********************/
            /*******************************************************/
            //считаем изменение времени
            /*long prevTime = DateTime.Now.Ticks;*/

            //бывший основной цикл
            {
                //считаем изменение времени
                long dt = DateTime.Now.Ticks - prevTime;

                //время в милисекундах
                TimeSpan time = new TimeSpan(dt);
                //собсно сам цикл
                if (time.Milliseconds >= (qtime as QuantizedTime).timeSlice)
                {
                    //generator.MakePacket();
                    foreach (TrafficGenerator generator in generators)
                    {
                        (generator as SimpleTrafficGenerator).MakePacket();
                    }
                    foreach (Analyzer analyzer in generatorAnalyzers)
                    {
                        (analyzer as SimpleAnalyzer).Update();
                    }

                    foreach (TokenBuket bucket in buckets)
                    {
                        (bucket as SimpleTokenBuket).Update();
                    }
                    foreach (Analyzer analyzer in bucketAnalyzers)
                    {
                        (analyzer as SimpleAnalyzer).Update();
                    }
                    for (int i = 0; i < buckets.Count; i++ )
                    {
                        (buckets.ElementAt(i) as SimpleTokenBuket).Update();
                        (bucketAnalyzers.ElementAt(i) as SimpleAnalyzer).Update();

                        //оптимизация!!
                        (buckets.ElementAt(i) as SimpleTokenBuket).TokensPerDt =
                             (generatorAnalyzers.ElementAt(i) as SimpleAnalyzer).
                             GetAveragePassedPacketsSize();
                    }

                        (multiplexer as SimpleMultiplexer).Update();

                    (multiplexorAnalyzer as SimpleAnalyzer).Update();
                    //(multiplexorAnalyzer as SimpleAnalyzer).PrintFirstQuantInfo();
                    (bucketsAnalyzer as SimpleAnalyzer).Update();

                    prevTime = DateTime.Now.Ticks;

                }
            }
            /*******************************************************/
            /*******************************************************/

        }
    }
}

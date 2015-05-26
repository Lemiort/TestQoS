using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    public class AverageStrategyQos: SimpleTBQoS
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

                    for (int i = 0; i < buckets.Count; i++ )
                    {
                        (buckets.ElementAt(i) as SimpleTokenBucket).Update();
                        (bucketAnalyzers.ElementAt(i) as SimpleAnalyzer).Update();

                        //оптимизация!!
                        (buckets.ElementAt(i) as SimpleTokenBucket).TokensPerDt = 
                             (generatorAnalyzers.ElementAt(i) as SimpleAnalyzer).
                             GetAveragePassedPacketsSize() * (float)1.2;
                            
                    }

                        (multiplexer as SimpleMultiplexer).Update();

                    (multiplexorAnalyzer as SimpleAnalyzer).Update();
                    (bucketsAnalyzer as SimpleAnalyzer).Update();

                    //история байтов мультиплексора
                    multiplexorBytes.Enqueue((multiplexer as SimpleMultiplexer).GetLastThroughputSize());
                    //сумма байтов за историю
                    MultiplexorSummaryBytes += (multiplexer as SimpleMultiplexer).GetLastThroughputSize();
                    multiplexorAverageBytes.Enqueue((float)MultiplexorSummaryBytes / (float)multiplexorBytes.Count);

                    if (multiplexorBytes.Count > historySize)
                    {
                        //убираем из истории байт, а так же из суммарного размера
                        MultiplexorSummaryBytes -= multiplexorBytes.Dequeue();
                        multiplexorAverageBytes.Dequeue();
                    }

                    prevTime = DateTime.Now.Ticks;

                }
            }
            /*******************************************************/
            /*******************************************************/

        }
    }
}

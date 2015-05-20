using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    public abstract class QoS
    {
        /// <summary>
        /// Период наблюдения. Квант времени, 
        /// за который не может пройти более одного пакета.
        /// Измеряется в миллисекундах.
        /// </summary>
        //private double observationPeriod;

        // Фабричные методы
        public abstract TokenBuket MakeTokenBuket();
        public abstract TrafficGenerator MakeTrafficGenerator();
        public abstract ModelTime MakeModelTime();
        public abstract Packet MakePacket();
        public abstract Multiplexer MakeMultiplexer();
        public abstract Analyzer MakeAnalyzer();


        /// <summary>
        /// запуск алгоритма
        /// </summary>
        public abstract void Run();

    }
}

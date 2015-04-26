using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    abstract class QoS
    {
        /// <summary>
        /// Период наблюдения. Квант времени, 
        /// за который не может пройти более одного пакета.
        /// Измеряется в миллисекундах.
        /// </summary>
        private double observationPeriod;

        public abstract TokenBuket TokenBuket();
        public abstract TrafficGenerator TrafficGenerator();



        // запускать алгоритм из QoS или TokenBuket? 
        public abstract void Run();
    }
}

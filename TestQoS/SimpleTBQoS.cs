using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// реализация фабрики QoS, создаёт вёдра и генераторы траффика
    /// </summary>
    class SimpleTBQoS : QoS
    {
        public override TokenBuket TokenBuket()
        {
            return new SimpleTokenBuket();
        }
        public override TrafficGenerator TrafficGenerator()
        {
            return new SimpleTrafficGenerator();
        }
    }
}

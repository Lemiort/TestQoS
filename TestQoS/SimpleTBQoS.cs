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
        /// <summary>
        /// Реализация фабричного метода MakeTokenBuket 
        /// </summary>
        /// <returns></returns>
        public override TokenBuket MakeTokenBuket()
        {
            return new SimpleTokenBuket();
        }

        /// <summary>
        /// Реализация фабричного метода MakeTrafficGenerator
        /// TODO
        /// </summary>
        /// <returns></returns>
        public override TrafficGenerator MakeTrafficGenerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public override ModelTime MakeModelTime()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public override Packet MakePacket()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        public override void Run()
        {
            throw new NotImplementedException();
        }
    }
}

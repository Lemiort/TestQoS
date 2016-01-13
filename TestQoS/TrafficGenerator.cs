using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// Герератор трафика
    /// </summary>
    public abstract class TrafficGenerator
    {
        public abstract Packet MakePacket();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// Реализация оболочки пакета
    /// </summary>
    public class SimplePacket : Packet
    {
        public SimplePacket(uint size)
        {
            this.Size = size;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// TODO: Надо как то это по-нормальному назвать
    /// </summary>
    class SimplePacket : Packet
    {
        public SimplePacket(int size)
        {
            this.Size = size;
        }
    }
}

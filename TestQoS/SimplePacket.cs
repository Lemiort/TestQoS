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
    public class SimplePacket : Packet
    {
        public SimplePacket(uint size)
        {
            this.Size = size;
        }
    }
}

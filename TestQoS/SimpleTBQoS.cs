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
        SimpleTrafficGenerator generator;
        SimpleTokenBuket bucket;


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
            QuantizedTime qtime = new QuantizedTime(10);
            generator = new SimpleTrafficGenerator(qtime, 24, 256, 10, 100);
            bucket = new SimpleTokenBuket(32);

            SimplePacket packet;
            while(true)
            {
                packet = (SimplePacket)generator.MakePacket();
                if (packet != null)
                {
                    bucket.PushPacket(packet, qtime);
                    if (bucket.PopPacket() != null)
                        Console.WriteLine(packet.Size);
                    else
                        Console.WriteLine("--");
                }
            }
            //throw new NotImplementedException();
        }
    }
}

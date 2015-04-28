using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    class SimpleTokenBuket : TokenBuket
    {
        /// <summary>
        /// число токенов в милисекунду
        /// </summary>
        protected double tokensSpeed;

        List<SimplePacket> packets;

        /// <summary>
        /// конструктор ведра
        /// </summary>
        /// <param name="tokensperTime">число токенов в милисекунду</param>
        public SimpleTokenBuket(double tokensPerTime)
        {
            tokensSpeed = tokensPerTime;
            packets = new List<SimplePacket>();
        }

        /// <summary>
        /// добавляет пакет в корзину
        /// </summary>
        /// <param name="packet">пакет</param>
        /// <param name="time"></param>
        public void PushPacket(SimplePacket packet, QuantizedTime time)
        {
            if(time.timeSlice * tokensSpeed >= packet.Size)
                packets.Add(packet);
        }

        /// <summary>
        /// достаёт один пакет из корзины
        /// </summary>
        /// <returns>возвращает пакет или null</returns>
        public SimplePacket PopPacket()
        {
            return packets.Last();
        }
    }
}

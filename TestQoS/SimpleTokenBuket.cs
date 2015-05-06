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
        public float TokensPerMs
        {
            get;
            set;
        }

        /// <summary>
        /// время обработки предыдущего пакета
        /// </summary>
        private long prevPacketTime;

        /// <summary>
        /// число токенов
        /// </summary>
        private long tokensCount;

        /// <summary>
        /// обработчик события обработки пакета
        /// </summary>
        /// <param name="packet">пакет или null</param>
        public delegate void PacketProcessHandler(Packet packet);

        /// <summary>
        /// отбрасывание или пропуск пакета
        /// </summary>
        /// <param name="packet">пакет на вход</param>
        /// <returns>если пакет проходит, возвращает пакета, иначе null</returns>
        public Packet ProcessPacket(Packet packet)
        {

            //проверка на ненулевой пакет
            if (packet != null)
            {
                //считаем изменение времени
                //TODO: сделать поправку на то, что Tick != милисекундам!!!
                long dt = DateTime.Now.Ticks - prevPacketTime;

                //время в милисекундах
                TimeSpan time = new TimeSpan(dt);

                //добавляем токены
                tokensCount += (long)(time.Milliseconds * TokensPerMs);

                //проверяем пакет
                if (packet.Size < tokensCount)
                {
                    tokensCount -= packet.Size;

                    //пропускаем пакет
                    onPacketPass(packet);
                    return packet;
                }
                else
                {
                    //не пропускаем пакет
                    onPacketNotPass(packet);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// конструктор
        /// TODO: написать конструктор получше
        /// </summary>
        public SimpleTokenBuket()
        {
            prevPacketTime = DateTime.Now.Ticks;
            tokensCount = 0;
        }

        public event PacketProcessHandler onPacketPass;
        public event PacketProcessHandler onPacketNotPass;

    }
}

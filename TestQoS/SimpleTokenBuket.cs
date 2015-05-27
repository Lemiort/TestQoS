using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// Реализация маркерной корзины
    /// </summary>
    public class SimpleTokenBucket : TokenBuket
    {
        /// <summary>
        /// время квантовани
        /// </summary>
        QuantizedTime qtime;

        /// <summary>
        /// очередь обработки пакетов
        /// </summary>
        Queue<Packet> packets;

        /// <summary>
        /// число токенов в квант
        /// </summary>
        public float TokensPerDt
        {
            get;
            set;
        }

        /// <summary>
        /// максимальное число токенов
        /// "ёмкость" маркер
        /// </summary>
        public float MaxTokensCount
        {
            get;
            set;
        }

        /// <summary>
        /// число токенов
        /// </summary>
        private long tokensCount;

        public long GetTokensCount()
        {
            return tokensCount;
        }

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
        public void ProcessPacket(Packet packet)
        {
            //проверка на ненулевой пакет
            if (packet != null)
            {
                packets.Enqueue(packet);                
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public SimpleTokenBucket(QuantizedTime _time)
        {
            qtime = _time;
            tokensCount = 0;
            MaxTokensCount = 400;
            TokensPerDt = 20;
            packets = new Queue<Packet>();
        }

        /// <summary>
        /// Функция обновления параметров маркерной корзины
        /// </summary>
        public void Update()
        {
            while (packets.Count != 0)
            {
                Packet packet = packets.Dequeue();
                //проверка на ненулевой пакет
                if (packet != null)
                {
                    if(tokensCount+ (long)TokensPerDt <= MaxTokensCount)
                        tokensCount += (long)TokensPerDt;
                    else
                    {
                        tokensCount = (long)MaxTokensCount;
                    }

                    //проверяем пакет
                    if (packet.Size <= tokensCount)
                    {
                        tokensCount -= packet.Size;
                        if(onPacketPass != null)
                            //пропускаем пакет
                            onPacketPass(packet);
                    }
                    else
                    {
                        if(onPacketNotPass != null)
                            //не пропускаем пакет
                            onPacketNotPass(packet);
                    }
                }
            }
        }

        public event PacketProcessHandler onPacketPass;
        public event PacketProcessHandler onPacketNotPass;

        /// <summary>
        /// конструктор копии
        /// </summary>
        /// <param name="prev"></param>
        public SimpleTokenBucket(SimpleTokenBucket prev)
        {
            this.MaxTokensCount = prev.MaxTokensCount;
            this.packets = new Queue<Packet>();
            foreach( var packet in prev.packets)
            {
                this.packets.Enqueue(packet);
            }
            this.qtime = prev.qtime;
            this.tokensCount = prev.tokensCount;
            this.TokensPerDt = prev.TokensPerDt;
        }
    }
}

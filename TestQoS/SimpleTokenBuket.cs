using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
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
        /// "ёмкость" ведра
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
                /*//считаем изменение времени
                long dt = DateTime.Now.Ticks - prevPacketTime;

                //время в милисекундах
                TimeSpan time = new TimeSpan(dt);

                //добавляем токены
                tokensCount += (long)(TokensPerDt * qtime.FromAnalogToDigital(time.Milliseconds));

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
                }*/
            }
        }

        /// <summary>
        /// конструктор
        /// TODO: написать конструктор получше
        /// </summary>
        public SimpleTokenBucket(QuantizedTime _time)
        {
            qtime = _time;
            tokensCount = 0;
            MaxTokensCount = 400;
            TokensPerDt = 20;
            packets = new Queue<Packet>();
        }

        public void Update()
        {
            while (packets.Count != 0)
            {
                Packet packet = packets.Dequeue();
                //проверка на ненулевой пакет
                if (packet != null)
                {
                    /*//считаем изменение времени
                    long dt = DateTime.Now.Ticks - prevPacketTime;

                    //время в милисекундах
                    TimeSpan time = new TimeSpan(dt);

                    //добавляем токены
                    tokensCount += (long)(TokensPerDt * qtime.FromAnalogToDigital(time.Milliseconds));*/
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

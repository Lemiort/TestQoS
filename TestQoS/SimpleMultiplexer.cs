using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    public class SimpleMultiplexer: Multiplexer
    {

        /// <summary>
        /// предыдущее время обработки всех пакетов, в тактах
        /// </summary>
        private long prevUpdateTime;

        /// <summary>
        /// dt
        /// </summary>
        QuantizedTime qtime;

        /// <summary>
        /// размер байтов в очереди
        /// </summary>
        UInt64 queueSize;

        private UInt64 lastThroughputSize;

        public UInt64 GetLastThroughputSize()
        {
            return lastThroughputSize;
        }

        /// <summary>
        /// обработчик события обработки пакета
        /// </summary>
        /// <param name="packet">пакет или null</param>
        public delegate void PacketProcessHandler(Packet packet);

        /// <summary>
        /// срабатывает, если пакет прошёл, пакет ловит анализатор
        /// </summary>
        public event PacketProcessHandler onPacketPass;

        /// <summary>
        /// срабатывает, если пакет был отброшен, пакеты ловит анализатор
        /// </summary>
        public event PacketProcessHandler onPacketNotPass;

        /// <summary>
        /// очередь пакетов, ждущих обработки
        /// </summary>
        Queue<Packet> packets;

        /// <summary>
        /// отбрасывание или пропуск пакета
        /// </summary>
        /// <param name="packet">пакет на вход</param>
        /// <returns>если пакет проходит, возвращает пакета, иначе null</returns>
        public void ProcessPacket(Packet packet)
        {
            if (packet != null)
            {
                //*if (queueSize + packet.Size <= MaxQueueSize)
                {
                    packets.Enqueue(packet);
                    //queueSize += packet.Size;
                }
            }
        }

        public SimpleMultiplexer(QuantizedTime time)
        {
            qtime = time;
            packets = new Queue<Packet>();
            BytesPerDt = 50;
            MaxQueueSize = 60;
            queueSize = 0;

            prevUpdateTime = DateTime.Now.Ticks;
        }

        /// <summary>
        /// обрабатывает все пакеты, попавшие в очередь
        /// </summary>
        public void Update()
        {
            //считаем изменение времени
            long dt = DateTime.Now.Ticks - prevUpdateTime;

            //время в милисекундах
            TimeSpan time = new TimeSpan(dt);

            //если время квантования прошло
            if (time.Milliseconds >= qtime.timeSlice)
            {
                //обрабатываем все пакеты в очереди
                while (packets.Count > 0)
                {
                    //достаём из очереди
                    Packet packet = packets.Dequeue();

                    if (packet != null)
                    {
                        //добавляем к размеру буффера
                        queueSize += packet.Size;

                        //если пролазит, обрабатываем
                        if (queueSize <= MaxQueueSize)
                        {
                            onPacketPass(packet);
                        }
                            //иначе на мороз
                        else
                        {
                            queueSize -= packet.Size;
                            onPacketNotPass(packet);
                        }

                    }
                }

                //сколько байтов реально ушло
                if (queueSize >= BytesPerDt)
                {
                    //записываем последнее ушедшее число байтов
                    lastThroughputSize = BytesPerDt;
                    queueSize -= BytesPerDt;
                }
                else
                {
                    //записываем последнее ушедшее число байтов
                    lastThroughputSize  = queueSize;
                    queueSize = 0;
                }
                prevUpdateTime = DateTime.Now.Ticks;
                /*if(passedBytes > 0)
                    Console.WriteLine("{0} bytes passed", passedBytes);*/
            }
        }
    }
}

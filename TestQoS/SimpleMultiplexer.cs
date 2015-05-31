using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// Реализация мультиплексора 
    /// </summary>
    public class SimpleMultiplexer: Multiplexer
    {

        /// <summary>
        /// Предыдущее время обработки всех пакетов, в тактах
        /// </summary>
        private long prevUpdateTime;

        /// <summary>
        /// Временная модель
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

        public UInt64 GetQueueSize()
        {
            return lastQueueSize;
        }

        //размер очереди после обработки пакета
        private ulong lastQueueSize;

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
                            if(onPacketPass != null)
                                onPacketPass(packet);
                        }
                            //иначе на мороз
                        else
                        {
                            queueSize -= packet.Size;
                            if(onPacketNotPass != null)
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
                lastQueueSize = queueSize;
                prevUpdateTime = DateTime.Now.Ticks;
                /*if(passedBytes > 0)
                    Console.WriteLine("{0} bytes passed", passedBytes);*/
            }
        }

        /// <summary>
        /// конструктор копии
        /// </summary>
        /// <param name="prev"></param>
        public SimpleMultiplexer(SimpleMultiplexer prev)
        {
            this.BytesPerDt = prev.BytesPerDt;
            this.lastThroughputSize = prev.lastThroughputSize;
            this.MaxQueueSize = prev.MaxQueueSize;
            this.packets = new Queue<Packet>();
            foreach(var packet in prev.packets)
            {
                packets.Enqueue(packet);
            }

            this.prevUpdateTime = prev.prevUpdateTime;
            this.qtime = prev.qtime;
            this.queueSize = prev.queueSize;
        }
    }
}

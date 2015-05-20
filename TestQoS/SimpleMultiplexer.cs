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
                packets.Enqueue(packet);
            }
        }

        public SimpleMultiplexer(QuantizedTime time)
        {
            qtime = time;
            packets = new Queue<Packet>();
            bytesPerDt = 200;

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

                ///пакеты, что прошли обработку
                UInt64 passedBytes = 0;
                while (packets.Count > 0)
                {
                    Packet packet = packets.Dequeue();
                    if (packet != null)
                    {
                        passedBytes += packet.Size;
                        if (passedBytes <= bytesPerDt)
                        {
                            onPacketPass(packet);
                        }
                        else
                        {
                            passedBytes -= packet.Size;
                            onPacketNotPass(packet);
                        }
                    }
                }
                prevUpdateTime = DateTime.Now.Ticks;
                /*if(passedBytes > 0)
                    Console.WriteLine("{0} bytes passed", passedBytes);*/
            }
        }
    }
}

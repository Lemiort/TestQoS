using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    public class SimpleAnalyzer: Analyzer
    {
        /// <summary>
        /// информацию о скольки последних квантах хранить
        /// </summary>
        public int QuantHistorySize
        {
            get;
            set;
        }

        /// <summary>
        /// общий размер прошедших пакетов
        /// </summary>
        private ulong summaryPassedPacketsSize;

        /// <summary>
        /// общий размер отброшенных пакетов
        /// </summary>
        private ulong summaryNotPassedPacketsSize;

        class HistoryQuant
        {
            public Queue<Packet> packets;
            public ulong summarySize
            {
                get;
                private set;
            }

            public HistoryQuant()
            {
                summarySize = 0;
                packets = new Queue<Packet>();
            }

            public void Enqueue(Packet packet)
            {
                summarySize += packet.Size;
                packets.Enqueue(packet);
            }

            public Packet Dequeue()
            {
                summarySize -= packets.Peek().Size;
                return packets.Dequeue();
            }

            public Packet Peek()
            {
                return packets.Peek();
            }

            public int Count()
            {
                return packets.Count();
            }
        }

        private Queue<HistoryQuant> quantsPassedBucket;
        private Queue<HistoryQuant> quantsNotPassedBucket;
        private Queue<HistoryQuant> quantsPassedMultiplexer;
        private Queue<HistoryQuant> quantsNotPassedMultiplexer;

        private HistoryQuant packetsPassedBucket;
        private HistoryQuant packetsNotPassedBucket;
        private HistoryQuant packetsPassedMultiplexer;
        private HistoryQuant packetsNotPassedMultiplexer;

        public SimpleAnalyzer()
        {
            quantsPassedBucket = new Queue<HistoryQuant>();
            quantsNotPassedBucket = new Queue<HistoryQuant>();
            quantsPassedMultiplexer = new Queue<HistoryQuant>();
            quantsNotPassedMultiplexer = new Queue<HistoryQuant>();

            packetsPassedBucket = new HistoryQuant();
            packetsNotPassedBucket = new HistoryQuant();
            packetsPassedMultiplexer = new HistoryQuant();
            packetsNotPassedMultiplexer = new HistoryQuant();

            summaryNotPassedPacketsSize = 0;
            summaryPassedPacketsSize = 0;

            QuantHistorySize = 100;
        }

        /// <summary>
        /// сохраняет информацию о пакете, что прошёл через ведро
        /// </summary>
        /// <param name="packet">пакет</param>
        public override void OnBucketPassPacket(Packet packet)
        {
            packetsPassedBucket.Enqueue(packet);
        }

        /// <summary>
        /// сохраняет информацию о пакете, что не прошёл через ведро
        /// </summary>
        /// <param name="packet">пакет</param>
        public override void OnBucketNotPassPacket(Packet packet)
        {
            packetsNotPassedBucket.Enqueue(packet);
        }

        /// <summary>
        /// сохраняет информацию о пакете, что прошёл через мультиплексор
        /// </summary>
        /// <param name="packet">пакет</param>
        public override void OnMultiplexerPassPacket(Packet packet)
        {
            packetsPassedMultiplexer.Enqueue(packet);
        }

        /// <summary>
        /// сохраняет информацию о пакете, что не прошёл через мультиплексор
        /// </summary>
        /// <param name="packet">пакет</param>
        public override void OnMultiplexerNotPassPacket(Packet packet)
        {
            packetsNotPassedMultiplexer.Enqueue(packet);
        }

        /// <summary>
        /// выводит инфу о первом кванте в консоль
        /// </summary>
        public void PrintFirstQuantInfo()
        {
            if (quantsPassedBucket.Count > 0)
            {
                if (quantsPassedBucket.Peek().summarySize > 0)
                {
                    Console.WriteLine("\n\n{0} bytes passed throght buckets", quantsPassedBucket.Peek().summarySize);
                    Console.WriteLine("{0} bytes not passed throght buckets", quantsNotPassedBucket.Peek().summarySize);
                    Console.WriteLine("{0} bytes passed throght multiplexor", quantsPassedMultiplexer.Peek().summarySize);
                    Console.WriteLine("{0} bytes not passed throght multiplexor", quantsNotPassedMultiplexer.Peek().summarySize);
                }
            }
        }

        /// <summary>
        /// переходит к записи в следующий квант
        /// вызывать каждый квант времени
        /// </summary>
        public void Update()
        {
            //записываем инфу о текущих квантах
            quantsPassedBucket.Enqueue(packetsPassedBucket);
            quantsNotPassedBucket.Enqueue(packetsNotPassedBucket);
            quantsPassedMultiplexer.Enqueue(packetsPassedMultiplexer);
            quantsNotPassedMultiplexer.Enqueue(packetsNotPassedMultiplexer);

            //записываем общее число прошедших и отброшенных байтов
            summaryPassedPacketsSize += packetsPassedMultiplexer.summarySize;
            summaryNotPassedPacketsSize += packetsNotPassedMultiplexer.summarySize;

            //убираем лишнюю инфу, если она есть
            while(quantsPassedBucket.Count > QuantHistorySize)
            {
                quantsPassedBucket.Dequeue();
                quantsNotPassedBucket.Dequeue();

                //убираем информацию о канувших в лету байтах
                summaryPassedPacketsSize -= quantsPassedMultiplexer.Peek().summarySize;
                summaryNotPassedPacketsSize -= quantsNotPassedMultiplexer.Peek().summarySize;

                quantsPassedMultiplexer.Dequeue();
                quantsNotPassedMultiplexer.Dequeue();
            }

            packetsPassedBucket = new HistoryQuant();
            packetsNotPassedBucket = new HistoryQuant();
            packetsPassedMultiplexer = new HistoryQuant();
            packetsNotPassedMultiplexer = new HistoryQuant();
        }

        /// <summary>
        /// среднее число прошедших пакетов за квант
        /// </summary>
        /// <returns></returns>
        public float GetAveragePassedPacketsSize()
        {
            return (float)summaryPassedPacketsSize / (float)quantsPassedMultiplexer.Count();
        }

        /// <summary>
        /// среднее число отброшенных пакетов за квант
        /// </summary>
        /// <returns></returns>
        public float GetAverageNotPassedPacketsSize()
        {
            return (float)summaryNotPassedPacketsSize / (float)quantsNotPassedMultiplexer.Count();
        }
    }
}

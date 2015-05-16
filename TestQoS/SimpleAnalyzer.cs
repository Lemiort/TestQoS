using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    class SimpleAnalyzer: Analyzer
    {
        /// <summary>
        /// информацию о скольки последних квантах хранить
        /// </summary>
        public int quantHistorySize
        {
            get;
            set;
        }

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

            public int Count()
            {
                return packets.Count();
            }
        }

        private Queue<HistoryQuant> quantsPassedBucket;
        private Queue<HistoryQuant> quantsNotPassedBucket;
        private Queue<HistoryQuant> quantsPassedMultiplexor;
        private Queue<HistoryQuant> quantsNotPassedMultiplexor;

        private HistoryQuant packetsPassedBucket;
        private HistoryQuant packetsNotPassedBucket;
        private HistoryQuant packetsPassedMultiplexer;
        private HistoryQuant packetsNotPassedMultiplexer;

        public SimpleAnalyzer()
        {
            quantsPassedBucket = new Queue<HistoryQuant>();
            quantsNotPassedBucket = new Queue<HistoryQuant>();
            quantsPassedMultiplexor = new Queue<HistoryQuant>();
            quantsNotPassedMultiplexor = new Queue<HistoryQuant>();

            packetsPassedBucket = new HistoryQuant();
            packetsNotPassedBucket = new HistoryQuant();
            packetsPassedMultiplexer = new HistoryQuant();
            packetsNotPassedMultiplexer = new HistoryQuant();

            quantHistorySize = 100;
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
            packetsPassedMultiplexer.Enqueue(packet);
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
            quantsPassedMultiplexor.Enqueue(packetsPassedMultiplexer);
            quantsNotPassedMultiplexor.Enqueue(packetsNotPassedMultiplexer);

            //убираем лишнюю инфу, если она есть
            while(quantsPassedBucket.Count > quantHistorySize)
            {
                quantsPassedBucket.Dequeue();
                quantsNotPassedBucket.Dequeue();
                quantsPassedMultiplexor.Dequeue();
                quantsNotPassedMultiplexor.Dequeue();
            }

            packetsPassedBucket = new HistoryQuant();
            packetsNotPassedBucket = new HistoryQuant();
            packetsPassedMultiplexer = new HistoryQuant();
            packetsNotPassedMultiplexer = new HistoryQuant();
        }
    }
}

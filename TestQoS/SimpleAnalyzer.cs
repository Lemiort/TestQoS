using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// История квантов
    /// </summary>
    public class HistoryQuant
    {
        public Queue<Packet> packets;

        /// <summary>
        /// общий размер пакетов в квант
        /// </summary>
        public ulong SummarySize
        {
            get;
            private set;
        }

        /// <summary>
        /// средний размер пакетов за последнее время
        /// </summary>
        public float AveragePacketsSize
        {
            get;
            set;
        }

        public HistoryQuant()
        {
            SummarySize = 0;
            packets = new Queue<Packet>();
        }

        public void Enqueue(Packet packet)
        {
            SummarySize += packet.Size;
            packets.Enqueue(packet);
        }

        public Packet Dequeue()
        {
            SummarySize -= packets.Peek().Size;
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

    /// <summary>
    /// Реализация анализатора
    /// </summary>
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


        /// <summary>
        /// инфа о последних N квантах
        /// </summary>
        public Queue<HistoryQuant> quantsPassed;

        /// <summary>
        /// инфа о псоледних N квантах
        /// </summary>
        public Queue<HistoryQuant> quantsNotPassed;

        /// <summary>
        /// инфа о пакетах за последний квант
        /// </summary>
        private HistoryQuant packetsPassed;

        /// <summary>
        /// инфа о пакетах за последний квант
        /// </summary>
        private HistoryQuant packetsNotPassed;

        public SimpleAnalyzer()
        {
            quantsPassed = new Queue<HistoryQuant>();
            quantsNotPassed = new Queue<HistoryQuant>();

            //создаём кванты для заполнения
            packetsPassed = new HistoryQuant();
            packetsNotPassed = new HistoryQuant();

            summaryNotPassedPacketsSize = 0;
            summaryPassedPacketsSize = 0;

            QuantHistorySize = 100;
        }

        /// <summary>
        /// сохраняет информацию о пакете, что прошёл через мультиплексор
        /// </summary>
        /// <param name="packet">пакет</param>
        public override void OnPassPacket(Packet packet)
        {
            packetsPassed.Enqueue(packet);
        }

        /// <summary>
        /// сохраняет информацию о пакете, что не прошёл через мультиплексор
        /// </summary>
        /// <param name="packet">пакет</param>
        public override void OnNotPassPacket(Packet packet)
        {
            packetsNotPassed.Enqueue(packet);
        }

        /// <summary>
        /// переходит к записи в следующий квант
        /// вызывать каждый квант времени
        /// </summary>
        public void Update()
        {
            //записываем среднюю статистику
            packetsPassed.AveragePacketsSize =
                (float)(summaryPassedPacketsSize + packetsPassed.SummarySize) / (float)quantsPassed.Count;
            packetsNotPassed.AveragePacketsSize =
                (float)(summaryNotPassedPacketsSize + packetsNotPassed.SummarySize) / (float)quantsNotPassed.Count;

            //записываем инфу о текущих квантах
            quantsPassed.Enqueue(packetsPassed);
            quantsNotPassed.Enqueue(packetsNotPassed);

            //записываем общее число прошедших и отброшенных байтов
            summaryPassedPacketsSize += packetsPassed.SummarySize;
            summaryNotPassedPacketsSize += packetsNotPassed.SummarySize;

            //убираем лишнюю инфу, если она есть
            while(quantsPassed.Count > QuantHistorySize)
            {

                //убираем информацию о канувших в лету байтах
                summaryPassedPacketsSize -= quantsPassed.Peek().SummarySize;
                summaryNotPassedPacketsSize -= quantsNotPassed.Peek().SummarySize;

                quantsPassed.Dequeue();
                quantsNotPassed.Dequeue();
            }

            packetsPassed = new HistoryQuant();
            packetsNotPassed = new HistoryQuant();
        }

        /// <summary>
        /// среднее число прошедших пакетов за окно
        /// </summary>
        /// <returns></returns>
        public float GetAveragePassedPacketsSize()
        {
            return (float)summaryPassedPacketsSize / (float)quantsPassed.Count();
        }

        /// <summary>
        /// среднее число отброшенных пакетов за окно
        /// </summary>
        /// <returns></returns>
        public float GetAverageNotPassedPacketsSize()
        {
            return (float)summaryNotPassedPacketsSize / (float)quantsNotPassed.Count();
        }

        /// <summary>
        /// конструктор копии
        /// </summary>
        /// <param name="prev"></param>
        SimpleAnalyzer(SimpleAnalyzer prev)
        {
            this.packetsNotPassed = prev.packetsNotPassed;
            this.packetsPassed = prev.packetsPassed;
            this.QuantHistorySize = prev.QuantHistorySize;
            this.quantsNotPassed = new Queue<HistoryQuant>();
            foreach( var quant in prev.quantsNotPassed)
            {
                this.quantsNotPassed.Enqueue(quant);
            }
            this.quantsPassed = new Queue<HistoryQuant>();
            foreach(var quant in prev.quantsPassed)
            {
                this.quantsPassed.Enqueue(quant);
            }
            this.summaryNotPassedPacketsSize = prev.summaryNotPassedPacketsSize;
            this.summaryPassedPacketsSize = prev.summaryPassedPacketsSize;
        }
    }
}

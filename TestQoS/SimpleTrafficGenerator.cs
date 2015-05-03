using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// Генератор трафика. Генерирует пакеты с размером, принадлежащим
    /// заданному промежутку [minPacketSize; maxPacketSize]. Период генерации принадлежит промежутку [minPacketSize; maxPacketSize]
    /// </summary>
    class SimpleTrafficGenerator : TrafficGenerator
    {
        Random rand;
        /// <summary>
        /// Минимальный размер пакета
        /// </summary>
        private int minPacketSize;

        /// <summary>
        /// Максимальный размер пакета
        /// </summary>
        private int maxPacketSize;

        /// <summary>
        /// минимальный промежуток времени между двумя пакетами (в квантах времени)
        /// </summary>
        private int minTimePeriod;

        /// <summary>
        /// максимальный промежуток времени между двумя пакетами (в квантах времени)
        /// </summary>
        private int maxTimePeriod;

        /// <summary>
        /// показывает сколько квантов осталось до следующей генерации пакета
        /// </summary>
        private int period;

        /// <summary>
        /// реализует переход от времени в миллисекундах, к времени в квантах, и на оборот
        /// </summary>
        private QuantizedTime time;

        /// <summary>
        /// возвращает пакет не раньше чем через minTimeperiod и не позже чем через maxTimePeriod
        /// </summary>
        /// <param name="time">реализует переход от времени в миллисекундах, к времени в квантах, и на оборот</param>
        /// <param name="minPacketSize">Минимальный размер пакета</param>
        /// <param name="maxPacketSize">Максимальный размер пакета</param>
        /// <param name="minTimePeriod">Минимальный промежуток времени между двумя пакетами (в миллисекундах - да вот хрен)</param>
        /// <param name="maxTimePeriod">Максимальный промежуток времени между двумя пакетами (в миллисекундах - да вот хрен)</param>
        public SimpleTrafficGenerator(QuantizedTime time, int minPacketSize, int maxPacketSize, double minTimePeriod, double maxTimePeriod)
        {
            rand = new Random((int)DateTime.Now.Ticks);
            // TODO: сделать "проверки на дурака" и тд
            this.time = time;
            this.minPacketSize = minPacketSize;
            this.maxPacketSize = maxPacketSize;
            this.minTimePeriod = this.time.FromAnalogToDigital(minTimePeriod);
            this.maxTimePeriod = this.time.FromAnalogToDigital(maxTimePeriod);

            period = GeneratePeriod();
        }

        public override Packet MakePacket()
        {
            // проверяем кончилось ли время текущего периода
            if(--period <= 0)
            {
                // отправляен пакет 
                period = GeneratePeriod();

                SimplePacket ret = new SimplePacket(GeneratePacketSize());

                onPacketGenerated(ret);

                return ret;           
            }
            else
            {
                // пакет не отправлен (думаю null логичнее пакета с размером 0,
                // но это стоит учитывать в дальнейшем)
                return null;
            }
        }

        /// <summary>
        /// Генерация периода
        /// </summary>
        /// <returns></returns>
        private int GeneratePeriod()
        {
            return rand.Next(minTimePeriod, maxTimePeriod);
        }

        /// <summary>
        /// Генерация размера пакета
        /// </summary>
        /// <returns></returns>
        private int GeneratePacketSize()
        {
            return rand.Next(minPacketSize, maxPacketSize);
        }

        /// <summary>
        /// делегат для события генерации пакета
        /// </summary>
        /// <param name="packet"></param>
        public delegate Packet Observer(Packet packet);

        /// <summary>
        /// событие генерации пакета
        /// </summary>
        public event Observer onPacketGenerated;
    }
}

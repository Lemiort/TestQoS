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
    public class SimpleTrafficGenerator : TrafficGenerator
    {
        Random rand;
        /// <summary>
        /// Минимальный размер пакета
        /// </summary>
        private uint minPacketSize;

        /// <summary>
        /// Максимальный размер пакета
        /// </summary>
        private uint maxPacketSize;

        /// <summary>
        /// минимальный промежуток времени между двумя пакетами (в квантах времени)
        /// </summary>
        private uint minTimePeriod;

        /// <summary>
        /// максимальный промежуток времени между двумя пакетами (в квантах времени)
        /// </summary>
        private uint maxTimePeriod;

        /// <summary>
        /// показывает сколько квантов осталось до следующей генерации пакета
        /// </summary>
        private uint period;

        /// <summary>
        /// реализует переход от времени в миллисекундах, к времени в квантах, и на оборот
        /// </summary>
        private QuantizedTime time;

        /// <summary>
        /// внутреннее зерно,  используется для детерминированности генераторов
        /// </summary>
        private int seed;

        /// <summary>
        /// возвращает пакет не раньше чем через minTimeperiod и не позже чем через maxTimePeriod
        /// </summary>
        /// <param name="time">реализует переход от времени в миллисекундах, к времени в квантах, и на оборот</param>
        /// <param name="minPacketSize">Минимальный размер пакета</param>
        /// <param name="maxPacketSize">Максимальный размер пакета</param>
        /// <param name="minTimePeriod">Минимальный промежуток времени между двумя пакетами в милисекундах</param>
        /// <param name="maxTimePeriod">Максимальный промежуток времени между двумя пакетами в милисекундах</param>
        public SimpleTrafficGenerator(QuantizedTime time, uint minPacketSize, uint maxPacketSize, 
            double minTimePeriod, double maxTimePeriod)
        {
            seed = (int)DateTime.Now.Ticks;
            // TODO: сделать "проверки на дурака" и тд
            this.time = time;
            this.minPacketSize = minPacketSize;
            this.maxPacketSize = maxPacketSize;
            this.minTimePeriod = this.time.FromAnalogToDigital(minTimePeriod);
            this.maxTimePeriod = this.time.FromAnalogToDigital(maxTimePeriod);

            period = GeneratePeriod();
        }


        /// <summary>
        /// конструктор копии
        /// </summary>
        /// <param name="previous"></param>
        public SimpleTrafficGenerator(SimpleTrafficGenerator previous)
        {
            this.rand = null;
            this.time = previous.time;
            this.minPacketSize = previous.minPacketSize;
            this.maxPacketSize = previous.maxPacketSize;
            this.minTimePeriod = previous.minTimePeriod;
            this.maxTimePeriod = previous.maxTimePeriod;
            this.period = previous.period;
            this.seed = previous.seed;
        }

        /// <summary>
        /// обновляет настройки генератора
        /// </summary>
        /// <param name="time">реализует переход от времени в миллисекундах, к времени в квантах, и на оборот</param>
        /// <param name="minPacketSize">Минимальный размер пакета</param>
        /// <param name="maxPacketSize">Максимальный размер пакета</param>
        /// <param name="minTimePeriod">Минимальный промежуток времени между двумя пакетами в милисекундах</param>
        /// <param name="maxTimePeriod">Максимальный промежуток времени между двумя пакетами в милисекундах</param>
        public void UpdateSettings(QuantizedTime time, uint minPacketSize, uint maxPacketSize, double minTimePeriod, double maxTimePeriod)
        {
            // TODO: сделать "проверки на дурака" и тд
            this.time = time;
            this.minPacketSize = minPacketSize;
            this.maxPacketSize = maxPacketSize;
            this.minTimePeriod = this.time.FromAnalogToDigital(minTimePeriod);
            this.maxTimePeriod = this.time.FromAnalogToDigital(maxTimePeriod);

            period = GeneratePeriod();
        }


        /// <summary>
        /// генерирует пакет, если прошло нужно число квантов
        /// вызывается каждый квант
        /// </summary>
        /// <returns></returns>
        public override Packet MakePacket()
        {
            // проверяем кончилось ли время текущего периода
            if(--period <= 0)
            {
                // отправляен пакет 
                period = GeneratePeriod();

                SimplePacket ret = new SimplePacket(GeneratePacketSize());

                if(onPacketGenerated != null)
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
        private uint GeneratePeriod()
        {
            rand = new Random(seed);
            seed = rand.Next();
            return (uint)rand.Next((int)minTimePeriod, (int)maxTimePeriod);
        }

        /// <summary>
        /// Генерация размера пакета
        /// </summary>
        /// <returns></returns>
        private uint GeneratePacketSize()
        {
            rand = new Random(seed);
            seed = rand.Next();
            return (uint)rand.Next((int)minPacketSize, (int)maxPacketSize);
        }

        /// <summary>
        /// делегат для события генерации пакета
        /// </summary>
        /// <param name="packet"></param>
        public delegate void Observer(Packet packet);

        /// <summary>
        /// событие генерации пакета
        /// </summary>
        public event Observer onPacketGenerated;

    }
}

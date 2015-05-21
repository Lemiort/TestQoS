using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// реализация фабрики QoS, создаёт вёдра и генераторы траффика
    /// </summary>
    public class SimpleTBQoS : QoS
    {

        /// <summary>
        /// общее время кватования
        /// </summary>
        public ModelTime qtime;

        /// <summary>
        /// период наблюдения
        /// </summary>
        private double observationPeriod;

        /// <summary>
        /// количество потоков/вёдер
        /// </summary>
        private uint numOfBuckets;

        /// <summary>
        /// из этого массива MakeTrafficGenerator 
        /// берёт minPacketSize
        /// </summary>
        List<uint> minPacketSizes;

        /// <summary>
        /// из этого массива MakeTrafficGenerator 
        /// берёт maxPacketSize
        /// </summary>
        List<uint> maxPacketSizes;

        /// <summary>
        /// из этого массива MakeTrafficGenerator 
        /// берёт minTimePeriod
        /// </summary>
        List<double> minTimePeriods;

        /// <summary>
        /// из этого массива MakeTrafficGenerator 
        /// берёт maxTimePeriod
        /// </summary>
        List<double> maxTimePeriods;

        public Analyzer analyzer;

        /// <summary>
        /// генераторы траффика
        /// </summary>
        List<TrafficGenerator> generators;

        /// <summary>
        /// вёдра, по 1 на каждый генератор
        /// </summary>
        List<TokenBuket> buckets;

        /// <summary>
        /// мультиплексор
        /// </summary>
        Multiplexer multiplexer;


        /// <summary>
        /// предыдущее время наблюдения
        /// </summary>
        long prevTime;


        /// <summary>
        /// Реализация фабричного метода MakeTokenBuket 
        /// </summary>
        /// <returns></returns>
        public override TokenBuket MakeTokenBuket()
        {
            return new SimpleTokenBuket(qtime  as QuantizedTime);
        }

        /// <summary>
        /// Реализация фабричного метода MakeTrafficGenerator
        /// TODO
        /// </summary>
        /// <returns></returns>
        public override TrafficGenerator MakeTrafficGenerator()
        {
            // minPacketSize
            if(minPacketSizes == null)
            {
                if(minPacketSizes.Count == 0)
                {
                    // TODO: Вызвать красивую ошибку
                    throw new NotImplementedException();
                }
            }
            // достаём первый элемент списка
            uint minPacketSize = this.minPacketSizes[0];
            // удаляем этот элемент из списка
            this.minPacketSizes.Remove(minPacketSize);

            // maxPacketSize
            if (maxPacketSizes == null)
            {
                if (maxPacketSizes.Count == 0)
                {
                    // TODO: Вызвать красивую ошибку
                    throw new NotImplementedException();
                }
            }
            // достаём первый элемент списка
            uint maxPacketSize = this.maxPacketSizes[0];
            // удаляем этот элемент из списка
            this.maxPacketSizes.Remove(maxPacketSize);

            // minTimePeriod
            if (minTimePeriods == null)
            {
                if (minTimePeriods.Count == 0)
                {
                    // TODO: Вызвать красивую ошибку
                    throw new NotImplementedException();
                }
            }
            // достаём первый элемент списка
            double minTimePeriod = this.minTimePeriods[0];
            // удаляем этот элемент из списка
            this.minTimePeriods.Remove(minTimePeriod);


            // maxTimePeriod
            if (maxTimePeriods == null)
            {
                if (maxTimePeriods.Count == 0)
                {
                    // TODO: Вызвать красивую ошибку
                    throw new NotImplementedException();
                }
            }
            // достаём первый элемент списка
            double maxTimePeriod = this.maxTimePeriods[0];
            // удаляем этот элемент из списка
            this.maxTimePeriods.Remove(maxTimePeriod);
            /// ^^^повторения кода, похорошему бы рефакторить^^^

            return new SimpleTrafficGenerator(qtime as QuantizedTime, 
                minPacketSize, maxPacketSize, minTimePeriod, maxTimePeriod);



            //return new SimpleTrafficGenerator( qtime as QuantizedTime, 64, 256, 200, 1500);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns>время квантования в 10 мс</returns>
        public override ModelTime MakeModelTime()
        {
            return new QuantizedTime(observationPeriod);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public override Packet MakePacket()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// создаёт экземпляр мульиплексера
        /// </summary>
        /// <returns>экземпляр мультплексора</returns>
        public override Multiplexer MakeMultiplexer()
        {
            return new SimpleMultiplexer( qtime as QuantizedTime);
            //throw new NotImplementedException();
        }

        public override Analyzer MakeAnalyzer()
        {
            return new SimpleAnalyzer();
            //throw new NotImplementedException();
        }



        /// <summary>
        /// инициализация 
        /// </summary>
        /// <param name="observationPeriod">время наблюдения в мс</param>
        /// <param name="numOfBuckets">число вёдер</param>
        /// <param name="minPacketSizes">минимальный размер пакета</param>
        /// <param name="maxPacketSizes">максимальный размер пакета</param>
        /// <param name="minTimePeriods">минимальное время между пакетами в мс</param>
        /// <param name="maxTimePeriods">максимальное время между пакетами в мс</param>
        public void Initializate(double observationPeriod, uint numOfBuckets,
            List<uint> minPacketSizes, List<uint> maxPacketSizes,
            List<double> minTimePeriods, List<double> maxTimePeriods)
        {
            this.observationPeriod = observationPeriod;
            this.numOfBuckets = numOfBuckets;
            this.minPacketSizes = minPacketSizes;
            this.maxPacketSizes = maxPacketSizes;
            this.minTimePeriods = minTimePeriods;
            this.maxTimePeriods = maxTimePeriods;

            //
            // Создание всех объектов
            //

            // время квантования
            qtime = this.MakeModelTime();

            //анализатор - вообще самая главная шишка, для него весь курсач
            analyzer = this.MakeAnalyzer();

            // генераторы трафика
            generators = new List<TrafficGenerator>();
            //и вёдра к ним
            buckets = new List<TokenBuket>();

            multiplexer = this.MakeMultiplexer();


            for (int i = 0; i < numOfBuckets; i++)
            {
                generators.Add(this.MakeTrafficGenerator());
                buckets.Add(this.MakeTokenBuket());

                //соединяем ведро с генератором
                (generators.Last() as SimpleTrafficGenerator).onPacketGenerated +=
                    (buckets.Last() as SimpleTokenBuket).ProcessPacket;

                //соединяем ведро с анализатором, иначе бешехельме, всё пропало, лови эксепшн
                (buckets.Last() as SimpleTokenBuket).onPacketPass +=
                    (analyzer as SimpleAnalyzer).OnBucketPassPacket;
                (buckets.Last() as SimpleTokenBuket).onPacketNotPass +=
                    (analyzer as SimpleAnalyzer).OnBucketNotPassPacket;

                //соединяем с мультиплексором
                (buckets.Last() as SimpleTokenBuket).onPacketPass +=
                    (multiplexer as SimpleMultiplexer).ProcessPacket;
            }
           

            //соединяем мультиплексор с анализатором, иначе событие не обработается и будет экспешн
            (multiplexer as SimpleMultiplexer).onPacketPass +=
                (analyzer as SimpleAnalyzer).OnMultiplexerPassPacket;
            (multiplexer as SimpleMultiplexer).onPacketNotPass +=
               (analyzer as SimpleAnalyzer).OnMultiplexerNotPassPacket;

            //начальное время
            prevTime = DateTime.Now.Ticks;
        }


        public void SetMultiplexerSpeed(ulong bytesPerDt)
        {
            multiplexer.bytesPerDt = bytesPerDt;
        }

        /// <summary>
        /// TODO
        /// Основной цикл
        /// </summary>
        public override void Run()
        {
            /*******************************************************/
            /************Создание всех объектов*********************/
            /*******************************************************/
            //время квантования
            if(generators == null)
            {
                throw new NullReferenceException();
            }
            /*******************************************************/
            /*******************************************************/
           


            /*******************************************************/
            /*********************Основной цикл*********************/
            /*******************************************************/
            //считаем изменение времени
            /*long*/ prevTime = DateTime.Now.Ticks;

            //пока не пришла команда завершаться
            while (true)
            {
                //считаем изменение времени
                long dt = DateTime.Now.Ticks - prevTime;

                //время в милисекундах
                TimeSpan time = new TimeSpan(dt);
                //собсно сам цикл
                if (time.Milliseconds >= (qtime as QuantizedTime).timeSlice)
                {
                    //generator.MakePacket();
                    foreach (TrafficGenerator generator in generators)
                    {
                        (generator as SimpleTrafficGenerator).MakePacket();
                    }
                    foreach (TokenBuket bucket in buckets)
                    {
                        (bucket as SimpleTokenBuket).Update();
                    }

                    (multiplexer as SimpleMultiplexer).Update();

                    (analyzer as SimpleAnalyzer).Update();
                    (analyzer as SimpleAnalyzer).PrintFirstQuantInfo();

                    prevTime = DateTime.Now.Ticks;

                }
            }
            /*******************************************************/
            /*******************************************************/
        }


        /// <summary>
        /// Основной цикл
        /// </summary>
        public void MakeTick()
        {
            /*******************************************************/
            /************Создание всех объектов*********************/
            /*******************************************************/
            //время квантования
            if (generators == null)
            {
                throw new NullReferenceException();
            }
            /*******************************************************/
            /*******************************************************/



            /*******************************************************/
            /*********************Основной цикл*********************/
            /*******************************************************/
            //считаем изменение времени
            /*long prevTime = DateTime.Now.Ticks;*/

            //бывший основной цикл
            {
                //считаем изменение времени
                long dt = DateTime.Now.Ticks - prevTime;

                //время в милисекундах
                TimeSpan time = new TimeSpan(dt);
                //собсно сам цикл
                if (time.Milliseconds >= (qtime as QuantizedTime).timeSlice)
                {
                    //generator.MakePacket();
                    foreach (TrafficGenerator generator in generators)
                    {
                        (generator as SimpleTrafficGenerator).MakePacket();
                    }
                    foreach (TokenBuket bucket in buckets)
                    {
                        (bucket as SimpleTokenBuket).Update();
                    }

                    (multiplexer as SimpleMultiplexer).Update();

                    (analyzer as SimpleAnalyzer).Update();
                    (analyzer as SimpleAnalyzer).PrintFirstQuantInfo();

                    prevTime = DateTime.Now.Ticks;

                }
            }
            /*******************************************************/
            /*******************************************************/
        }

        


        /// <summary>
        /// обработчик пакета, что прошёл через ведро
        /// </summary>
        /// <param name="packet">пакет</param>
        public void OnPacketPass(Packet packet)
        {
            Console.WriteLine("Packet passed! {0}", packet.Size);
        }

        /// <summary>
        /// обработчик отброшенного пакета
        /// </summary>
        /// <param name="packet">пакет</param>
        public void OnPacketNotPass(Packet packet)
        {
            Console.WriteLine("Lost packet {0}", packet.Size);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// реализация QoS, отвечает за инициализацию 
    /// системы и порождение частей её частей
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


        /// <summary>
        /// Вес потерь на корзинах
        /// </summary>
        public List<uint> TokenBuketsWeights { get; set; }

        /// <summary>
        /// Вес очереди мультиплексора
        /// </summary>
        public uint QueueWeight { get; set; }

        /// <summary>
        /// Вес потерь на мультиплексоре
        /// </summary>
        public uint MultiplexerWeight { get; set; }

        /// <summary>
        /// анализатор мультиплексора
        /// </summary>
        public Analyzer multiplexorAnalyzer;
        
        /// <summary>
        /// анализатор всех ведёр
        /// </summary>
        public Analyzer bucketsAnalyzer;

        /// <summary>
        /// генераторы траффика
        /// </summary>
        protected List<TrafficGenerator> generators;

        /// <summary>
        /// анализаторы траффика
        /// </summary>
        protected List<Analyzer> generatorAnalyzers;

        /// <summary>
        /// вёдра, по 1 на каждый генератор
        /// </summary>
        protected List<TokenBuket> buckets;

        /// <summary>
        /// анализаторы по 1 шт. на ведро
        /// </summary>
        protected List<Analyzer> bucketAnalyzers;

        /// <summary>
        /// мультиплексор
        /// </summary>
        protected Multiplexer multiplexer;

        /// <summary>
        /// предыдущее время наблюдения
        /// </summary>
        protected long prevTime;

        /// <summary>
        /// история прошедших байтов
        /// </summary>
        public Queue<UInt64> multiplexorBytes;

        /// <summary>
        /// история средних значений байтв
        /// </summary>
        public Queue<float> multiplexorAverageBytes;

        /// <summary>
        /// сумма байтов за историю
        /// </summary>
        public UInt64 MultiplexorSummaryBytes
        {
            get;
            set;
        }

        /// <summary>
        /// размер истории
        /// </summary>
        protected int historySize;


        /// <summary>
        /// Максимальные скорости поступления токенов в корзины
        /// </summary>
        protected List<float> maxTokensPerDts;

        /// <summary>
        /// Реализация фабричного метода MakeTokenBuket 
        /// </summary>
        /// <returns></returns>
        public override TokenBuket MakeTokenBuket()
        {
            return new SimpleTokenBucket(qtime  as QuantizedTime);
        }

        /// <summary>
        /// сид
        /// </summary>
        private int seed;

        /// <summary>
        /// генератор случайных чисел
        /// </summary>
        private Random rand;

        /// <summary>
        /// Реализация фабричного метода MakeTrafficGenerator
        /// </summary>
        /// <returns></returns>
        public override TrafficGenerator MakeTrafficGenerator()
        {
            // minPacketSize
            if(minPacketSizes == null)
            {
                if(minPacketSizes.Count == 0)
                {
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
                    // Вызвать красивую ошибку
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
                    throw new NotImplementedException();
                }
            }
            // достаём первый элемент списка
            double maxTimePeriod = this.maxTimePeriods[0];
            // удаляем этот элемент из списка
            this.maxTimePeriods.Remove(maxTimePeriod);
            
            rand = new Random(seed);
            seed = rand.Next();
            return new SimpleTrafficGenerator(qtime as QuantizedTime, 
                minPacketSize, maxPacketSize, minTimePeriod, maxTimePeriod, seed);
        }

        /// <summary>
        /// Реализация фабричного метода MakeModelTime
        /// </summary>
        /// <returns>время квантования в 10 мс</returns>
        public override ModelTime MakeModelTime()
        {
            return new QuantizedTime(observationPeriod);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Реализация фабричного метода MakePacket
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
           SimpleAnalyzer ret =  new SimpleAnalyzer();
           ret.QuantHistorySize = historySize;
           return ret;
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
        /// <param name="_histrorySize">информацию о скольки последних байтах храним</param>
        /// <param name="maxTokensCounts">Размеры вёдер</param>
        /// <param name="_seed">seed</param>
        public void Initializate(double observationPeriod, uint numOfBuckets,
            List<uint> minPacketSizes, List<uint> maxPacketSizes,
            List<double> minTimePeriods, List<double> maxTimePeriods,
            int _histrorySize, List<float> maxTokensCounts, int _seed)
        {
            this.observationPeriod = observationPeriod;
            this.numOfBuckets = numOfBuckets;
            this.minPacketSizes = minPacketSizes;
            this.maxPacketSizes = maxPacketSizes;
            this.minTimePeriods = minTimePeriods;
            this.maxTimePeriods = maxTimePeriods;
            this.seed = _seed;

            //
            // Создание всех объектов
            //

            //размер истории, чтобы анализаторы правильно создались
            historySize = _histrorySize;

            //история байтов
            multiplexorBytes = new Queue<ulong>();
            multiplexorAverageBytes = new Queue<float>();
            MultiplexorSummaryBytes = 0;

            // время квантования
            qtime = this.MakeModelTime();

            //анализатор - вообще самая главная шишка, для него весь курсач
            multiplexorAnalyzer = this.MakeAnalyzer();
            bucketsAnalyzer = this.MakeAnalyzer();

            // генераторы трафика
            generators = new List<TrafficGenerator>();
            //анализаторы генераторов
            generatorAnalyzers = new List<Analyzer>();
            //и вёдра к ним
            buckets = new List<TokenBuket>();
            //и анализаторы к вёдрам
            bucketAnalyzers = new List<Analyzer>();

            multiplexer = this.MakeMultiplexer();

            for (int i = 0; i < numOfBuckets; i++)
            {
                generators.Add(this.MakeTrafficGenerator());
                generatorAnalyzers.Add(this.MakeAnalyzer());
                buckets.Add(this.MakeTokenBuket());
                (buckets.Last() as SimpleTokenBucket).MaxTokensCount = maxTokensCounts[i];
                bucketAnalyzers.Add(this.MakeAnalyzer());

                //соединяем ведро с генератором
                (generators.Last() as SimpleTrafficGenerator).onPacketGenerated +=
                    (buckets.Last() as SimpleTokenBucket).ProcessPacket;
                //и генератор с анализатором
                (generators.Last() as SimpleTrafficGenerator).onPacketGenerated +=
                    (generatorAnalyzers.Last() as SimpleAnalyzer).OnPassPacket;

                //соединяем ведро с анализатором, иначе бешехельме, всё пропало, лови эксепшн
                (buckets.Last() as SimpleTokenBucket).onPacketPass +=
                    (bucketsAnalyzer as SimpleAnalyzer).OnPassPacket;
                (buckets.Last() as SimpleTokenBucket).onPacketNotPass +=
                    (bucketsAnalyzer as SimpleAnalyzer).OnNotPassPacket;

                //соединяем с мультиплексором
                (buckets.Last() as SimpleTokenBucket).onPacketPass +=
                    (multiplexer as SimpleMultiplexer).ProcessPacket;
            }
           
            //соединяем мультиплексор с анализатором, иначе событие не обработается и будет экспешн
            (multiplexer as SimpleMultiplexer).onPacketPass +=
                (multiplexorAnalyzer as SimpleAnalyzer).OnPassPacket;
            (multiplexer as SimpleMultiplexer).onPacketNotPass +=
               (multiplexorAnalyzer as SimpleAnalyzer).OnNotPassPacket;

            //начальное время
            prevTime = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Инициализация пропускноу способности мультиплексора
        /// </summary>
        /// <param name="bytesPerDt"></param>
        public void SetMultiplexer(ulong bytesPerDt, ulong maxQueueSize)
        {
            multiplexer.BytesPerDt = bytesPerDt;
            multiplexer.MaxQueueSize = maxQueueSize;
            if (multiplexer.BytesPerDt > multiplexer.MaxQueueSize)
                multiplexer.MaxQueueSize = multiplexer.BytesPerDt;            
        }       

        /// <summary>
        /// Основной цикл
        /// </summary>
        public override void Run()
        {
            // Создание времени квантования
            if(generators == null)
            {
                throw new NullReferenceException();
            }

            //считаем изменение времени
            prevTime = DateTime.Now.Ticks;

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
                        (bucket as SimpleTokenBucket).Update();
                    }

                    (multiplexer as SimpleMultiplexer).Update();

                    (multiplexorAnalyzer as SimpleAnalyzer).Update();

                    prevTime = DateTime.Now.Ticks;

                }
            }
        }

        /// <summary>
        /// Основной цикл
        /// </summary>
        public override void MakeTick()
        {
            // Создание времени квантования
            if (generators == null)
            {
                throw new NullReferenceException();
            }

            //считаем изменение времени
            long dt = DateTime.Now.Ticks - prevTime;

            //время в милисекундах
            TimeSpan time = new TimeSpan(dt);
            if (time.Milliseconds >= (qtime as QuantizedTime).timeSlice)
            {
                //записываем текущие данные о токенах в квант
                List<float> currentTokensPerDts = new List<float>();
                for (int i = 0; i < buckets.Count; i++)
                {
                    currentTokensPerDts.Add((buckets[i] as SimpleTokenBucket).TokensPerDt);
                }
                //ищем оптимальные значения
                List<float> optimalTokensPerDts = this.OptimalTokensPerDts(currentTokensPerDts);

                //применяем оптимальные значения
                for (int i = 0; i < buckets.Count; i++)
                {
                    (buckets[i] as SimpleTokenBucket).TokensPerDt = optimalTokensPerDts[i];
                }

                //генерим пакеты
                foreach (TrafficGenerator generator in generators)
                {
                    (generator as SimpleTrafficGenerator).MakePacket();
                }
                //заносим инфу в анализатор
                foreach (Analyzer analyzer in generatorAnalyzers)
                {
                    (analyzer as SimpleAnalyzer).Update();
                }

                //обрабатываем пакеты и заносим инфу в анализатор
                for (int i = 0; i < buckets.Count; i++)
                {
                    (buckets[i] as SimpleTokenBucket).Update();
                    (bucketAnalyzers[i] as SimpleAnalyzer).Update();
                }

                //запускаем мультиплексор
                (multiplexer as SimpleMultiplexer).Update();
                //анализируем результаты работы
                (multiplexorAnalyzer as SimpleAnalyzer).Update();
                (bucketsAnalyzer as SimpleAnalyzer).Update();

                //история байтов мультиплексора
                multiplexorBytes.Enqueue((multiplexer as SimpleMultiplexer).GetLastThroughputSize());
                //сумма байтов за историю
                MultiplexorSummaryBytes += (multiplexer as SimpleMultiplexer).GetLastThroughputSize();
                multiplexorAverageBytes.Enqueue((float)MultiplexorSummaryBytes / (float)multiplexorBytes.Count);

                if (multiplexorBytes.Count > historySize)
                {
                    //убираем из истории байт, а так же из суммарного размера
                    MultiplexorSummaryBytes -= multiplexorBytes.Dequeue();
                    multiplexorAverageBytes.Dequeue();
                }

                prevTime = DateTime.Now.Ticks;
            }
        }

        /// <summary>
        /// Целевая функция
        /// </summary>
        /// <param name="tokensPerDts"></param>
        /// <returns></returns>
        public uint ObjectiveFunction(List<float> tokensPerDts)
        {
            List<SimpleTrafficGenerator> generatorsCopy = new List<SimpleTrafficGenerator>();

            //копируем генераторы
            foreach (var generator in generators)
            {
                generatorsCopy.Add(
                    new SimpleTrafficGenerator(generator as SimpleTrafficGenerator));
            }

            //копируем вёдра
            List<SimpleTokenBucket> bucketsCopy = new List<SimpleTokenBucket>();
            foreach (var bucket in buckets)
            {
                bucketsCopy.Add(
                    new SimpleTokenBucket(bucket as SimpleTokenBucket));
            }

            //копируем мультиплексор
            SimpleMultiplexer multiplexerCopy =
                new SimpleMultiplexer(multiplexer as SimpleMultiplexer);

            //анализаторы для сбора инфы
            SimpleAnalyzer multiplexorAnalyzerCopy = new SimpleAnalyzer();
            //анализаторы ведёр
            List<SimpleAnalyzer> bucketAnalyzersCopy = new List<SimpleAnalyzer>();

            //устанавливаем в вёдра параметры
            for (int i = 0; i < tokensPerDts.Count; i++)
            {
                //соединяем генераторы с вёдрами
                generatorsCopy[i].onPacketGenerated += bucketsCopy[i].ProcessPacket;

                //заполняем вёдра параметарми из аргумента
                bucketsCopy[i].TokensPerDt = tokensPerDts[i];

                //соединяем с мультиплексором
                bucketsCopy[i].onPacketPass += multiplexerCopy.ProcessPacket;

                //считаем потери на корзинах

                //индивидуальный анализатор
                bucketAnalyzersCopy.Add(new SimpleAnalyzer());
                //инициализируем его
                bucketAnalyzersCopy[i].QuantHistorySize = multiplexorAnalyzerCopy.QuantHistorySize;
                //соединяем анализатор с ведром
                bucketsCopy[i].onPacketNotPass += bucketAnalyzersCopy[i].OnNotPassPacket;
            }
            //считаем потери на мультиплексоре
            multiplexerCopy.onPacketNotPass += multiplexorAnalyzerCopy.OnNotPassPacket;

            //генерация
            for (int i = 0; i < generatorsCopy.Count; i++)
            {
                generatorsCopy[i].MakePacket();
            }

            //обработка вёдрами
            foreach (var bucket in bucketsCopy)
            {
                bucket.Update();
            }
            //анализ ведёр
            foreach (var analyzer in bucketAnalyzersCopy)
            {
                analyzer.Update();
            }

            //обработка мультиплексором
            multiplexerCopy.Update();
            //анализ мультипоексора
            multiplexorAnalyzerCopy.Update();

            //потери мультипелксора*вес 
            uint ret = ((uint)multiplexorAnalyzerCopy.GetAverageNotPassedPacketsSize()) * this.QueueWeight;
            //+ очередь мультиплексора*вес
            ret += (uint)multiplexerCopy.GetQueueSize() * this.MultiplexerWeight;
            //потери на вёдрах * вес
            for (int i = 0; i < bucketAnalyzersCopy.Count; i++)
            {
                ret += (uint)bucketAnalyzersCopy[i].GetAverageNotPassedPacketsSize()
                    * (uint)this.TokenBuketsWeights[i];
            }

            return ret;
        }

        protected virtual List<float> OptimalTokensPerDts(List<float> firstTokensPerDts)
        {
            return firstTokensPerDts;
        }


        /// <summary>
        /// Инициализация ограничения сверху для скорости поступления
        /// маркеров в маркерные корзины.
        /// </summary>
        protected void InitMaxTokensPerDts()
        {
            maxTokensPerDts = new List<float>();

            for (int i = 0; i < buckets.Count; i++)
            {
                maxTokensPerDts.Add(
                    (buckets[i] as SimpleTokenBucket).MaxTokensCount -
                    (buckets[i] as SimpleTokenBucket).GetTokensCount()
                    );
            }
        }
    }
}

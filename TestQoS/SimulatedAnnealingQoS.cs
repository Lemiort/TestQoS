using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// это пиздец, товарищи
    /// </summary>
    public class SimulatedAnnealingQoS : SimpleTBQoS
    {
        private Random rand;

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
        /// Начальная температура чем она выше,
        /// тем дольше работает алгоритм и больше вероятность
        /// правельного решения
        /// </summary>
        private double initalTemperature;

        /// <summary>
        /// текущая тампература
        /// </summary>
        private double temperature;

        /// <summary>
        /// минимальная температура
        /// </summary>
        private double minTemperature;

        /// <summary>
        /// Максимальные скорости поступления токенов в корзины
        /// </summary>
        private List<float> maxTokensPerDts;

        /// <summary>
        /// Функция находит очередные параметры для оптимазируемой функции.
        /// Тут творится какая то магия, после которой появляются новые скорости 
        /// поступления жетонов в корзины. Работа функции зависит от температуры,
        /// чем она ниже, тем ближе новый вектор к предыдущему
        /// </summary>
        /// <param name="tokensPerDts">Вектор текущих скоростей поступления токенов в корзины</param>
        /// <param name="elementNum">Элемент значение которого следует поменять</param>
        /// <returns>Вектор новых скоростей поступления токенов в корзины</returns>
        private List<float> SetNextTBSpeedValue(List<float> tokensPerDts, int elementNum)
        {
            List<float> result;                       
            result = new List<float>();
            for(int i = 0; i < tokensPerDts.Count; i++)
            {
                // генерация нового значения скорости поступления токенов i-й корзины               
                if (i == elementNum)
                {
                    // генерация новой скорости для выбраной корзины
                    float newTokensPerDt;
                    do
                    {
                        // сохраняем старое значение
                        newTokensPerDt = tokensPerDts[i];
                        // ганерируем приращение
                        float delta = maxTokensPerDts[i]*(float)rand.NextDouble()*(float)temperature;
                        
                        // к старому значению прибавляем или вычитаем это приращение
                        if(rand.Next(2)==0)
                        {
                            newTokensPerDt += delta;
                        }
                        else
                        {
                            newTokensPerDt -= delta;
                        }

                        // проверка на принадлежность поласти значений
                        if(newTokensPerDt < 0)
                        {
                            newTokensPerDt = 0;
                        }
                        if(newTokensPerDt > maxTokensPerDts[i])
                        {
                            newTokensPerDt = maxTokensPerDts[i];
                        }
                    }
                    while (tokensPerDts[i] == newTokensPerDt);
                    result.Add(newTokensPerDt);
                }
                else
                {
                    // для остальных корзин значение оставляем преждним
                    result.Add(tokensPerDts[i]);
                }
            }                   

            return result;
        }

        /// <summary>
        /// Метод понижения температуры
        /// TODO хз какой закое юзать правильней и оптимальней, 
        /// пока юзается линейный. сделать более вменябельный закон
        /// </summary>
        /// <param name="i">номер итерации</param>
        private void DecreaseTemperature(int i)
        {
            temperature = initalTemperature * 0.1 / i;
        }

        /// <summary>
        /// вероятность принятия нового состояния 
        /// (число от 0 до 1)
        /// </summary>
        /// <param name="newState">значение целевой функции, в новом состоянии</param>
        /// <param name="oldState">значение целевой функции, в старом состоянии</param>
        /// <returns></returns>
        private double NewStateProbability(uint newState,uint oldState)
        {
            // если новое состояние лучше старого, его вероятность 1
            if (newState < oldState)
            {
                return 1.0;
            }
            // разница состояний
            uint stateDifference = newState - oldState;

            // вероятность выбора худшего результата
            double probability = Math.Exp((-1.0 * (double)stateDifference) / temperature);
            return probability;
        }

        /// <summary>
        /// Целевая функция
        /// </summary>
        /// <param name="tokensPerDts"></param>
        /// <returns></returns>
        private uint ObjectiveFunction(List<float> tokensPerDts)
        {
            List<SimpleTrafficGenerator> generatorsCopy = new List<SimpleTrafficGenerator>();
            
            //копируем генераторы
            foreach(var generator in generators)
            {
                generatorsCopy.Add(
                    new SimpleTrafficGenerator(generator as SimpleTrafficGenerator));
            }

            //копируем вёдра
            List<SimpleTokenBucket> bucketsCopy = new List<SimpleTokenBucket>();
            foreach(var bucket in buckets)
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

                //считаем потери на вёдрах

                //индивидуальный анализатор
                bucketAnalyzersCopy.Add( new SimpleAnalyzer());
                //инициализируем его
                bucketAnalyzersCopy[i].QuantHistorySize = multiplexorAnalyzerCopy.QuantHistorySize;
                //соединяем анализатор с ведром
                bucketsCopy[i].onPacketNotPass +=bucketAnalyzersCopy[i].OnNotPassPacket;
            }
            //считаем потери на мультиплексоре
            multiplexerCopy.onPacketNotPass += multiplexorAnalyzerCopy.OnNotPassPacket;

            //////////////////////////////////////////
            ///**************Собственно проход*******/
            ///
            //генерация
            for (int i = 0; i < generatorsCopy.Count; i++ )
            {
                Packet t1, t2;
                t1 = generatorsCopy[i].MakePacket();
                //t2 = generatorsCopy[i].MakePacket();
                /*if(t1 != null && t2 != null)
                    throw new Exception(t1.Size.ToString() + " " + t2.Size.ToString());*/
            }

                //обработка вёдрами
                foreach (var bucket in bucketsCopy)
                {
                    bucket.Update();
                }
            //анализ ведёр
            foreach(var analyzer in bucketAnalyzersCopy)
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
            ret +=  (uint)multiplexerCopy.GetQueueSize() * this.MultiplexerWeight;
            //потери на вёдрах * вес
            for(int i = 0; i < bucketAnalyzersCopy.Count; i++)
            {                
                ret += (uint)bucketAnalyzersCopy[i].GetAverageNotPassedPacketsSize()
                    * (uint)this.TokenBuketsWeights[i];
            }
            /*if(ret != 0)
                throw new Exception();*/

           return ret;
        }

        /// <summary>
        /// Инициализация ограничения сверху для скорости поступления
        /// маркеров в маркерные корзины, зависит от параметром мультиплексора
        /// и приоритетов.
        /// TODO
        /// </summary>
        private void InitMaxTokensPerDts()
        {
            maxTokensPerDts = new List<float>();

            for (int i = 0; i < buckets.Count; i++)
            {
                maxTokensPerDts.Add(
                    (buckets[i] as SimpleTokenBucket).MaxTokensCount -
                    (buckets[i] as SimpleTokenBucket).GetTokensCount()
                    );
            }

            //TODO: пофиксить этот код
      /*      for (int i = 0; i < buckets.Count; i++ )
            {
                maxTokensPerDts.Add(Math.Min(
                    (buckets[i] as SimpleTokenBucket).MaxTokensCount -
                    (buckets[i] as SimpleTokenBucket).GetTokensCount(),
                    lastPacketsSize[i] - 
                    (buckets[i] as SimpleTokenBucket).GetTokensCount() ) );
            }*/
                
        }

        /// <summary>
        /// Поиск оптимальных скоростей поступления
        /// маркеров в маркерные корзины методом имитации отжига.
        /// </summary>
        /// <param name="firstTokensPerDts">начальный набор состояний</param>
        /// <returns>оптимальное состояние</returns>
        private List<float> OptimalTokensPerDts(List<float> firstTokensPerDts)
        {
            // инициализация параметров алгоритма
            this.initalTemperature = 10;
            this.minTemperature = 1e-4;
            List<float> oldTokensPerDts = firstTokensPerDts;
            List<float> newTokensPerDts;
            rand = new Random((int)DateTime.Now.Ticks);
            int i = 0;
            int iMax = 1000; // защита от зацикливания
            this.temperature = this.initalTemperature;
            this.InitMaxTokensPerDts();

            while((temperature > minTemperature) && (i++ < iMax))
            {
                // поэлементный обход
                for (int j = 0; j < oldTokensPerDts.Count; j++)
                {
                    // переход в новое состояние по j-му элементу
                    newTokensPerDts = this.SetNextTBSpeedValue(oldTokensPerDts, j);
                    uint newState = this.ObjectiveFunction(newTokensPerDts);
                    uint oldState = this.ObjectiveFunction(oldTokensPerDts);
                    double probability = this.NewStateProbability(newState, oldState);

                    // кидаем кубик
                    double value = rand.NextDouble();
                    // если попали в зону
                    if (value <= probability)
                    {
                        // совершаем переход
                        oldTokensPerDts = newTokensPerDts;
                    }
                }

                // понижаем температуру 
                this.DecreaseTemperature(i);
            }


            return oldTokensPerDts;
        }

        public override void MakeTick()
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
                        (buckets.ElementAt(i) as SimpleTokenBucket).Update();
                        (bucketAnalyzers.ElementAt(i) as SimpleAnalyzer).Update();
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
            /*******************************************************/
            /*******************************************************/

        }
    }
}

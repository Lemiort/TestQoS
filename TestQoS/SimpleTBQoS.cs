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
    class SimpleTBQoS : QoS
    {
        /// <summary>
        /// общее время кватования
        /// </summary>
        public ModelTime qtime;

        public Analyzer analyzer;


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
            ///TODO
            return new SimpleTrafficGenerator( qtime as QuantizedTime, 64, 256, 200, 1500);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns>время квантования в 10 мс</returns>
        public override ModelTime MakeModelTime()
        {
            return new QuantizedTime(10.0);
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
        /// TODO
        /// Основной цикл
        /// </summary>
        public override void Run()
        {
            //throw new NotImplementedException();
            //создаём экземпляры объектов

            /*******************************************************/
            /************Создание всех объектов*********************/
            /*******************************************************/
            //время квантования
            qtime = this.MakeModelTime();

            TrafficGenerator generator = this.MakeTrafficGenerator();
            TokenBuket bucket = this.MakeTokenBuket();
            Multiplexer multiplexer = this.MakeMultiplexer();
            Analyzer analyzer = this.MakeAnalyzer();
            /*******************************************************/
            /*******************************************************/


            /******************************************************/
            /**********Настройка параметров************************/
            /******************************************************/
            //TODO: сделать эту настройку где-то внутри
            (bucket as SimpleTokenBuket).TokensPerDt = 100.0F;
            /*******************************************************/
            /*******************************************************/


            /*******************************************************/
            /*******Соединение всех объектов между собой************/
            /*******************************************************/
            //заставляем обрабатывать каждый сгенерированный пакет
            (generator as SimpleTrafficGenerator).onPacketGenerated += (bucket as SimpleTokenBuket).ProcessPacket;

            //костыль, костыль и ещё раз костыль
            //обработчик прошедшего и непрошедшего пакета в ведре
            (bucket as SimpleTokenBuket).onPacketPass += (multiplexer as SimpleMultiplexer).ProcessPacket;
            //(bucket as SimpleTokenBuket).onPacketNotPass += this.OnPacketNotPass;

            //записуем всё в анализатор
            (bucket as SimpleTokenBuket).onPacketPass += (analyzer as SimpleAnalyzer).OnBucketPassPacket;
            (bucket as SimpleTokenBuket).onPacketNotPass += (analyzer as SimpleAnalyzer).OnBucketNotPassPacket;

            //обработка мултиплексора
            (multiplexer as SimpleMultiplexer).onPacketPass += (analyzer as SimpleAnalyzer).OnMultiplexerPassPacket;
            (multiplexer as SimpleMultiplexer).onPacketNotPass += (analyzer as SimpleAnalyzer).OnMultiplexerNotPassPacket;

            /*******************************************************/
            /*******************************************************/


            /*******************************************************/
            /*********************Основной цикл*********************/
            /*******************************************************/
            //считаем изменение времени
            long prevTime = DateTime.Now.Ticks;
            while (true)
            {
                //считаем изменение времени
                long dt = DateTime.Now.Ticks - prevTime;

                //время в милисекундах
                TimeSpan time = new TimeSpan(dt);
                //собсно сам цикл
                if (time.Milliseconds >= (qtime as QuantizedTime).timeSlice)
                {
                    generator.MakePacket();
                    (bucket as SimpleTokenBuket).Update();
                    (multiplexer as SimpleMultiplexer).Update();

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

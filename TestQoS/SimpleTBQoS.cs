﻿using System;
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
        /// Реализация фабричного метода MakeTokenBuket 
        /// </summary>
        /// <returns></returns>
        public override TokenBuket MakeTokenBuket()
        {
            return new SimpleTokenBuket();
        }

        /// <summary>
        /// Реализация фабричного метода MakeTrafficGenerator
        /// TODO
        /// </summary>
        /// <returns></returns>
        public override TrafficGenerator MakeTrafficGenerator()
        {
            ///TODO: негодный конструктор, время нихера ни в милисекундах
            return new SimpleTrafficGenerator(new QuantizedTime(0.1), 64, 256, 200000, 1000000);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public override ModelTime MakeModelTime()
        {
            throw new NotImplementedException();
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
        /// TODO
        /// Основной цикл
        /// </summary>
        public override void Run()
        {
            //throw new NotImplementedException();
            //создаём экземпляры объектов
            TrafficGenerator generator = this.MakeTrafficGenerator();
            TokenBuket bucket = this.MakeTokenBuket();

            //TODO: сделать эту настройку где-то внутри
            (bucket as SimpleTokenBuket).TokensPerMs = 0.1F;

            //заставляем обрабатывать каждый сгенерированный пакет
            (generator as SimpleTrafficGenerator).onPacketGenerated += (bucket as SimpleTokenBuket).ProcessPacket;

            //костыль, костыль и ещё раз костыль
            //обработчик прошедшего и непрошедшего пакета
            (bucket as SimpleTokenBuket).onPacketPass += this.OnPacketPass;
            (bucket as SimpleTokenBuket).onPacketNotPass += this.OnPacketNotPass;

            while (true)
            {
                generator.MakePacket();
            }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    class Program
    {
        static void Main(string[] args)
        {
            QoS qos = new SimpleTBQoS();

            //создаём экземпляры объектов
            TrafficGenerator generator = qos.MakeTrafficGenerator();
            TokenBuket bucket = qos.MakeTokenBuket();

            //TODO: сделать эту настройку где-то внутри
            (bucket as SimpleTokenBuket).TokensPerMs = 0.000005F;

            //заставляем обрабатывать каждый сгенерированный пакет
            (generator as SimpleTrafficGenerator).onPacketGenerated += (bucket as SimpleTokenBuket).ProcessPacket;

            //костыль, костыль и ещё раз костыль
            //обработчик прошедшего и непрошедшего пакета
            (bucket as SimpleTokenBuket).onPacketPass += (qos as SimpleTBQoS).OnPacketPass;
            (bucket as SimpleTokenBuket).onPacketNotPass += (qos as SimpleTBQoS).OnPacketNotPass;

            while(true)
            {
                generator.MakePacket();
            }
        }
    }
}

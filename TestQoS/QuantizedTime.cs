using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// Реализация временной модели.
    /// </summary>
    public class QuantizedTime : ModelTime
    {
        /// <summary>
        /// промежуток наблюдения (в миллисекундах)
        /// </summary>
        public double timeSlice
        {
            get;
            private set;
        }

        /// <summary>
        /// создаёт время квантования
        /// </summary>
        /// <param name="timeSlice">время в милисекундах</param>
        public QuantizedTime(double timeSlice)
        {
            this.timeSlice = timeSlice;
        }

        /// <summary>
        /// конвертация миллисекунд в кванты
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public uint FromAnalogToDigital(double time)
        {
            double eps = 1e-8;
            double realDiv = time / timeSlice;
            uint intDiv = (uint)(time / timeSlice);
            if (Math.Abs(realDiv - intDiv) > eps) ++intDiv;

            return intDiv;
        }

        /// <summary>
        /// конвертация квантов в миллисекунды
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public double FromDigitalToAnalog(uint time)
        {
            return time * timeSlice;
        }
    }
}

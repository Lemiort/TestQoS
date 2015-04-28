using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    class QuantizedTime : ModelTime
    {
        // TODO: возмодно эту переменную стоит переименовать (или/и сдалать только для чтения)
        /// <summary>
        /// промежуток наблюдения (в миллисекундах)
        /// </summary>
        public double timeSlice;

        /// <summary>
        /// конструктор времени квантовани
        /// </summary>
        /// <param name="timeSlice">промежуток намблюдения dt в милисекундах</param>
        public QuantizedTime(double timeSlice)
        {
            this.timeSlice = timeSlice;
        }

        /// <summary>
        /// перевод времени
        /// TODO: дописать норм коммент
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public int FromAnalogToDigital(double time)
        {
            return (int)(time / timeSlice) + 1;
        }

        /// <summary>
        /// перевод времени
        /// TODO: дописать норм коммент
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public double FromDigitalToAnalog(int time)
        {
            return time * timeSlice;
        }
    }
}

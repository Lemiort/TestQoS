using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    public class QuantizedTime : ModelTime
    {
        // TODO: возможно эту переменную стоит переименовать (или/и сдалать только для чтения)
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
        /// перевод времени
        /// TODO: дописать норм коммент
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public uint FromAnalogToDigital(double time)
        {
            // если вдруг поделится без остатка то будет косяк (10/2 = 6)
            // мб это так и надо, а может и нет
            // return (uint)(time / timeSlice) + 1;

            // добавлю ка на всякий это
            double eps = 1e-8;
            double realDiv = time / timeSlice;
            uint intDiv = (uint)(time / timeSlice);
            if (Math.Abs(realDiv - intDiv) > eps) ++intDiv;

            return intDiv;
        }

        /// <summary>
        /// перевод времени
        /// TODO: дописать норм коммент
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public double FromDigitalToAnalog(uint time)
        {
            return time * timeSlice;
        }
    }
}

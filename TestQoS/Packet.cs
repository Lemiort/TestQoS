using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// облочка для пакета
    /// </summary>
    abstract class Packet
    {
        /// <summary>
        /// Размер пакета в битах
        /// </summary>
        public int Size { get; protected set; }
    }
}

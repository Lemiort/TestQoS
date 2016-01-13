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
    public abstract class Packet
    {
        /// <summary>
        /// Размер пакета в битах
        /// </summary>
        public uint Size { get; protected set; }
    }
}

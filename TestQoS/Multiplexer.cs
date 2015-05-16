using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// мультиплексор, по сути - финальное ведро
    /// </summary>
    abstract class Multiplexer
    {
        /// <summary>
        /// ширина канала
        /// </summary>
        public UInt64 bytesPerDt { get; set; }

    }
}

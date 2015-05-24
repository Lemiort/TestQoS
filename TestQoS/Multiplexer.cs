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
    public abstract class Multiplexer
    {
        /// <summary>
        /// ширина канала в байтах на квант
        /// </summary>
        public UInt64 BytesPerDt { get; set; }

        /// <summary>
        /// длина очереди в байтах
        /// </summary>
        public UInt64 MaxQueueSize { get; set; }

    }
}

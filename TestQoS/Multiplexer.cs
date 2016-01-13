using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// Мультиплексор
    /// </summary>
    public abstract class Multiplexer
    {
        /// <summary>
        /// ширина канала в байтах на квант
        /// </summary>
        public ulong BytesPerDt { get; set; }

        /// <summary>
        /// длина очереди в байтах
        /// </summary>
        public ulong MaxQueueSize { get; set; }
    }
}

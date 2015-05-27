using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// анализатор, собирает информацию
    /// обо всех пакетах
    /// </summary>
    public abstract class Analyzer
    {
        public abstract void OnPassPacket(Packet packet);
        public abstract void OnNotPassPacket(Packet packet);
    }
}

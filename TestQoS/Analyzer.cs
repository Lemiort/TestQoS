using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    /// <summary>
    /// анализатор, собирающий инфу обо всех утечках пакетов
    /// </summary>
    public abstract class Analyzer
    {
        public abstract void OnBucketPassPacket(Packet packet);
        public abstract void OnBucketNotPassPacket(Packet packet);
        public abstract void OnMultiplexerPassPacket(Packet packet);
        public abstract void OnMultiplexerNotPassPacket(Packet packet);
    }
}

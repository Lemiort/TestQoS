﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    public abstract class TrafficGenerator
    {
        public abstract Packet MakePacket();
    }
}

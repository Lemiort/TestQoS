﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleTBQoS qos = new SimpleTBQoS();
            qos.Run();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{
    class StackedTokenBuket : SimpleTokenBuket
    {        
        private uint tokenBuketID;

        private TrafficGenerator traficGenerator;


        /// <summary>
        /// конструктор страдает болячками конструктора родителя
        /// </summary>
        /// <param name="_time"></param>
        /// <param name="ID"></param>
        /// <param name="traficGenerator"></param>
        public StackedTokenBuket(QuantizedTime _time, uint ID, TrafficGenerator traficGenerator)
            : base(_time) 
        {
            this.tokenBuketID = ID;
            this.traficGenerator = traficGenerator;
        }


    }
}

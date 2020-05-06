using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace C4RT4
{
    public class profileR
    {
        public int dos;
        public int tapis;
        public int son;
        public int[] extra = new int[4];

        public profileR()
        {
            
        }

        ~profileR()
        {
            System.GC.Collect();
        }
    }
}

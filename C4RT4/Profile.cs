using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace C4RT4
{
    public class Profile
    {
        public int dos;
        public int tapis;
        public int son;
        public int[] extra = new int[4];

        public Profile()
        {
            
        }

        ~Profile()
        {
            System.GC.Collect();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    static internal class Contains
    {
        static public bool Arrays(int[] ints1, int[] ints2)
        {
            bool equal = true; 
            for(int i = 0; i < ints1.Length; i++)
            {
                if (ints1[i] != ints2[i]) equal = false;
            }
            return equal;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerlekampMassey
{
   
        public static class GaloisField
        {
            static readonly int[] logTable = new int[256];
            static readonly int[] expTable = new int[256];

            static GaloisField()
            {
                int x = 1;
                for (int i = 0; i < 256; i++)
                {
                    expTable[i] = x;
                    logTable[x] = i;

                    x <<= 1;
                    if (x >= 256)
                    {
                        x ^= 0x11B;
                    }
                }
            }

            public static int Multiply(int a, int b)
            {
                if (a == 0 || b == 0)
                {
                    return 0;
                }
                else
                {
                    int logA = logTable[a];
                    int logB = logTable[b];
                    int logResult = (logA + logB) % 255;
                    return expTable[logResult];
                }
            }

            public static int Exp(int power)
            {
                power = power % 255;
                if (power < 0)
                {
                    power += 255;
                }
                return expTable[power];
            }

            public static int Log(int value)
            {
                if (value == 0)
                {
                    throw new ArgumentException("Cannot compute logarithm of zero.");
                }

                return logTable[value];
            }
        }
    }


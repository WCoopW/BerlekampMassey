using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerlekampMassey
{

    public class GaloisField
    {
        Random rnd = new Random();
        static readonly int[] logTable = new int[256];
        static readonly int[] expTable = new int[256];
        public byte[] Encoded(byte[] plain)
        {
            var b = new byte[21];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = (byte)rnd.Next(0,2);
            }
            for(int i = 15; i < 21; i++ )
            {
                b[i] = plain[i - 15];
            }
            return b;
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
        public static bool[] BerlekampMasseyDecoder(byte[] receivedCode)
        {
            int n = receivedCode.Length;
            int k = 6;
            int t = 3;

            int[] sigma = new int[n + 1];
            int[] c = new int[n + 1];
            int[] b = new int[n + 1];

            sigma[0] = 1;
            b[0] = 1;

            int L = 0;
            int m = -1;

            for (int i = 0; i < n; i++)
            {
                int d = receivedCode[i];

                for (int j = 1; j <= L; j++)
                {
                    d ^= c[j] & receivedCode[i - j];
                }

                if (d == 1)
                {
                    var t_cpy = (int[])c.Clone();
                    var b_cpy = (int[])b.Clone();
                    for (int j = 0; j <= k; j++)
                    {
                        c[j + m + 1] ^= b_cpy[j];
                    }

                    if (L <= i / 2)
                    {
                        L = i + 1 - L;
                        m = i;
                        Array.Copy(t_cpy, b, t_cpy.Length);
                    }
                }
            }

            return DecodeSyndrome(receivedCode, sigma, t);
        }

        public static bool[] DecodeSyndrome(byte[] receivedCode, int[] sigma, int t)
        {
            bool[] decodedMessage = new bool[receivedCode.Length];

            int n = receivedCode.Length;
            int k = 6;

            int[] S = new int[t + 1];
            int[] errorLocations = new int[t + 1];

            for (int i = 0; i < n; i++)
            {
                int d = receivedCode[i];
                for (int j = 1; j <= t; j++)
                {
                    d ^= sigma[j] & receivedCode[i - j + k];
                }

                for (int j = t; j >= 1; j--)
                {
                    S[j] = S[j - 1] ^ (d & sigma[j]);
                }
                S[0] = d;
                int l = 0;
                if (S[t] == 1)
                {
                    for (int j = 0; j < t; j++)
                    {
                        if (errorLocations[j] != -1)
                        {
                            l ^= (errorLocations[j] + j ) % n;
                        }
                    }

                    var delta = l;

                    for (int j = t; j >= 0; j--)
                    {
                        if (sigma[j] != 0 && errorLocations[t - j] != -1)
                        {
                            decodedMessage[errorLocations[t - j]] ^= true;
                        }
                    }

                    decodedMessage[i - k + delta] ^= true;
                }

                for (int j = t; j > 0; j--)
                {
                    errorLocations[j] = errorLocations[j - 1];
                }

                if (S[t] == 1)
                {
                    errorLocations[0] = i - k;
                }
                else
                {
                    errorLocations[0] = -1;
                }
            }
            return decodedMessage;

        }
    }
}
        


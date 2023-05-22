namespace BerlekampMassey
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte[] plainreg = new byte[] { 1, 1, 0, 1, 0, 1 };
            var b = new byte[] { 1,0,1,0,1,1,1,1,1,0,1,0,1,1,1,1,0,0,1,0,1 };
            var k = new GaloisField();
            k.Encoded(plainreg);
            // 101010
            var bb = BerlekampMassey(b);
            for (int i = 0; i < 6; i++)
            {
                Console.Write(bb[i]);
            }
            Console.ReadLine();
            // 1,3,5
        }
        public static byte[] BerlekampMassey(byte[] s)
        {
            int L, N, m, d;
            int n = s.Length;
            byte[] c = new byte[n];
            byte[] b = new byte[n];
            byte[] t = new byte[n];

            //Initialization
            b[0] = c[0] = 1;
            N = L = 0;
            m = -1;

            //Algorithm core
            while (N < n)
            {
                d = s[N];
                for (int i = 1; i <= L; i++)
                    d ^= c[i] & s[N - i];            //(d+=c[i]*s[N-i] mod 2)
                if (d == 1)
                {
                    Array.Copy(c, t, n);    //T(D)<-C(D)
                    for (int i = 0; (i + N - m) < n; i++)
                        c[i + N - m] ^= b[i];
                    if (L <= (N >> 1))
                    {
                        L = N + 1 - L;
                        m = N;
                        Array.Copy(t, b, n);    //B(D)<-T(D)
                    }
                }
                N++;
            }
            return b;
        }
    }
}
       
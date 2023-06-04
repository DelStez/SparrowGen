using System;
using System.Numerics;
using System.Security.Cryptography;

namespace SparrowGen
{
    public class MickyShnorrRandom
    {
        private static BigInteger p = BigInteger.Parse("1526724213212...");
        private static BigInteger q = BigInteger.Parse("5855991611207...");
        private static BigInteger g = BigInteger.Parse("4139223058474...");
        private static BigInteger x = 0;
        private static BigInteger y = 0;


        private static SHA256 sha256 = SHA256.Create();

        static MickyShnorrRandom()
        {
            GenerateKeys();
        }

        public static BigInteger Next()
        {
            BigInteger r = GenerateR();
            BigInteger res = BigInteger.ModPow(g, r, p);
            byte[] hash = sha256.ComputeHash(res.ToByteArray());
            BigInteger h = new BigInteger(hash);
            BigInteger s = (r - x * h) % q;

            if (s < 0)
                s += q;

            return s;
        }

        private static void GenerateKeys()
        {
            Random random = new Random();

            do
            {
                x = new BigInteger(q.ToByteArray());
                y = BigInteger.ModPow(g, x, p);
            } while (y == 1 || y == -1 || y == 0);
        }
        
        private static BigInteger GenerateR()
        {
            byte[] buf = new byte[q.ToByteArray().LongLength + 1];

            while (true)
            {
                RandomNumberGenerator.Create().GetBytes(buf);
                BigInteger r = new BigInteger(buf);

                if (r <= 0 || r >= q)
                    continue;

                return r;
            }
        }
    }
}
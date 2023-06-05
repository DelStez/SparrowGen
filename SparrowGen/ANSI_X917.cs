using System;
using System.IO;
using System.Security.Cryptography;

namespace SparrowGen
{
    public static class ANSI_X917
    {
        private static TripleDES des = TripleDES.Create();
        private static byte[] key;
        private static byte[] iv;

        static ANSI_X917()
        {
            key = CreateKey(24);
            iv = CreateIV(des.BlockSize / 8);
        }

        public static byte[] Generate(int length)
        {
            byte[] data = new byte[length];

            for (int i = 0; i < length; i += des.BlockSize / 8)
            {
                byte[] block = EncryptBlock(iv);
                Array.Copy(block, 0, data, i, Math.Min(block.Length, length - i));
                iv = block;
            }

            return data;
        }

        private static byte[] EncryptBlock(byte[] input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.Close();
                return ms.ToArray();
            }
        }

        private static byte[] CreateKey(int length)
        {
            byte[] key = new byte[length];
            RandomNumberGenerator.Create().GetBytes(key);
            return key;
        }

        private static byte[] CreateIV(int length)
        {
            byte[] iv = new byte[length];
            RandomNumberGenerator.Create().GetBytes(iv);
            return iv;
        }
    }
}
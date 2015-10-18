/*  
 *  Copyright © 2012 Matthew David Elgert - mdelgert@yahoo.com
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation; either version 2.1 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA 
 * 
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  
 */

using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{

    public static class Crypto
    {

        private static int DEFAULT_PASSPHRASE_LENGTH = 16; //Must be 16
        private static int DEFAULT_PASSPHRASESALT_LENGTH = 32;
        private static String DEFAULT_ALLOWED_CHARS_STRONG = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ23456789~!@#$%^&*()_+-={}|[]:;<>?.,`"; 
        private static String DEFAULT_ALLOWED_CHARS_SIMPLE = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ23456789";

        public static string Encrypt(string plainText, string passwordsalt)
        {
            if (String.IsNullOrEmpty(plainText)) throw new ArgumentException("Invalid value submitted for encryption");
            if (String.IsNullOrEmpty(passwordsalt)) throw new ArgumentException("Cannot obtain passwordsalt");
            string passPhrase = GenerateRandomPassphrase();
            string preKey = String.Concat(passwordsalt, passPhrase);
            byte[] bytePreKey = Encoding.ASCII.GetBytes(preKey);
            SHA256 sha = SHA256.Create();
            byte[] fullKey = sha.ComputeHash(bytePreKey);
            sha.Clear();
            byte[] iv = fullKey.Take(16).ToArray<byte>();
            byte[] key = fullKey.Skip(16).ToArray<byte>();
            byte[] plain = Encoding.ASCII.GetBytes(plainText);
            RijndaelManaged algorithm = new RijndaelManaged();
            algorithm.Mode = CipherMode.ECB;
            algorithm.Padding = PaddingMode.PKCS7;
            ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv);
            byte[] cipherText = encryptor.TransformFinalBlock(plain, 0, plain.Length);
            byte[] salt = Encoding.ASCII.GetBytes(passPhrase);
            byte[] final = new byte[salt.Length + cipherText.Length];
            Buffer.BlockCopy(salt, 0, final, 0, salt.Length);
            Buffer.BlockCopy(cipherText, 0, final, salt.Length, cipherText.Length);
            return Convert.ToBase64String(final);
        }

        public static string Decrypt(string encryptedValue, string passwordsalt)
        {
            if (string.IsNullOrEmpty(encryptedValue)) throw new ArgumentException("Invalid value submitted for decryption");
            byte[] all = Convert.FromBase64String(encryptedValue);
            byte[] salt = all.Take(16).ToArray<byte>();
            byte[] encryptedData = all.Skip(16).ToArray<byte>();
            string a = Encoding.UTF8.GetString(salt);
            string b = String.Concat(passwordsalt, a);
            byte[] c = Encoding.UTF8.GetBytes(b);
            SHA256 sha = SHA256.Create();
            byte[] fullKey = sha.ComputeHash(c);
            byte[] iv = fullKey.Take(16).ToArray<byte>();
            byte[] key = fullKey.Skip(16).ToArray<byte>();
            RijndaelManaged algorithm = new RijndaelManaged();
            algorithm.Mode = CipherMode.ECB;
            algorithm.Padding = PaddingMode.PKCS7;
            ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv);
            byte[] plainText = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            string decrypted = Encoding.UTF8.GetString(plainText);
            return decrypted;
        }

        public static string GeneratePasswordSalt()
        {
            Byte[] randomBytes = new Byte[DEFAULT_PASSPHRASESALT_LENGTH];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            char[] chars = new char[DEFAULT_PASSPHRASESALT_LENGTH];
            int allowedCharCount = DEFAULT_ALLOWED_CHARS_STRONG.Length;
            for (int i = 0; i < DEFAULT_PASSPHRASESALT_LENGTH; i++)
            {
                chars[i] = DEFAULT_ALLOWED_CHARS_STRONG[(int)randomBytes[i] % allowedCharCount];
            }
            return new string(chars);
        }

        public static string GenerateRandomPassphrase()
        {
            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            byte[] random = new byte[DEFAULT_PASSPHRASE_LENGTH];
            rngCsp.GetBytes(random);
            return Encoding.ASCII.GetString(random);
        }

        public static string GenerateSimplePassword()
        {
            Byte[] randomBytes = new Byte[8];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            char[] chars = new char[8];
            int allowedCharCount = DEFAULT_ALLOWED_CHARS_SIMPLE.Length;
            for (int i = 0; i < 8; i++)
            {
                chars[i] = DEFAULT_ALLOWED_CHARS_SIMPLE[(int)randomBytes[i] % allowedCharCount];
            }
            return new string(chars);
        }


    }

}

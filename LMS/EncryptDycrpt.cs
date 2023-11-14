using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LMS
{
    public class EncryptDecrypt
    {
        // Encrypt the text
        public static string EncryptText(string strText)
        {
            return Encrypt(strText, "&D%#@?,:*");
        }
        //Decrypt the text 
        public static string DecryptText(string strText)
        {
            return Decrypt(strText, "&D%#@?,:*");
        }

        //The function used to encrypt the text
        private static string Encrypt(string strText, string strEncrKey)
        {
            byte[] byKey = { };
            byte[] IV = {
        0x12,
        0x34,
        0x56,
        0x78,
        0x90,
        0xab,
        0xcd,
        0xef
    };


            //byKey = System.Text.Encoding.UTF8.GetBytes(Strings.Left(strEncrKey, 8));

            byKey = System.Text.Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        //The function used to decrypt the text
        private static string Decrypt(string strText, string sDecrKey)
        {
            byte[] byKey = {

    };
            byte[] IV = {
        0x12,
        0x34,
        0x56,
        0x78,
        0x90,
        0xab,
        0xcd,
        0xef
    };
            byte[] inputByteArray = new byte[strText.Length + 1];
            //byte[] inputByteArray = new byte[strText.Length - IV.Length];


            //byKey = System.Text.Encoding.UTF8.GetBytes(Strings.Left(sDecrKey, 8));
            byKey = System.Text.Encoding.UTF8.GetBytes(sDecrKey.Substring(0, 8));
            //byKey = System.Text.Encoding.UTF8.GetBytes(sDecrKey.Left(16));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(strText);

            //string result;
            //using (var msDecrypt = new MemoryStream(inputByteArray))
            //{
            //    using (var csDecrypt = new CryptoStream(msDecrypt, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Read))
            //    {
            //        using (var srDecrypt = new StreamReader(csDecrypt))
            //        {
            //            result = srDecrypt.ReadToEnd();
            //        }
            //    }
            //}



            //return result;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);

            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;

            return encoding.GetString(ms.ToArray());

        }




    }
}
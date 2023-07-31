using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LoginFinal.HelpingClasses
{
    public class StringCipher
    {

        #region Id Encryption

        public static string EncryptId(int id)
        {
            string stringToEncrypt = id.ToString();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
            byte[] rgbIV = { 0x21, 0x43, 0x56, 0x87, 0x10, 0xfd, 0xea, 0x1c };
            byte[] key = { };
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes("A0D1nX0Q");
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, rgbIV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static int DecryptId(string EncId)
        {
            if (EncId.Contains(' '))
            {
                EncId = EncId.Replace(' ', '+');
            }
            int id = -1;
            id = Decryptid(HttpUtility.UrlDecode(EncId));
            string str = "";
            if (id == 0)
            {
                str = HttpUtility.UrlEncode(EncId);
                id = Decryptid(HttpUtility.UrlDecode(str));
            }
            return id;
        }

        public static int Decryptid(string EncId)
        {
            byte[] inputByteArray = new byte[EncId.Length + 1];
            byte[] rgbIV = { 0x21, 0x43, 0x56, 0x87, 0x10, 0xfd, 0xea, 0x1c };
            byte[] key = { };
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes("A0D1nX0Q");
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(EncId);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, rgbIV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return Convert.ToInt32(encoding.GetString(ms.ToArray()));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion


        public static string Encrypt(string InputText, string KeyString = "Nodlays")
        {
            MemoryStream memoryStream = null;
            CryptoStream cryptoStream = null;
            try
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 128;
                    AES.BlockSize = 128;
                    byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);
                    PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(KeyString, Encoding.ASCII.GetBytes(KeyString.Length.ToString()));
                    using (ICryptoTransform Encryptor = AES.CreateEncryptor(SecretKey.GetBytes(16), SecretKey.GetBytes(16)))
                    {
                        using (memoryStream = new MemoryStream())
                        {
                            using (cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(PlainText, 0, PlainText.Length);
                                cryptoStream.FlushFinalBlock();
                                return Convert.ToBase64String(memoryStream.ToArray());
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (memoryStream != null)
                    memoryStream.Close();
                if (cryptoStream != null)
                    cryptoStream.Close();
            }
        }


        public static string Decrypt(string InputText, string KeyString = "Nodlays")
        {
            MemoryStream memoryStream = null;
            CryptoStream cryptoStream = null;
            try
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 128;
                    AES.BlockSize = 128;
                    byte[] EncryptedData = Convert.FromBase64String(InputText);
                    PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(KeyString, Encoding.ASCII.GetBytes(KeyString.Length.ToString()));
                    using (ICryptoTransform Decryptor = AES.CreateDecryptor(SecretKey.GetBytes(16), SecretKey.GetBytes(16)))
                    {
                        using (memoryStream = new MemoryStream(EncryptedData))
                        {
                            using (cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read))
                            {
                                byte[] PlainText = new byte[EncryptedData.Length];
                                return Encoding.Unicode.GetString(PlainText, 0, cryptoStream.Read(PlainText, 0, PlainText.Length));
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (memoryStream != null)
                    memoryStream.Close();
                if (cryptoStream != null)
                    cryptoStream.Close();
            }
        }


        public static string Base64Encode(string plainText)
        {
            plainText = plainText.Replace(" ", "+");
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}

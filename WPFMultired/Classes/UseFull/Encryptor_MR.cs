using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WPFMultired.Classes.UseFull
{
    public class Encrytor_MR
    {
        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static string timeStamp;

        public Encrytor_MR()
        {
            timeStamp = CurrentTimeMillis().ToString();
        }
        //Separo todos los mensajes enviados por Multired
        public string DataSplit(string data)
        {
            return data.Split('|')[0];
        }

        //Devuelvo timestamp como lo solicita Multired
        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }


        //Devuelvo el parámetro ya cifrado
        public string GetFullParameter(string data, string keyToEncrypt)
        {
            return Encrypt(GetDataToEncrypt(data), keyToEncrypt);
        }

        //Creo la cadena estandar indicada por multired para el envío de cada parámetro
        private string GetDataToEncrypt(string text)
        {
            return string.Concat(text, "|", timeStamp);
        }

        /***
         * 
         * Encripto el texto utilizando una llave AES de 128bits y 
         * un cifrado de bloqueo de cadena, y retorno un string codificado en base 64
         */
        public String Encrypt(String plainText, String key)
        {
            try
            {

                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                var encryptBity = GetRijndaelManaged(key).CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                return Convert.ToBase64String(encryptBity);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public String Decrypt(String encryptedText, String key)
        {
            try
            {
                var encryptedBytes = Convert.FromBase64String(encryptedText);
                var decryptByte = GetRijndaelManaged(key).CreateDecryptor().TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                string response = Encoding.UTF8.GetString(decryptByte);
                int end;
                int start;
                end = response.Length;
                start = response.IndexOf("|");
                response = response.Remove(start, end - start);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Creo el objeto RM con la configuracion necesario para ser compatible con Multired
        public RijndaelManaged GetRijndaelManaged(String secretKey)
        {
            try
            {
                var keyBytes = new byte[16];
                var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
                Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
                return new RijndaelManaged
                {
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7,
                    KeySize = 128,
                    BlockSize = 128,
                    Key = keyBytes,
                    IV = keyBytes
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

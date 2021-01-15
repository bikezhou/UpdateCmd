using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace UpdateCmd.Helpers
{
    public class FileHelper
    {
        /// <summary>
        /// 获取文件MD5值
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetMD5Hash(string filename)
        {
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var md5 = new MD5CryptoServiceProvider();
                var bytes = md5.ComputeHash(fs);
                var sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}

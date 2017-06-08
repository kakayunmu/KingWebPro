using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace King.Utility.Security
{
    public class Encryption
    {
        /// <summary>
        /// 带盐的字符串MD5
        /// </summary>
        /// <param name="salt"></param>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static string Md5WithSalt(string salt, string inputStr)
        {
            if (string.IsNullOrEmpty(salt))
            {
                throw new Exception("Md5WithSalt 方法参数 salt 不允许为空！");
            }
            salt = Md5(salt);
            return Md5(Md5(salt) + Md5(inputStr));
        }

        /// <summary>
        /// 字符串MD5加密
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static string Md5(string inputStr)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(inputStr));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }

        /// <summary>
        /// 产生一个指定长度的随机字符串
        /// </summary>
        /// <param name="figure">长度</param>
        /// <param name="rt">类型 默认数字字母混合</param>
        /// <returns></returns>
        public static string GetRandomStr(int figure, RandomType rt = RandomType.blend)
        {
            char[] character = null;
            char[] number = { '0', '1', '2', '3', '4', '5', '6', '8', '9' };
            char[] letter = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            if (rt == RandomType.number)
            {
                character = number;
            }
            else if (rt == RandomType.letter)
            {
                character = letter;
            }
            else
            {
                character = new char[36];
                number.CopyTo(character, 0);
                letter.CopyTo(character, number.Length);
            }

            Random rnd = new Random();
            string tarStr = string.Empty;
            for (int i = 0; i < figure; i++)
            {
                tarStr += character[rnd.Next(character.Length)];
            }
            return tarStr;
        }

        public enum RandomType
        {
            /// <summary>
            /// 字母数字混合
            /// </summary>
            blend = 0,
            /// <summary>
            /// 纯数字
            /// </summary>
            number = 1,
            /// <summary>
            /// 纯字母
            /// </summary>
            letter = 2
        }

    }
}

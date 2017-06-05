using System;
using Newtonsoft.Json;

namespace King.Utility.Convert
{
    public class ByteConVertHelper
    {
        /// <summary>
        /// 将对象转换为byte数组
        /// </summary>
        /// <param name="obj">被转换的对象</param>
        /// <returns>转换后的Byte数组</returns>
        public static byte[] Object2Bytes(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            byte[] serializedResult = System.Text.Encoding.UTF8.GetBytes(json);
            return serializedResult;
        }

        /// <summary>
        /// 将Byte数组转换成对象
        /// </summary>
        /// <param name="buff">被转换的Byte数组</param>
        /// <returns>转换后的对象</returns>
        public static object Bytes2Object(byte[] buff)
        {
            string json = System.Text.Encoding.UTF8.GetString(buff);
            return JsonConvert.DeserializeObject<object>(json);
        }
        /// <summary>
        /// 将Byte数值转换成对象
        /// </summary>
        /// <typeparam name="T">需要转换成的类型</typeparam>
        /// <param name="buff">被转换Byte数组</param>
        /// <returns>转换完成后的对象</returns>
        public static T Bytes2Object<T>(byte[] buff)
        {
            string json = System.Text.Encoding.UTF8.GetString(buff);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}

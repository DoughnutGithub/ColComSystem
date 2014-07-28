using CL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CL.Common
{
    public class Util
    {
        #region MyRegion


        /// <summary>
        /// 添加上传错误提示
        /// </summary>
        /// <param name="ue"></param>
        /// <param name="title"></param>
        /// <param name="error"></param>
        public static void AddUploadError(ref UploadError ue, string title, string error)
        {
            if (ue == null) ue = new UploadError();
            ue.Items.Add(new UploadError { Title = title, Content = error });
        }

        public static String GetUploadError(UploadError ue)
        {
            StringBuilder sb = new StringBuilder();
            if (ue != null)
            {
                foreach (UploadError error in ue.Items)
                {
                    if (sb.Length > 0) sb.Append("<br />");
                    sb.Append(error.Title + ":" + error.Content);
                }
            }
            return sb.ToString();
        }

        #endregion

        #region xml序列化
        /// <summary>
        /// xml反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strXml"></param>
        /// <returns></returns>
        public static object xmlDeserialize<T>(string strXml)
        {
            try
            {
                if (strXml == null || strXml == "") return default(T);
                string xml = string.Empty;
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(typeof(T));

                using (TextReader tr = new StringReader(strXml))
                {
                    T t = (T)xmls.Deserialize(tr);
                    tr.Close();
                    return t;
                }

            }
            catch { return default(T); }
        }
        /// <summary>
        /// xml序列化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string xmlSerialize(Object o)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(o.GetType());
                TextWriter w = new StringWriter();
                xmls.Serialize(w, o);
                w.Flush();
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = false;
                doc.LoadXml(w.ToString());
                return doc.OuterXml;

            }
            catch { return null; }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="pObj"></param>
        /// <returns></returns>
        public static byte[] SerializeObject(object pObj)
        {
            if (pObj == null) return null;
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, pObj);
                stream.Position = 0;
                byte[] read = new byte[stream.Length];
                stream.Read(read, 0, read.Length);
                stream.Close();
                return read;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static object DeserializeObject(Byte[] bytes)
        {
            if (bytes == null) return null;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return binaryFormatter.Deserialize(ms);
            }

        }

        #endregion

        #region MyRegion
        public static String GetModelMessage<T>(T m)
        {
            PropertyInfo[] pi = StaticCache<Type, PropertyInfo[]>.Get(typeof(T), p => p.GetProperties());
            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo p in pi)
            {
                if (sb.Length > 0)
                    sb.Append("\r\n");
                sb.AppendFormat("{0}:{1}", p.Name, MemberAccessor<T>.Instance.GetValue(m, p.Name));
            }
            return sb.ToString();
        }

        public static String GetModelMessage1(Object m)
        {
            if (m == null) return null;
            Type t = m.GetType();
            PropertyInfo[] pi = StaticCache<Type, PropertyInfo[]>.Get(t, p => p.GetProperties());
            StringBuilder sb = new StringBuilder();

            foreach (PropertyInfo p in pi)
            {
                if (sb.Length > 0)
                    sb.Append("\r\n");
                sb.AppendFormat("{0}:{1}", p.Name, p.GetValue(m));
            }
            return sb.ToString();
        }

        #endregion
    }
}

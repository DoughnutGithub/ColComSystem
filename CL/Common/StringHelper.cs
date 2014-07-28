using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Common
{
    /// <summary>
    /// 字段符操作
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// 返回数组字符串删除前后空格
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="SplitText"></param>
        /// <returns></returns>
        public static string[] of_SplitString(string Text, string SplitText)
        {
            string[] array = of_SplitString(true, Text, SplitText);
            for (int i = 0; i < array.Length; i++)
                array[i] = array[i].Trim();
            return array;
        }
        public static string[] of_SplitString(bool removeempty, string Text, string SplitText)
        {
            if (string.IsNullOrEmpty(Text)) return new string[0];
            string[] sp = new string[1];
            sp[0] = SplitText;
            if (removeempty)
                return Text.Split(sp, StringSplitOptions.RemoveEmptyEntries);
            else return Text.Split(sp, StringSplitOptions.None);
        }

        public static string[] of_SplitString(string Text, string SplitText, bool removeRepeat)
        {
            string[] ret = of_SplitString(Text, SplitText);
            if (!removeRepeat) return ret;
            ArrayList list = new ArrayList();
            foreach (string s in ret)
            {
                if (!list.Contains(s))
                {
                    list.Add(s);
                }
            }
            return (String[])list.ToArray(typeof(String));
        }

        public static string[] of_UpString(string[] array, int index)
        {
            if (index == 0) return array;
            string up = array[index]; string up1 = array[index - 1];
            array[index - 1] = up; array[index] = up1;
            return array;
        }
        public static string of_GetString(string[] array, string split)
        {
            if (array == null) return "";
            StringBuilder sb = new StringBuilder();
            foreach (string s in array)
            {
                if (sb.Length > 0) sb.Append(split);
                sb.Append(s);
            }
            return sb.ToString();
        }

        public static bool CheckStringInArray(string checkstring, string[] array)
        {
            foreach (string s in array)
            {
                if (s == checkstring)
                    return true;
            }
            return false;
        }
        public static bool CheckStringContainArrayText(string checkstring, string arrayText, string SplitText)
        {
            string[] array = of_SplitString(arrayText, SplitText);
            foreach (string s in array)
            {
                if (s.Contains(checkstring))
                    return true;
            }
            return false;
        }
    }
}

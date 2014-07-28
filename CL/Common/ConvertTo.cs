using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CL.Common
{
    /// <summary>
    /// 转换
    /// </summary>
    public class ConvertTo
    {
        /// <summary>
        /// 将数字月份转换成英文月份
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ConvertIntToEngMonth(object obj)
        {
            string result = string.Empty;
            int month = 0;
            if (obj.GetType().ToString() == "System.String")
                month = ConvertTo.ConvertToInt32(obj);
            else
                month = (int)obj;
            switch (month)
            {
                case 1:
                    result = "JAN.";
                    break;
                case 2:
                    result = "FEB.";
                    break;
                case 3:
                    result = "MAR.";
                    break;
                case 4:
                    result = "APR.";
                    break;
                case 5:
                    result = "MAY.";
                    break;
                case 6:
                    result = "JUN.";
                    break;
                case 7:
                    result = "JUL.";
                    break;
                case 8:
                    result = "AUG.";
                    break;
                case 9:
                    result = "SEPT.";
                    break;
                case 10:
                    result = "OCT.";
                    break;
                case 11:
                    result = "NOV.";
                    break;
                case 12:
                    result = "DEC.";
                    break;
            }
            return result;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Scrub(object text)
        {
            if (text == DBNull.Value || text == null) return "";
            return text.ToString().Trim();
        }
        public static DateTime ScrubDate(string text)
        {
            text = Scrub(text);
            return Convert.ToDateTime(text);

        }

        /// <summary>
        /// 转换String
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ConvertToString(object o)
        {
            o = Scrub(o);
            return o == null ? "" : o.ToString();
        }

        /// <summary>
        /// 转换String
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ConvertToStringNull(object o)
        {
            o = Scrub(o);
            return o == null ? null : o.ToString();
        }



        public static object ConvertToSqlNull(object obj)
        {
            if (obj == null) return DBNull.Value;
            else return obj;
        }

        /// <summary>
        /// 转换Int32
        /// 出错返回0
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static int ConvertToInt32(object o)
        {
            string str = Scrub(o);
            str = of_ProcessNumberFormat(str);
            if (CheckNumber(str))
            {
                //处理小数
                if (str.Contains("."))
                {
                    str = Math.Round(ConvertTo.ConvertToDouble(str), 0).ToString();
                }
                return Convert.ToInt32(str);
            }
            else
                return 0;
        }

        public static int? ConvertToInt32Null(object o)
        {
            string str = Scrub(o);
            str = of_ProcessNumberFormat(str);
            if (CheckNumber(str))
                return Convert.ToInt32(str);
            else
                return null;
        }
        public static int ConvertToInt(object o)
        {
            string str = Scrub(o);
            str = of_ProcessNumberFormat(str);
            if (CheckNumber(str))
                return Convert.ToInt32(str);
            else
                return 0;
        }
        /// <summary>
        /// 转换Double
        /// 出错返回0
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static Double ConvertToDouble(object o)
        {
            string str = Scrub(o);
            str = of_ProcessNumberFormat(str);
            if (CheckNumber(str))
                return Convert.ToDouble(str);
            else
                return 0;
        }

        public static Double? ConvertToDoubleNull(object o)
        {
            string str = Scrub(o);
            str = of_ProcessNumberFormat(str);
            if (CheckNumber(str))
                return Convert.ToDouble(str);
            else
                return null;
        }

        private static String of_ProcessNumberFormat(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                text = text.Replace(",", "");
                if (text.EndsWith("%"))
                {
                    text = text.Replace("%", "");
                    if (CheckNumber(text))
                        return (Convert.ToDouble(text) / 100).ToString();
                }
                return text;
            }
            else return text;
        }

        public static bool ConvertToBoolean(object o)
        {
            string str = Scrub(o);
            if (str.ToLower() == "true" || str == "真" || str == "1") return true;
            else return false;

        }
        public static bool? ConvertToBooleanNull(object o)
        {
            if (o == null) return null;
            else return ConvertToBoolean(o);

        }

        /// <summary>
        /// 转换Decimal
        /// 出错返回0
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static decimal ConvertToDecimal(object o)
        {
            string str = Scrub(o);
            str = of_ProcessNumberFormat(str);
            if (CheckNumber(str))
                return Convert.ToDecimal(str);
            else
                return 0;
        }

        public static decimal? ConvertToDecimalNull(object o)
        {
            string str = Scrub(o);
            str = of_ProcessNumberFormat(str);
            if (CheckNumber(str))
                return Convert.ToDecimal(str);
            else
                return null;
        }




        public static bool CheckNumber(string strNumber)
        {
            strNumber = of_ProcessNumberFormat(strNumber);
            if (string.IsNullOrEmpty(strNumber) || strNumber == "+" || strNumber == "-") return false;
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumber) &&
            !objTwoDotPattern.IsMatch(strNumber) &&
            !objTwoMinusPattern.IsMatch(strNumber) &&
            objNumberPattern.IsMatch(strNumber);


        }
        /// <summary>
        /// 日期转换
        /// 出错则返回系统默认日期
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static DateTime? ConvertToDate(object o)
        {
            string text = Scrub(o);
            if (CheckDate(text))
                return Convert.ToDateTime(text);
            else return null;


        }



        /// <summary>
        /// 日期验证
        /// yyyy-MM-dd
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool CheckDate(string strDate)
        {
            strDate = strDate.Replace("/", "-");
            Regex objDatePattern = new Regex(@"(((^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(10|12|0?[13578])([-\/\._])(3[01]|[12][0-9]|0?[1-9]))|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(11|0?[469])([-\/\._])(30|[12][0-9]|0?[1-9]))|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(0?2)([-\/\._])(2[0-8]|1[0-9]|0?[1-9]))|(^([2468][048]00)([-\/\._])(0?2)([-\/\._])(29))|(^([3579][26]00)([-\/\._])(0?2)([-\/\._])(29))|(^([1][89][0][48])([-\/\._])(0?2)([-\/\._])(29))|(^([2-9][0-9][0][48])([-\/\._])(0?2)([-\/\._])(29))|(^([1][89][2468][048])([-\/\._])(0?2)([-\/\._])(29))|(^([2-9][0-9][2468][048])([-\/\._])(0?2)([-\/\._])(29))|(^([1][89][13579][26])([-\/\._])(0?2)([-\/\._])(29))|(^([2-9][0-9][13579][26])([-\/\._])(0?2)([-\/\._])(29)))((\s+(0?[1-9]|1[012])(:[0-5]\d){0,2}(\s[AP]M))?$|(\s+(\d|([01]\d|2[0-3]))(:[0-5]\d){0,2})?$))");

            //Regex objDatePattern1 = new Regex(@"(((^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(10|12|0?[13578])([-\/\._])(3[01]|[12][0-9]|0?[1-9]))|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(11|0?[469])([-\/\._])(30|[12][0-9]|0?[1-9]))|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(0?2)([-\/\._])(2[0-8]|1[0-9]|0?[1-9]))|(^([2468][048]00)([-\/\._])(0?2)([-\/\._])(29))|(^([3579][26]00)([-\/\._])(0?2)([-\/\._])(29))|(^([1][89][0][48])([-\/\._])(0?2)([-\/\._])(29))|(^([2-9][0-9][0][48])([-\/\._])(0?2)([-\/\._])(29))|(^([1][89][2468][048])([-\/\._])(0?2)([-\/\._])(29))|(^([2-9][0-9][2468][048])([-\/\._])(0?2)([-\/\._])(29))|(^([1][89][13579][26])([-\/\._])(0?2)([-\/\._])(29))|(^([2-9][0-9][13579][26])([-\/\._])(0?2)([-\/\._])(29)))((\s+(0?[1-9]|1[012])(:[0-5]\d){0,2}(\s[AP]M))?$|(\s+([01]\d|2[0-3])(:[0-5]\d){0,2})?$))");
            //Regex objDatePattern2 = new Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$");
            return objDatePattern.IsMatch(strDate); //|| objDatePattern1.IsMatch(strDate) || objDatePattern2.IsMatch(strDate) ;


        }
    }
}

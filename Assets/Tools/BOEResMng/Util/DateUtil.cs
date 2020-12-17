using UnityEngine;
using System;
using System.Collections;
namespace BOE.BOEComponent.Util
{
    public class DateUtil
    {

        /************************************************************************/
        /* 格式化时间：hh：mm：ss                                               */
        /************************************************************************/

        public static string FormatHourAndMinAndSec(int totalSec)
        {
            TimeSpan ts = TimeSpan.FromSeconds(totalSec);
            return FormatUnit(ts.Hours.ToString()) + ":" + FormatUnit(ts.Minutes.ToString()) + ":" + FormatUnit(ts.Seconds.ToString());
        }
        public static string FormatHourAndMin(int totalSec)
        {
            TimeSpan ts = TimeSpan.FromSeconds(totalSec);
            return FormatUnit(ts.Hours.ToString()) + ":" + FormatUnit(ts.Minutes.ToString());
        }
        public static DateTime ConvertLongToDateTime(long d)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(d + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult;
        }
        public static string FormatToDate(long tSec)
        {
            /*
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            TimeSpan toNow = new TimeSpan(tSec);
            */
             DateTime d=ConvertLongToDateTime(tSec);
            string res= d.ToString("M月d日");
            return res ;
        }
        private static string FormatUnit(string str)
        {
            if (str.Length % 2 != 0)
            {
                return "0" + str;
            }
            else
            {
                return str;
            }
        }


        public static long getTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        /************************************************************************/
        /* 获得UTC当前秒数                                                      */
        /************************************************************************/
        /// <summary>
        /// 获得UTC当前秒数
        /// </summary>
        /// <value>The now sec.</value>
        public static int NowSec
        {
            get
            {
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
                TimeSpan toNow = dtNow.Subtract(dtStart);
                string timeStamp = toNow.Ticks.ToString();
                int now = int.Parse(timeStamp.Substring(0, timeStamp.Length - 7));
                return now;
            }
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <returns>The to date time.</returns>
        /// <param name="timeStamp">Time stamp.</param>
        public  static DateTime StampToDateTime(string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dateTimeStart.Add(toNow);
        }

        /// <summary>
        /// 根据时间戳的秒数格式化时间,三种表现形式：今天，昨天，具体时间(Year-Month-Day）
        /// </summary>
        /// <returns>The date.</returns>
        /// <param name="tSec">以秒为单位的时间戳.</param>
        public static string FormatDate(int tSec)
        {
            TimeSpan ts = TimeSpan.FromSeconds((double)tSec);
            DateTime dt = StampToDateTime(tSec.ToString());
            DateTime nowDt = DateTime.Today;
            //如果大于今天0点，显示今天
            if (dt.Year == nowDt.Year && dt.Month == nowDt.Month)
            {
                if (dt.Day == nowDt.Day)
                {
                    return "今天";
                }
                else if (dt.Day == nowDt.Day - 1)
                {
                    return "昨天";
                }
            }
            return dt.Year + "-" + dt.Month + "-" + dt.Day;
        }

        public static string GetNowTimeAll()
        {
            return System.DateTime.Now.ToString("yyyy-MM-dd HH:mm" + ":00");
        }
        public static string GetNowTimeMM()
        {
            return System.DateTime.Now.ToString("HH:mm");
        }

        public static string GetNowDate()
        {
            return System.DateTime.Now.ToString("yyyy年M月d日");
        }

        public static string GetNowWeek()
        {
            DayOfWeek dw = System.DateTime.Now.DayOfWeek;
            string week;
            if(dw ==DayOfWeek.Monday)
            {
                week = "星期一";
            }
           else if (dw == DayOfWeek.Tuesday)
            {
                week = "星期二";
            }
           else  if (dw == DayOfWeek.Wednesday)
            {
                week = "星期三";
            }
          else  if (dw == DayOfWeek.Thursday)
            {
                week = "星期四";
            }
            else if (dw == DayOfWeek.Friday)
            {
                week = "星期五";
            }
            else if (dw == DayOfWeek.Saturday)
            {
                week = "星期六";
            }
            else  
            {
                week = "星期日";
            }
            return week;
        }
    }
}

using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace BOE.BOEComponent.Util
{
    public static class StringUtils
    {
        /// <summary>
        /// 得到本地时间
        /// </summary>
        /// <returns>单位秒</returns>
        public static long getMillTime()
        {
            DateTime timeStamp = new DateTime(1970, 1, 1);
            long time = (DateTime.UtcNow.Ticks - timeStamp.Ticks) / 10000000;
            return time;
        }
        /// <summary>
        /// 临时效率低
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static String FormatLongToTimeStr(long time)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int hour = 0;
            int minute = 0;
            int second = 0;
            second = Convert.ToInt32(time) / 1000;

            if (second > 60)
            {
                minute = second / 60;
                second = second % 60;
            }
            if (minute > 60)
            {
                hour = minute / 60;
                minute = minute % 60;
            }
            stringBuilder.Append(hour.ToString("D2"));
            stringBuilder.Append(":");
            stringBuilder.Append(minute.ToString("D2"));
            stringBuilder.Append(":");
            stringBuilder.Append(second.ToString("D2"));

            return stringBuilder.ToString();
        }


        /// <summary>
        /// 获取字符串的字符宽度，英文（半角）1，中文（全角）2
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int getCharWidth(string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;

            byte[] buf = System.Text.Encoding.Default.GetBytes(s);
            int cnt = 0;

            // 查找英文和数字 32 -> 127'z' ，占1个字宽
            foreach (byte b in buf)
            {
                if (b >= 32 && b <= 127)
                    cnt++;
            }

            // 其他的字符算2个字宽
            int cnt2 = s.Length - cnt;
            if (cnt2 > 0)
                cnt = cnt + cnt2 * 2;

            return cnt;
        }


        /// <summary>
        /// 裁剪字符串
        /// </summary>
        /// <param name="sourceStr"></param>
        /// <param name="strWidth">最大字宽，汉字等全角字符2，英文/数字等半角字符1</param>
        /// <returns></returns>
        public static String ClipStringN(string sourceStr,int strWidth)
        {
            if (sourceStr == null || 0==strWidth)
                return string.Empty;

            string key = sourceStr.Trim();

            if (string.IsNullOrEmpty(key))
                return string.Empty;

            // 长度没有越界，不需要检查
            if (key.Length * 2 <= strWidth)
                return key;

            // 英文1个字宽 ，中文2个字宽
            int maxWidth = strWidth;

            int width = getCharWidth(key);

            if (width > maxWidth)
            {
                // 先假设所有的字都是汉字（2个字宽），先取maxWidth/2个，不够再继续
                int count = maxWidth / 2;
                string tmp = key.Substring(0, count);
                width = getCharWidth(tmp);
                while (width < maxWidth)
                {
                    ++count;
                    string tmp2 = key.Substring(0, count);

                    // FIXME: 每次都计算完整字符串，效率低，可以只计算增加的字符的字宽
                    width = getCharWidth(tmp2);
                    if (width > maxWidth)
                        break;

                    tmp = tmp2;
                }

                key = tmp;
            }

            return key;
        }

        public static string FormatStringByMaxNum(this string str, int length, bool addThreePoint = true)
        {

            if (string.IsNullOrEmpty(str) )
            {
                return str;
            }
            var tmp= str.Trim().Replace(" ", "　").Replace("\n", "").Replace("\r", "").Replace("\t", "");
            if (str.Length < length/2)
            {
                return tmp;
            }
            int count = 0;
            var sb = new StringBuilder();
            char[] ss = tmp.ToArray();
            for (int i = 0; i < ss.Length; i++)
            {
                count += (Encoding.UTF8.GetBytes(ss[i].ToString()).Length > 1 ? 2 : 1);
                sb.Append(ss[i]);
                if (count >= length) break;
            }
            return (sb.ToString().Length < str.Length && addThreePoint) ? sb.Append("...").ToString() : sb.ToString();
        }
        /// <summary>
        /// 省略中间的方法
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="beforpiont">...之前留的字符数</param>
        /// <param name="length">要截取的总长度</param>
        /// <returns></returns>
        public  static  string FormatStringByMaxNumMidle(string str, int beforpiont, int length)
        {

            if (string.IsNullOrEmpty(str) || str.Length < length / 2)
            {
                return str;
            }
            // var tmp = str.Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
            int count = 0;
            int all = 0;
            var sb = new StringBuilder();
            char[] ss = str.ToArray();
            for (int i = 0; i < ss.Length; i++)
            {

                if (count <= beforpiont)
                {
                    count += (Encoding.UTF8.GetBytes(ss[i].ToString()).Length > 1 ? 2 : 1);
                    sb.Append(ss[i]);
                }
                all += (Encoding.UTF8.GetBytes(ss[i].ToString()).Length > 1 ? 2 : 1);
            }
            if (all <= length)
            {
                return str;
            }
            else
            {
                sb.Append("...");
                for (int i = ss.Length - 1; i > 0; i--)
                {
                    if (count < length - beforpiont)
                    {
                        count += (Encoding.UTF8.GetBytes(ss[i].ToString()).Length > 1 ? 2 : 1);
                        sb.Append(ss[i]);
                    }

                }
                return sb.ToString();
            }
        }


    }
}


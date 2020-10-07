using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ShindaLibrary
{
    public class StringTools
    {
        /// <summary>
        /// 從 ByteArray 轉成字串 (MD5 加密用)
        /// </summary>
        /// <param name="inputArray"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] inputArray)
        {
            var str = String.Concat(inputArray.Select(a => a.ToString("X2")));

            return str;
        }

        /// <summary>
        /// 隱藏名字中間的幾個字
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string MaskName(string name)
        {
            var str = "";
            if (!string.IsNullOrEmpty(name))
            {
                string maskstr, maskchar;
                maskchar = null;
                if (Regex.IsMatch(name, "[A-Za-z]"))
                {
                    if (name.IndexOf("-") > 1)
                    {
                        maskstr = name.Split('-')[1];
                        str = name.Replace(maskstr, "*");
                    }
                    else if (name.IndexOf(" ") > 1)
                    {
                        maskstr = name.Split(' ')[1];
                        str = name.Replace(maskstr, "*");
                    }
                    else
                    {
                        int End = (int)(name.Length / 2);
                        maskstr = name.Substring(1, End);
                        for (int i = 0; i < maskstr.Length; i++)
                        {
                            maskchar = maskchar + "*";
                        }
                        str = name.Replace(maskstr, maskchar);
                    }
                }
                else
                {

                    int End = (int)(name.Length / 2);
                    maskstr = name.Substring(1, End);
                    for (int i = 0; i < maskstr.Length; i++)
                    {
                        maskchar = maskchar + "*";
                    }
                    str = name.Replace(maskstr, maskchar);
                }
            }
            else
            {

                str = "";
            }

            return str;
        }
    }
}

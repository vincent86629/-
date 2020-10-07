using System;
using System.Text.RegularExpressions;

namespace ShindaLibrary
{
    public class RegexTools
    {
        /// <summary>
        /// 驗統編
        /// </summary>
        /// <param name="companyNumber">欲驗證的統編</param>
        /// <returns>是否符合統編格式</returns>
        public static bool CheckCompanyNumber(string companyNumber)
        {
            // 假設統一編號為 A B C D E F G H
            // A - G 為編號, H 為檢查碼
            // A - G 個別乘上特定倍數, 若乘出來的值為二位數則將十位數和個位數相加
            // A x 1
            // B x 2
            // C x 1
            // D x 2
            // E x 1
            // F x 2
            // G x 4
            // H x 1
            // 最後將所有數值加總, 被 10 整除就為正確
            // 若上述演算不正確並且 G 為 7 得話, 再加上 1 被 10 整除也為正確
            if (companyNumber.Trim().Length != 8)
            {
                return false;
            }

            var monitor = new Regex(@"^[0-9]*$");

            if (!monitor.IsMatch(companyNumber)) return false;

            int[] intTmpVal = new int[6];
            int intTmpSum = 0;
            for (int i = 0; i < 6; i++)
            {
                // 位置在奇數位置的*2，偶數位置*1，位置計算從0開始
                if (i % 2 == 1)
                    intTmpVal[i] = OverTen(int.Parse(companyNumber[i].ToString()) * 2);
                else
                    intTmpVal[i] = OverTen(int.Parse(companyNumber[i].ToString()));

                intTmpSum += intTmpVal[i];
            }

            // 第6碼*4
            intTmpSum += OverTen(int.Parse(companyNumber[6].ToString()) * 4);

            // 第7碼*1
            intTmpSum += OverTen(int.Parse(companyNumber[7].ToString()));

            if (intTmpSum % 10 == 0) return true;
            if (int.Parse(companyNumber[6].ToString()) == 7 && (intTmpSum + 1) % 10 == 0) return true;

            return false;
        }

        private static int OverTen(int intVal) //超過10則十位數與個位數相加，直到相加後小於10
        {
            if (intVal >= 10)
                intVal = OverTen((intVal / 10) + (intVal % 10));
            return intVal;
        }
    }

}

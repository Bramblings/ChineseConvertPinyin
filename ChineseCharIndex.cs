using System.Collections;

namespace ChineseConvertPinyin
{
    internal static class ChineseCharIndex
    {
        /// <summary>
        /// 采用二分查找算法，获取字符存在于字符集的索引
        /// </summary>
        /// <param name="charCodes"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        internal static int GetCharIndex(int[] charCodes, int code)
        {
            int index = 0;
            int beginIndex = 0;
            int endIndex = charCodes.Length - 1;
            if (beginIndex > endIndex) return -1;

            while (beginIndex <= endIndex)
            {
                index = (beginIndex + endIndex) / 2;
                if (charCodes[index] == code)
                {
                    return index;
                }

                if (charCodes[index] < code)
                {
                    beginIndex = index + 1;
                }
                else
                {
                    endIndex = index - 1;
                }
            }
            return -1;
        }

        /// <summary>
        /// 采用Context算法，获取当前字符的上下文
        /// </summary>
        /// <param name="text"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        internal static string[] GetCharContext(string text, int index, int count)
        {
            int startIndex = 0, endIndex = 0;
            ArrayList arrayList = new ArrayList();
            for (int i = 1; i <= count; i++)
            {
                startIndex = index - count + i;
                endIndex = startIndex + count - 1;
                if (startIndex >= 0 && endIndex <= (text.Length - 1))
                {
                    arrayList.Add(text.Substring(startIndex, count));
                }
            }
            return (string[])arrayList.ToArray(typeof(string));
        }
    }
}

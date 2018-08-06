using System.Reflection;

namespace ChineseConvertPinyin
{
    internal static class ChineseCharSet
    {
        #region Static Method
        /// <summary>
        /// 获取单字字符编码字符集
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        internal static int[] GetCharCodes(PinyinSingle number)
        {
            int[] charCodes = new int[] { };
            switch (number)
            {
                case PinyinSingle.Four:
                    charCodes = CharSetSingle.CharSetFour.CharCodes;
                    break;
                case PinyinSingle.Five:
                    charCodes = CharSetSingle.CharSetFive.CharCodes;
                    break;
                case PinyinSingle.Six:
                    charCodes = CharSetSingle.CharSetSix.CharCodes;
                    break;
                case PinyinSingle.Seven:
                    charCodes = CharSetSingle.CharSetSeven.CharCodes;
                    break;
                case PinyinSingle.Eight:
                    charCodes = CharSetSingle.CharSetEight.CharCodes;
                    break;
                case PinyinSingle.Nine:
                    charCodes = CharSetSingle.CharSetNine.CharCodes;
                    break;
            }
            return charCodes;
        }

        /// <summary>
        /// 获取单字字符拼音字符集
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        internal static string[] GetCharPinyin(PinyinSingle number)
        {
            string[] charPinyin = new string[] { };
            switch (number)
            {
                case PinyinSingle.Four:
                    charPinyin = CharSetSingle.CharSetFour.CharPinyin;
                    break;
                case PinyinSingle.Five:
                    charPinyin = CharSetSingle.CharSetFive.CharPinyin;
                    break;
                case PinyinSingle.Six:
                    charPinyin = CharSetSingle.CharSetSix.CharPinyin;
                    break;
                case PinyinSingle.Seven:
                    charPinyin = CharSetSingle.CharSetSeven.CharPinyin;
                    break;
                case PinyinSingle.Eight:
                    charPinyin = CharSetSingle.CharSetEight.CharPinyin;
                    break;
                case PinyinSingle.Nine:
                    charPinyin = CharSetSingle.CharSetNine.CharPinyin;
                    break;
            }
            return charPinyin;
        }

        /// <summary>
        /// 获取词组字符集
        /// </summary>
        /// <param name="number"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static string[] GetMultCharSet(PinyinMultiple number, int index)
        {
            string[] charSet = new string[] { };
            if (index >= 0)
            {
                switch (number)
                {
                    case PinyinMultiple.Two:
                        charSet = CharSetMultiple.TwoCharSet.CharCodes[index];
                        break;
                    case PinyinMultiple.Three:
                        charSet = CharSetMultiple.ThreeCharSet.CharCodes[index];
                        break;
                    case PinyinMultiple.Four:
                        charSet = CharSetMultiple.FourCharSet.CharCodes[index];
                        break;
                }
            }
            return charSet;
        }
        #endregion

        #region 通过反射获取字符集
        private const string CharSetNamespace = "ChineseConvertPinyin.CharSetSingle";
        /// <summary>
        /// 获取指定类名中的字符编码字符集
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        internal static int[] GetCharCodes(string className)
        {
            int[] charCodes = { };
            if (!string.IsNullOrEmpty(className))
            {
                FieldInfo fieldInfo = GetFieldInfo(className, "CharCodes");
                if (fieldInfo != null)
                {
                    charCodes = (int[])fieldInfo.GetValue(null);
                }
            }
            return charCodes;
        }

        /// <summary>
        /// 获取指定类名中的拼音字符集
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        internal static string[] GetCharPinyin(string className)
        {
            string[] charPinyin = { };
            if (!string.IsNullOrEmpty(className))
            {
                FieldInfo fieldInfo = GetFieldInfo(className, "CharPinyin");
                if (fieldInfo != null)
                {
                    charPinyin = (string[])fieldInfo.GetValue(null);
                }
            }
            return charPinyin;
        }

        /// <summary>
        /// 获取指定类名中指定名称的字段
        /// </summary>
        /// <param name="className"></param>
        /// <param name="charSet"></param>
        /// <returns></returns>
        private static FieldInfo GetFieldInfo(string className, string charSet)
        {
            FieldInfo fieldInfo = null;
            Assembly assembly = Assembly.GetExecutingAssembly();
            object obj = assembly.CreateInstance(CharSetNamespace + "." + className);
            if (obj != null)
            {
                fieldInfo = obj.GetType().GetField(charSet, BindingFlags.NonPublic | BindingFlags.Static);
            }
            return fieldInfo;
        }
        #endregion
    }
}

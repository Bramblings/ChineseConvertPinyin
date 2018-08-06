using System;
using System.Collections;

namespace ChineseConvertPinyin
{
    public class ChineseToPinyin
    {
        #region Public Method
        /// <summary>
        /// 获取整个字符串的拼音，当某个字符为多音字时，使用其上下文匹配多音字
        /// </summary>
        /// <param name="text">待转换的字符串</param>
        /// <returns></returns>
        public string GetFullPinyin(string text)
        {
            return GetFullPinyin(text, PinyinTone.None);
        }

        /// <summary>
        /// 获取整个字符串的拼音，当某个字符为多音字时，使用其上下文匹配多音字
        /// </summary>
        /// <param name="text">待转换的字符串</param>
        /// <param name="pinyinTone">设置以何种方式返回拼音的声调</param>
        /// <returns></returns>
        public string GetFullPinyin(string text, PinyinTone pinyinTone)
        {
            return GetFullPinyin(text, pinyinTone, "");
        }

        /// <summary>
        /// 获取整个字符串的拼音，当某个字符为多音字时，使用其上下文匹配多音字
        /// </summary>
        /// <param name="text">待转换的字符串</param>
        /// <param name="pinyinTone">设置以何种方式返回拼音的声调</param>
        /// <param name="separator">设置以何种字符串分割每个字符的拼音</param>
        /// <returns></returns>
        public string GetFullPinyin(string text, PinyinTone pinyinTone, string separator)
        {
            if (!string.IsNullOrEmpty(text))
            {
                string pinyin;
                string result = string.Empty;
                for (int i = 0; i < text.Length; i++)
                {
                    pinyin = FindMultPinyin(text, i);
                    if (pinyin.Length > 1)
                    {
                        pinyin = SetPinyinTone(pinyin, pinyinTone);
                        result += pinyin + separator;
                    }
                    else
                    {
                        result += pinyin;
                    }
                }
                return result;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取字符串中每个字符的首字母大写
        /// </summary>
        /// <param name="text">待转换的字符串</param>
        /// <returns></returns>
        public string GetInitialPinyin(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                string pinyin;
                string result = string.Empty;
                for (int i = 0; i < text.Length; i++)
                {
                    pinyin = FindMultPinyin(text, i);
                    if (pinyin.Length > 1)
                    {
                        pinyin = pinyin.Substring(0, 1).ToUpper();
                    }
                    result += pinyin;
                }
                return result;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取单个字符的拼音，单个字符为多音字时，返回第一个拼音
        /// </summary>
        /// <param name="word">待转换的字符</param>
        /// <returns></returns>
        public string GetSingleFirstPinyin(char word)
        {
            return GetSingleFirstPinyin(word, PinyinTone.None);
        }

        /// <summary>
        /// 获取单个字符的拼音，单个字符为多音字时，返回第一个拼音
        /// </summary>
        /// <param name="word">待转换的字符</param>
        /// <param name="pinyinTone">设置以何种方式返回拼音的声调</param>
        /// <returns></returns>
        public string GetSingleFirstPinyin(char word, PinyinTone pinyinTone)
        {
            string pinyin = FindPinyin(word);
            string[] pinyins = pinyin.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            pinyin = pinyins[0];
            if (pinyin.Length > 1)
            {
                pinyin = SetPinyinTone(pinyins[0], pinyinTone);
            }
            return pinyin;
        }

        /// <summary>
        /// 获取单个字符的拼音，单个字符为多音字时，返回所有的拼音
        /// </summary>
        /// <param name="word">待转换的字符</param>
        /// <returns></returns>
        public string[] GetSingleFullPinyin(char word)
        {
            return GetSingleFullPinyin(word, PinyinTone.None);
        }

        /// <summary>
        /// 获取单个字符的拼音，单个字符为多音字时，返回所有的拼音
        /// </summary>
        /// <param name="word">待转换的字符</param>
        /// <param name="pinyinTone">设置以何种方式返回拼音的声调</param>
        /// <returns></returns>
        public string[] GetSingleFullPinyin(char word, PinyinTone pinyinTone)
        {
            string pinyin = FindPinyin(word);
            string[] pinyins = pinyin.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < pinyins.Length; i++)
            {
                pinyin = pinyins[i];
                if (pinyin.Length > 1)
                {
                    pinyins[i] = SetPinyinTone(pinyins[i], pinyinTone);
                }
            }
            return pinyins;
        }

        /// <summary>
        /// 判断字符是否为中文
        /// </summary>
        /// <param name="word">判断是否为中文的字符</param>
        /// <returns></returns>
        public bool IsChinese(char word)
        {
            if ((uint)word >= 0x4E00 && (uint)word <= 0x9FA5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Private Method
        private string FindPinyin(char word)
        {
            if (IsChinese(word))
            {
                string unicode = GetCharUnicode(word);
                string firstChar = unicode.Substring(0, 1);
                PinyinSingle number = (PinyinSingle)Enum.Parse(typeof(PinyinSingle), firstChar);
                int[] charCodes = ChineseCharSet.GetCharCodes(number);
                if (charCodes.Length > 0)
                {
                    int hexCode = Convert.ToInt32(unicode, 16);
                    int index = ChineseCharIndex.GetCharIndex(charCodes, hexCode);
                    if (index >= 0)
                    {
                        string[] pinyin = ChineseCharSet.GetCharPinyin(number);
                        if (pinyin.Length > 0)
                        {
                            return pinyin[index];
                        }
                    }
                }
            }
            return word.ToString();
        }

        private string FindMultPinyin(string text, int index)
        {
            string pinyin = FindPinyin(text[index]);
            string[] pinyins = pinyin.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (pinyins.Length > 1)
            {
                return GetMultPinyin(text, index, pinyins[0]);
            }
            return pinyin;
        }

        private string GetMultPinyin(string text, int index, string pinyin)
        {
            string unicode;
            int hexCode, charIndex;
            string[] charCodes, charContext, pinyinStr;
            PinyinMultiple number;
            for (int i = 2; i <= 4; i++)
            {
                charContext = ChineseCharIndex.GetCharContext(text, index, i);
                if (charContext.Length > 0)
                {
                    number = (PinyinMultiple)Enum.Parse(typeof(PinyinMultiple), i.ToString());
                    unicode = GetCharUnicode(text[index]);
                    hexCode = Convert.ToInt32(unicode, 16);
                    charIndex = ChineseCharIndex.GetCharIndex(CharSetMultiple.OneCharSet.CharCodes, hexCode);
                    charCodes = ChineseCharSet.GetMultCharSet(number, charIndex);
                    if (charCodes.Length > 0)
                    {
                        pinyinStr = MultPinyinMatch(charContext, charCodes);
                        if (pinyinStr.Length == 1)
                        {
                            return pinyinStr[0];
                        }
                    }
                }
            }
            return pinyin;
        }

        private string[] MultPinyinMatch(string[] charContext, string[] charCodes)
        {
            string pinyin;
            string contextStr, charCode;
            ArrayList arrayList = new ArrayList();
            for (int i = 0; i < charContext.Length; i++)
            {
                contextStr = GetCharUnicode(charContext[i]);
                for (int j = 0; j < charCodes.Length; j++)
                {
                    charCode = charCodes[j];
                    if (charCode.Contains(contextStr))
                    {
                        pinyin = charCode.Substring(charCode.IndexOf(":") + 1);
                        if (!arrayList.Contains(pinyin))
                        {
                            arrayList.Add(pinyin);
                        }
                        break;
                    }
                }
            }
            return (string[])arrayList.ToArray(typeof(string));
        }

        private string SetPinyinTone(string pinyin, PinyinTone pinyinTone)
        {
            switch (pinyinTone)
            {
                case PinyinTone.None:
                    pinyin = pinyin.Remove(pinyin.Length - 1, 1);
                    break;
                case PinyinTone.Numeral:
                    break;
                case PinyinTone.Alphabetic:
                    pinyin = PinyinAlphabeticTone.GetAlpTone(pinyin);
                    break;
            }
            return pinyin;
        }

        private string GetCharUnicode(string strs)
        {
            string result = string.Empty;
            foreach (char item in strs)
            {
                result += GetCharUnicode(item);
            }
            return result;
        }

        private string GetCharUnicode(char word)
        {
            return string.Format("{0:X4}", (int)word);
        }
        #endregion
    }
}

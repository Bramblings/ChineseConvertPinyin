namespace ChineseConvertPinyin
{
    internal class PinyinAlphabeticTone
    {
        private static string[] strSpec = new string[] { "io", "ie", "iu", "iv", "uo", "ue", "ui", "uv", "vo", "ve", "vi", "vu" };
        private static string[] strChar = new string[] { "a", "e", "o", "i", "u", "v" };
        private static string[] strTone = new string[] { "āáǎàa", "ēéěèe", "ōóǒòo", "īíǐìi", "ūúǔùu", "ǖǘǚǜü" };

        internal static string GetAlpTone(string pinyin)
        {
            string vowel;
            int tone, index;
            tone = int.Parse(pinyin.Substring(pinyin.Length - 1, 1));
            pinyin = pinyin.Substring(0, pinyin.Length - 1);
            vowel = GetVowel(pinyin);
            if (!string.IsNullOrEmpty(vowel))
            {
                index = GetVowelIndex(vowel);
                pinyin = pinyin.Replace(vowel, (strTone[index][tone - 1]).ToString());
            }
            return pinyin;
        }

        private static string GetVowel(string pinyin)
        {
            string vowel = string.Empty;
            for (int i = 0; i < strSpec.Length; i++)
            {
                if (pinyin.Contains(strSpec[i]))
                {
                    vowel = strSpec[i].Substring(strSpec[i].Length - 1, 1);
                    break;
                }
            }
            if (string.IsNullOrEmpty(vowel))
            {
                for (int i = 0; i < strChar.Length; i++)
                {
                    if (pinyin.Contains(strChar[i]))
                    {
                        vowel = strChar[i];
                        break;
                    }
                }
            }
            return vowel;
        }

        private static int GetVowelIndex(string vowel)
        {
            for (int i = 0; i < strChar.Length; i++)
            {
                if (strChar[i] == vowel)
                {
                    return i;
                }
            }
            return 0;
        }
    }
}

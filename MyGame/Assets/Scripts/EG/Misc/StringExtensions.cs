using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace EG.Misc {
/// <summary>
/// Класс-расширение для добавления дополнительных методов классу String
/// </summary>
public static class StringExtensions {
  /// <summary>
  /// Зашифровать строку, используя MD5.
  /// </summary>
  /// <param name="encryptedString">Исходная строка.</param>
  /// <returns>Зашифрованная с помощью MD5 строка</returns>
  public static string CryptWithMD5(this string encryptedString) {
    Debug.Assert(String.IsNullOrEmpty(encryptedString) == false,
                 "String.IsNullOrEmpty(encryptedString) == false",
                 "Ожидается, что строка будет не пустой");
    if (encryptedString == null) {
      throw new ArgumentNullException("encryptedString",
                                      "Ожидается, что encryptedstring != null");
    }

    if (_md5 == null) {
      _md5 = MD5.Create();
    }

    return Convert.ToBase64String(_md5.ComputeHash(encryptedString.SerializeToUnicode()));
  }

  private static MD5 _md5;

  #region Сериализация строк в байтовый массив

  /// <summary>
  /// Сериализовать строку в байтовый массив, предполагая, что строка хранится в Unicode-формате.
  /// </summary>
  /// <param name="s">Исходная строка.</param>
  /// <returns>байтовый массив</returns>
  public static byte[] SerializeToUnicode(this string s) {
    Debug.Assert(s != null, "s != null");
    return Encoding.Unicode.GetBytes(s);
  }

  /// <summary>
  /// Сериализовать строку в байтовый массив, предполагая, что строка хранится в Utf8-формате.
  /// </summary>
  /// <param name="s">Исходная строка.</param>
  /// <returns>байтовый массив</returns>
  public static byte[] SerializeToUTF8(this string s) {
    Debug.Assert(s != null, "s != null");
    return Encoding.UTF8.GetBytes(s);
  }

  /// <summary>
  /// Сериализовать строку в байтовый массив, предполагая, что строка хранится в Utf7-формате.
  /// </summary>
  /// <param name="s">Исходная строка.</param>
  /// <returns>байтовый массив</returns>
  public static byte[] SerializeToUTF7(this string s) {
    Debug.Assert(s != null, "s != null");
    return Encoding.UTF7.GetBytes(s);
  }

  /// <summary>
  /// Сериализовать строку в байтовый массив, предполагая, что строка хранится в ASCII-формате.
  /// </summary>
  /// <param name="s">Исходная строка.</param>
  /// <returns>байтовый массив</returns>
  public static byte[] SerializeToAnscii(this string s) {
    Debug.Assert(s != null, "s != null");
    return Encoding.ASCII.GetBytes(s);
  }

  #endregion

  /// <summary>
  /// Сгенерировать простенький пароль.
  /// </summary>
  /// <param name="length">Длина пароля.</param>
  /// <returns>Рандомная строка длиной length</returns>
  public static string GeneratePassword(int length) {
    Debug.Assert(length > 0, "length > 0",
                 "Пароль длиной меньше 1 крайне не защищенный !");

    var password = new char[length];

    for (int i = 0; i < length; ++i) {
      password[i] = (char)RandomGenerator.Next(50, 125);
    }

    return new string(password);
  }

  /// <summary>
  /// Преобразовать переносы строки LF в CRLF
  /// </summary>
  /// <param name="val"></param>
  /// <returns></returns>
  public static string ConvertLFToCRLF(this string val) {
    int pos = val.IndexOf("\n");
    while (pos != -1) {
      while (pos == 0 || pos >= 1 && val[pos - 1] != '\r') {
        val = val.Substring(0, pos) + "\r" + val.Substring(pos, val.Length - pos);
        pos = val.IndexOf("\n", pos + 2);
      }
      if (pos == -1) {
        break;
      }
      pos = val.IndexOf("\n", pos + 1);
    }
    return val;
  }

  public static string Increment(this string originalString) {
    var split = Regex.Split(originalString, "\\d*$");
    if (split.Length > 1) {
      var x1 = split[0];
      var x2 = originalString.Substring(x1.Length,
                                        originalString.Length - x1.Length);

      if (String.IsNullOrEmpty(x2)) {
        return originalString + "1";
      }

      try {
        var val = int.Parse(x2);
        val = val < 0 ? 0 : val;

        string sVal = val.ToString();
        int valLen = sVal.Length;

        bool b = true;
        for (int i = 0; i < valLen; ++i) {
          if (sVal[i] != '9') {
            b = false;
            break;
          }
        }
        if (b) {
          valLen++;
        }

        for (int i = 0; i < x2.Length - valLen; ++i) {
          x1 += "0";
        }

        return x1 + (++val).ToString();
      }
      catch (FormatException) {
        return originalString + "1";
      }
    }
    return originalString + "1";
  }

  /// <summary>
  /// Безопасное форматирование строки
  /// </summary>
  /// <param name="message"></param>
  /// <param name="args"></param>
  /// <returns></returns>
  public static string SafeFormat(string message, params object[] args) {
    if (String.IsNullOrEmpty(message)) {
      return String.Empty;
    }

    try {
      message = Regex.Replace(message, @"{(\D[^}]*)}", "{{$1}}");
      return String.Format(message, args);
    }
    catch (Exception innerEx) {
      try {
        return String.Format("Exception in Format():\nИсключение: {0}\nТип исключения:{1}\nИсточник:{2}\nПроизошло:\n{3}\nСведения:{4}\nЧисло аргументов:{5}",
                             innerEx.Message, innerEx.GetType().Name, innerEx.Source, innerEx.StackTrace, message,
                             (args == null ? 0 : args.Length));
      }
      catch (Exception innerEx2) {
        try {
          return String.Format("Exception in Format():\nИсключение: {0}\nТип исключения:{1}\nИсточник:{2}\nПроизошло:\n{3}",
                               innerEx2.Message, innerEx2.GetType().Name, innerEx2.Source, innerEx2.StackTrace);
        }
        catch (Exception) {
          return ExceptionInFormat;
        }
      }
    }
  }

  /// <summary>
  /// Безопасное форматирование строки
  /// </summary>
  /// <param name="cultureInfo"></param>
  /// <param name="message"></param>
  /// <param name="args"></param>
  /// <returns></returns>
  public static string SafeFormat(CultureInfo cultureInfo, string message, params object[] args) {
    if (String.IsNullOrEmpty(message)) {
      return String.Empty;
    }

    try {
      message = Regex.Replace(message, @"{(\D[^}]*)}", "{{$1}}");
      return String.Format(cultureInfo, message, args);
    }
    catch (Exception innerEx) {
      try {
        return String.Format(cultureInfo,
                             "Exception in Format():\nИсключение: {0}\nТип исключения:{1}\nИсточник:{2}\nПроизошло:\n{3}\nСведения:{4}\nЧисло аргументов:{5}",
                             innerEx.Message, innerEx.GetType().Name, innerEx.Source, innerEx.StackTrace, message,
                             (args == null ? 0 : args.Length));
      }
      catch (Exception innerEx2) {
        try {
          return String.Format(cultureInfo,
                               "Exception in Format():\nИсключение: {0}\nТип исключения:{1}\nИсточник:{2}\nПроизошло:\n{3}",
                               innerEx2.Message, innerEx2.GetType().Name, innerEx2.Source, innerEx2.StackTrace);
        }
        catch (Exception) {
          return ExceptionInFormat;
        }
      }
    }
  }

  public const string ExceptionInFormat = "Exception in Format()";

  #region Сравнение строк

  public static bool IsEquals(this string s, string s2) {
    return s.IsEqual(s2);
  }

  public static bool IsEqual(this string s, string s2) {
    if (s2 == null) {
      return false;
    }
    if (s != null && String.IsNullOrEmpty(s) && String.IsNullOrEmpty(s2)) {
      return true;
    }
    return s.CompareTo(s2) == 0;
  }

  public static bool IsEqual(this string s, string s2, bool ignoreCase) {
    if (s2 == null) {
      return false;
    }
    if (s != null && String.IsNullOrEmpty(s) && String.IsNullOrEmpty(s2)) {
      return true;
    }
    return String.Compare(s, s2, ignoreCase) == 0;
  }

  public static bool IsEqual(this string s, string s2, StringComparison stringComparison) {
    if (s2 == null) {
      return false;
    }
    if (s != null && String.IsNullOrEmpty(s) && String.IsNullOrEmpty(s2)) {
      return true;
    }
    return String.Compare(s, s2, stringComparison) == 0;
  }

  public static bool IsEqualIgnoreCase(this string s, string s2) {
    if (s2 == null) {
      return false;
    }
    if (s != null && String.IsNullOrEmpty(s) && String.IsNullOrEmpty(s2)) {
      return true;
    }
    return String.Compare(s, s2, false) == 0;
  }

  public static bool IsSafeEqual(string s, string s2) {
    if (s == null && s2 == null) {
      return true;
    }
    if (s == null || s2 == null) {
      return false;
    }
    return s.IsEqual(s2);
  }

  public static bool IsSafeEqual(string s, string s2, bool ignoreCase) {
    if (s == null && s2 == null) {
      return true;
    }
    if (s == null || s2 == null) {
      return false;
    }
    return s.IsEqual(s2, ignoreCase);
  }

  public static bool IsSafeEqual(string s, string s2, StringComparison stringComparison) {
    if (s == null && s2 == null) {
      return true;
    }
    if (s == null || s2 == null) {
      return false;
    }
    return s.IsEqual(s2, stringComparison);
  }

  #endregion

  #region Получение атрибутов

  public static string GetAttrValue(this string str, string name) {
    return str.GetAttrValue(name, "=", "\"", "\"");
  }

  public static StringAttr GetAttrValue(this string str, string name, string separator,
                                        string leftBorder, string rightBorder) {
    return str.GetAttrValue(name, separator, leftBorder, rightBorder, 0,
                            StringComparison.InvariantCulture);
  }

  public static StringAttr GetAttrValue(this string str, string name, string separator,
                                        string leftBorder, string rightBorder, StringComparison stringComparison) {
    return str.GetAttrValue(name, separator, leftBorder, rightBorder, 0, stringComparison);
  }

  public static StringAttr GetAttrValue(this string str, string name, string separator,
                                        string leftBorder, string rightBorder, int startIndex) {
    return str.GetAttrValue(name, separator, leftBorder, rightBorder, startIndex,
                            StringComparison.InvariantCulture);
  }

  public static StringAttr GetAttrValue(this string str, string name, string separator,
                                        string leftBorder, string rightBorder, int startIndex, StringComparison stringComparison) {
    if (String.IsNullOrEmpty(str)) {
      ExceptionGenerator.Run<ArgumentNullException>("Строка не определена или пустая");
    }

    if (String.IsNullOrEmpty(name)) {
      ExceptionGenerator.Run<ArgumentNullException>("Атрибут не определен");
    }

    if (String.IsNullOrEmpty(separator)) {
      ExceptionGenerator.Run<ArgumentNullException>("Разделитель separator не определен");
    }

    if (leftBorder == null) {
      leftBorder = String.Empty;
    }

    if (rightBorder == null) {
      rightBorder = String.Empty;
    }

    int beginIndex = startIndex < str.Length ? str.IndexOf(String.Format("{0}{1}{2}", name, separator,
                     leftBorder), startIndex, stringComparison) : -1;
    if (beginIndex == -1) {
      StringAttr emptyStringAttr = new StringAttr();
      emptyStringAttr.Name = name;
      emptyStringAttr.Separator = separator;
      emptyStringAttr.LeftBorder = leftBorder;
      emptyStringAttr.RightBorder = rightBorder;
      emptyStringAttr.Value = String.Empty;
      return emptyStringAttr;
    }

    int endIndex = -1;
    if (!String.IsNullOrEmpty(rightBorder)) {
      endIndex = str.IndexOf(rightBorder, beginIndex + name.Length + separator.Length + leftBorder.Length,
                             stringComparison);
    }
    else {
      endIndex = str.Length;
    }

    if (endIndex == -1) {
      string sMsg =
        "Не корректный атрибут данных: endIndex: {0}, beginIndex: {1}, name.Length: {2}, ";
      sMsg += "separator.Length: {3}, leftBorder.Length: {4}, (справочно) rightBorder.Length: {5}, ";
      sMsg += "name: \"{6}\", separator: \"{7}\", leftBorder: \"{8}\", (справочно) rightBorder: \"{9}\", ";
      sMsg += "str.Length: {10}, str = {11}";
      ExceptionGenerator.Run<IndexOutOfRangeException>(sMsg, endIndex, beginIndex,
          name.Length, separator.Length, leftBorder.Length, rightBorder.Length, name, separator,
          leftBorder, rightBorder, str.Length, str);
    }

    if (endIndex - beginIndex - name.Length - separator.Length - leftBorder.Length < 0) {
      string sMsg =
        "Не корректный атрибут данных: endIndex: {0}, beginIndex: {1}, name.Length: {2}, ";
      sMsg += "separator.Length: {3}, leftBorder.Length: {4}, (справочно) rightBorder.Length: {5}, ";
      sMsg += "name: \"{6}\", separator: \"{7}\", leftBorder: \"{8}\", (справочно) rightBorder: \"{9}\", ";
      sMsg += "str.Length: {10}, str = {11}";
      ExceptionGenerator.Run<IndexOutOfRangeException>(sMsg, endIndex, beginIndex,
          name.Length, separator.Length, leftBorder.Length, rightBorder.Length, name, separator,
          leftBorder, rightBorder, str.Length, str);
    }

    StringAttr stringAttr = new StringAttr();
    stringAttr.Name = name;
    stringAttr.StartIndex = beginIndex;
    stringAttr.EndIndex = endIndex;
    stringAttr.Separator = separator;
    stringAttr.LeftBorder = leftBorder;
    stringAttr.RightBorder = rightBorder;

    beginIndex = beginIndex + name.Length + separator.Length + leftBorder.Length;
    int len = endIndex - beginIndex;
    if (len == 0) {
      stringAttr.Value = String.Empty;
    }
    else {
      stringAttr.Value = str.Substring(beginIndex, len);
    }

    return stringAttr;
  }

  #endregion

  #region Поиск слов

  public static bool ExistWord(this string actualString, string word, bool matchCase,
                               int startIndex = 0) {
    StringComparison stringComparisonMode = matchCase ? StringComparison.CurrentCulture :
                                            StringComparison.CurrentCultureIgnoreCase;
    return actualString.ExistWord(word, stringComparisonMode, startIndex);
  }

  public static bool ExistWord(this string actualString, string word,
                               StringComparison stringComparisonMode, int startIndex = 0) {
    if (startIndex < 0 || startIndex > actualString.Length) {
      ExceptionGenerator.RunArgumentOutOfRangeException(startIndex, 0, actualString.Length);
    }
    int index = actualString.IndexOf(word, startIndex, stringComparisonMode);
    if (index >= 0) {
      if (index == 0 && word.Length == actualString.Length) {
        return true;
      }

      if (index > 0 && index + word.Length < actualString.Length) {
        return actualString[index - 1] == ' ' && actualString[index + word.Length] == ' ';
      }
      if (index > 0) {
        return actualString[index - 1] == ' ';
      }
      if (index + word.Length < actualString.Length - 1) {
        return actualString[index + word.Length] == ' ';
      }
    }
    return false;
  }

  public static int FindWord(this string actualString, string word, bool matchCase,
                             int startIndex = 0) {
    StringComparison stringComparisonMode = matchCase ? StringComparison.CurrentCulture :
                                            StringComparison.CurrentCultureIgnoreCase;
    return actualString.FindWord(word, stringComparisonMode, startIndex);
  }

  public static int FindWord(this string actualString, string word,
                             StringComparison stringComparisonMode, int startIndex = 0) {
    if (actualString.ExistWord(word, stringComparisonMode, startIndex)) {
      return actualString.IndexOf(word, startIndex, stringComparisonMode);
    }
    return -1;
  }

  #endregion

  public static bool CharOutQuots(this string actualString, int index, int startIndex = 0) {
    if (index < 0 || index > actualString.Length) {
      ExceptionGenerator.RunArgumentOutOfRangeException(index, 0, actualString.Length);
    }
    if (startIndex < 0 || startIndex > actualString.Length) {
      ExceptionGenerator.RunArgumentOutOfRangeException(startIndex, 0, actualString.Length);
    }
    int q_index = actualString.IndexOf('\"', startIndex);
    if (q_index == -1) {
      return true;
    }

    int q_index_end = actualString.IndexOf('\"', q_index + 1);
    if (q_index > index) {
      return true;
    }
    while (q_index_end > -1 && q_index_end < index) {
      q_index = actualString.IndexOf('\"', q_index_end + 1);
      if (q_index == -1) {
        break;
      }
      q_index_end = actualString.IndexOf('\"', q_index + 1);
    }
    if (q_index_end < index || q_index > index) {
      return true;
    }
    return false;
  }

  public static bool CharOutQuots(this string actualString, int index, ref int nextQuotParaIndex,
                                  int startIndex = 0) {
    if (index < 0 || index > actualString.Length) {
      ExceptionGenerator.RunArgumentOutOfRangeException(index, 0, actualString.Length);
    }
    if (startIndex < 0 || startIndex > actualString.Length) {
      ExceptionGenerator.RunArgumentOutOfRangeException(startIndex, 0, actualString.Length);
    }
    int quotIndex = actualString.IndexOf('\"', startIndex);
    if (quotIndex == -1) {
      return true;
    }

    int quotIndexEnd = actualString.IndexOf('\"', quotIndex + 1);
    if (quotIndex > index) {
      nextQuotParaIndex = quotIndex;
      return true;
    }
    while (quotIndexEnd > -1 && quotIndexEnd < index) {
      quotIndex = actualString.IndexOf('\"', quotIndexEnd + 1);
      if (quotIndex == -1) {
        break;
      }
      quotIndexEnd = actualString.IndexOf('\"', quotIndex + 1);
    }
    if (quotIndex > index) {
      nextQuotParaIndex = quotIndex;
      return true;
    }
    if (quotIndexEnd < index) {
      nextQuotParaIndex = quotIndexEnd + 1;
      return true;
    }
    if (quotIndexEnd > -1 && quotIndexEnd < index) {
      nextQuotParaIndex = quotIndexEnd + 1;
    }
    return false;
  }

  public static bool StringOutQuots(this string actualString, int leftIndex, int rightIndex,
                                    int startIndex = 0) {
    if (leftIndex < 0 || leftIndex > actualString.Length) {
      ExceptionGenerator.RunArgumentOutOfRangeException(leftIndex, 0, actualString.Length);
    }
    if (rightIndex < 0 || rightIndex > actualString.Length) {
      ExceptionGenerator.RunArgumentOutOfRangeException(rightIndex, 0, actualString.Length);
    }
    if (startIndex < 0 || startIndex > actualString.Length) {
      ExceptionGenerator.RunArgumentOutOfRangeException(startIndex, 0, actualString.Length);
    }

    int q_index = actualString.IndexOf('\"', startIndex);
    if (q_index == -1) {
      return true;
    }

    int q_index_end = actualString.IndexOf('\"', q_index + 1);
    if (q_index > rightIndex) {
      return true;
    }
    while (q_index_end > -1 && q_index_end < leftIndex) {
      q_index = actualString.IndexOf('\"', q_index_end + 1);
      if (q_index == -1) {
        break;
      }
      q_index_end = actualString.IndexOf('\"', q_index + 1);
    }
    if (q_index_end < leftIndex || q_index > rightIndex) {
      return true;
    }
    return false;
  }

  public static bool StringOutQuots(this string actualString, string searchStr,
                                    int startIndex = 0) {
    int index = actualString.IndexOf(searchStr, startIndex);
    if (index == -1) {
      return true;
    }
    return actualString.StringOutQuots(index, index + searchStr.Length - 1, startIndex);
  }

  public static bool StringInQuots(this string actualString, string searchStr,
                                   int startIndex = 0) {
    int index = actualString.IndexOf(searchStr, startIndex);
    if (index == -1) {
      return false;
    }
    return actualString.StringOutQuots(index, index + searchStr.Length - 1, startIndex) == false;
  }

  public static T[] Split<T>(this string actualString, char separator,
                             StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries, int count = 0) {
    char[] separators = new char[1] { separator };
    string[] sArray = count == 0 ?
                      actualString.Split(separators, options) : actualString.Split(separators, count, options);
    if (sArray == null || sArray.Length == 0) {
      return null;
    }
    T[] res = new T[sArray.Length];
    Type type = typeof(T);
    for (int i = 0; i < sArray.Length; ++i) {
      res[i] = type.ToTypeValue<T>(sArray[i]);
    }
    return res;
  }

  public static T[] Split<T>(this string actualString, char[] separators,
                             StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries, int count = 0) {
    string[] sArray = count == 0 ?
                      actualString.Split(separators, options) : actualString.Split(separators, count, options);
    if (sArray == null || sArray.Length == 0) {
      return null;
    }
    T[] res = new T[sArray.Length];
    Type type = typeof(T);
    for (int i = 0; i < sArray.Length; ++i) {
      res[i] = type.ToTypeValue<T>(sArray[i]);
    }
    return res;
  }

  public static T[] Split<T>(this string actualString, string separator,
                             StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries, int count = 0) {
    string[] separators = new string[1] { separator };
    string[] sArray = count == 0 ?
                      actualString.Split(separators, options) : actualString.Split(separators, count, options);
    if (sArray == null || sArray.Length == 0) {
      return null;
    }
    T[] res = new T[sArray.Length];
    Type type = typeof(T);
    for (int i = 0; i < sArray.Length; ++i) {
      res[i] = type.ToTypeValue<T>(sArray[i]);
    }
    return res;
  }

  public static T[] Split<T>(this string actualString, string[] separators,
                             StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries, int count = 0) {
    string[] sArray = count == 0 ?
                      actualString.Split(separators, options) : actualString.Split(separators, count, options);
    if (sArray == null || sArray.Length == 0) {
      return null;
    }
    T[] res = new T[sArray.Length];
    Type type = typeof(T);
    for (int i = 0; i < sArray.Length; ++i) {
      res[i] = type.ToTypeValue<T>(sArray[i]);
    }
    return res;
  }
}
}

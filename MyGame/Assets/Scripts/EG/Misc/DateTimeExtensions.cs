using System;
using System.Globalization;

namespace EG.Misc {
public static class DateTimeExtensions {
  public static string ToStr(this DateTime dateTime, string split, string dateSplit,
                             string timeSplit) {
    DateTimeFormatInfo fmt = new CultureInfo(String.Empty).DateTimeFormat;
    if (!String.IsNullOrEmpty(dateSplit)) {
      fmt.ShortDatePattern = String.Format("yyyy{0}MM{0}dd", dateSplit);
    }
    else {
      fmt.ShortDatePattern = "yyyy-MM-dd";
    }
    if (!String.IsNullOrEmpty(timeSplit)) {
      fmt.ShortTimePattern = String.Format("HH{0}mm{0}ss", timeSplit);
    }
    else {
      fmt.ShortTimePattern = "HH:mm:ss";
    }
    return String.Format("{0}{1}{2}", dateTime.ToString("d", fmt), split, dateTime.ToString("t", fmt));
  }

  public static string ToStr(this DateTime dateTime) {
    return dateTime.ToStr(" ", "-", ":");
  }

  public static string ToStrOrDefault(this DateTime dateTime, string split, string dateSplit,
                                      string timeSplit) {
    try {
      return dateTime.ToStr(split, dateSplit, timeSplit);
    }
    catch (Exception ex) {
      if (ex is ArgumentException || ex is ArgumentNullException || ex is InvalidOperationException
          || ex is FormatException || ex is NotSupportedException) {
        return String.Format("{0}{1}{2}", dateTime.ToShortDateString(), split, dateTime.ToLongTimeString());
      }
      throw ExceptionGenerator.Run(ex);
    }
  }

  public static string ToStrOrDefault(this DateTime dateTime) {
    return dateTime.ToStrOrDefault(" ", "-", ":");
  }
}
}

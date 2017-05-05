using System;

namespace EG.Misc {
public static class ArrayExtensions {
  public static string ToStr(this Array items, string split = " ", bool quote = true) {
    if (items == null) {
      return String.Empty;
    }

    if (items.Length == 0) {
      return "()";
    }

    string s = "(";
    foreach (var item in items) {
      if (quote) {
        s += String.Format("\"{0}\",{1}", item.ToString(), split);
      }
      else {
        s += String.Format("{0},{1}", item.ToString(), split);
      }
    }
    return String.Format("{0}{1}", s.Substring(0, s.Length - 1 - split.Length), ")");
  }
}
}

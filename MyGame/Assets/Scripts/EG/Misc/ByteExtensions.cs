using System.Diagnostics;
using System.Text;

namespace EG.Misc {
/// <summary>
/// Класс-расширение для добавления дополнительных методов при работе с байтами
/// </summary>
public static class ByteExtensions {
  public static bool GetBool(this byte i) {
    return i > 0 ? true : false;
  }

  public static bool ToBool(this byte i) {
    return i > 0 ? true : false;
  }

  /// <summary>
  /// Десериализовать строку из байтового массива, предполагая что строка имеет формат Unicode.
  /// </summary>
  /// <param name="v">Байтовый вектор.</param>
  /// <returns>Десериализованная строка</returns>
  public static string DeserializeFromUnicode(this byte[] v) {
    Debug.Assert(v != null, "v != null");
    return Encoding.Unicode.GetString(v);
  }

  /// <summary>
  /// Десериализовать строку из байтового массива, предполагая что строка имеет формат UTF8.
  /// </summary>
  /// <param name="v">Байтовый вектор.</param>
  /// <returns>Десериализованная строка</returns>
  public static string DeserializeFromUTF8(this byte[] v) {
    Debug.Assert(v != null, "v != null");
    return Encoding.UTF8.GetString(v);
  }

  /// <summary>
  /// Десериализовать строку из байтового массива, предполагая что строка имеет формат ASCII.
  /// </summary>
  /// <param name="v">Байтовый вектор.</param>
  /// <returns>Десериализованная строка</returns>
  public static string DeserializeFromASCII(this byte[] v) {
    Debug.Assert(v != null, "v != null");
    return Encoding.ASCII.GetString(v);
  }
}
}

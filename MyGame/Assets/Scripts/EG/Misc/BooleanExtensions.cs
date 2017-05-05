namespace EG.Misc {
/// <summary>
/// Класс-расширение для добавления дополнительных методов при работе с bool
/// </summary>
public static class BooleanExtensions {
  public static int GetInt(this bool b) {
    return b ? 1 : 0;
  }

  public static int ToInt(this bool b) {
    return b ? 1 : 0;
  }

  public static string ToIntString(this bool b) {
    return b ? "1" : "0";
  }
}
}

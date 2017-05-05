namespace EG.Misc {
public static class IntExtensions {
  public static bool GetBool(this int i) {
    return i > 0 ? true : false;
  }

  public static bool ToBool(this int i) {
    return i > 0 ? true : false;
  }
}
}

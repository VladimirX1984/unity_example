using EG;
using UnityEngine;

class GameLevelJsonLoader {

  public static Vector2 GetPos(FullSerializer.fsData jsData) {
    if (!jsData.IsDictionary) {
      return Vector2.zero;
    }
    var data = jsData.AsDictionary;
    return new Vector2((float)data["x"].AsDouble, (float)data["y"].AsDouble);
  }

  public static Vector2 GetSize(FullSerializer.fsData jsData) {
    if (!jsData.IsDictionary) {
      return Vector2.zero;
    }
    var data = jsData.AsDictionary;
    return new Vector2((float)data["w"].AsDouble, (float)data["h"].AsDouble);
  }
}

using UnityEngine;
using System;
using System.Collections;

using EG;
using EG.Objects;

public class Helpers {
  public static IEnumerator SetShield(BaseGameObject obj, Transform transform, float time) {
    GameObject go = EGHelpers.CreateSprite(transform.position,
                                           (GameManager.Instance.GetGameLevel() as GameLevel).shield,
                                           "Shield");
    var sprRender = go.GetComponent<SpriteRenderer>();
    var bounds = sprRender.bounds;
    var size = bounds.size;
    go.transform.localScale = new Vector3(1f, 1f, 1f);
    go.transform.parent = transform;
    obj.IsShield = true;
    var ienumator = EGHelpers.SetGameObjectShield(obj, time);
    UnityEngine.Object.Destroy(go);
    obj.IsShield = false;
    return ienumator;
  }
}

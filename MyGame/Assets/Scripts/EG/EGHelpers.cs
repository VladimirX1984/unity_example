using UnityEngine;
using System;
using System.Collections;

using EG.Kernel;
using EG.Misc;
using EG.Objects;

namespace EG {

  public class EGHelpers {

    public const float _EPSILON = 0.0001f;

    public static IEnumerator Move(Transform obj, float distance, Action act = null) {
      return Move(obj, distance, 0.2f, 0.01f, act);
    }

    public static IEnumerator Move(Transform obj, float distance, float speed, float waitTime,
                                   Action act = null) {
      float yend = obj.position.y + distance;
      float offset = distance > 0f ? speed : -1f * speed;
      while (Mathf.Abs(yend - obj.position.y) > _EPSILON) {
        Vector3 v = obj.position;
        v.y += offset;
        obj.position = v;
        yield return new WaitForSeconds(waitTime);
      }
      Vector3 vend = obj.position;
      vend.y = yend;
      obj.position = vend;
      if (act != null) {
        act();
      }
    }

    public static GameObject CreateSprite(Vector3 position, Sprite sprite, string name,
                                          bool bAddBoxCollider = false) {
      var go = new GameObject(name);
      go.transform.position = position;
      SpriteRenderer spr = go.AddComponent<SpriteRenderer>();
      spr.sprite = sprite;
      if (bAddBoxCollider) {
        go.AddComponent<BoxCollider2D>();
      }
      return go;
    }

    public static GameObject CreateSprite(Vector3 position, Sprite sprite, string name,
                                          Transform parent, bool bAddBoxCollider = false) {
      var go = new GameObject(name);
      go.transform.position = position;
      go.transform.parent = parent;
      SpriteRenderer spr = go.AddComponent<SpriteRenderer>();
      spr.sprite = sprite;
      if (bAddBoxCollider) {
        go.AddComponent<BoxCollider2D>();
      }
      return go;
    }

    public static GameObject CreateSprite(Vector3 position, Sprite sprite, string name,
                                          Type colliderType) {
      var go = new GameObject(name);
      go.transform.position = position;
      SpriteRenderer spr = go.AddComponent<SpriteRenderer>();
      spr.sprite = sprite;
      if (typeof(Collider2D).IsAssignableFrom(colliderType)) {
        go.AddComponent(colliderType);
      }
      return go;
    }

    public static GameObject CreateSprite(Vector3 position, Sprite sprite, string name,
                                          Transform parent, Type colliderType) {
      var go = new GameObject(name);
      go.transform.position = position;
      go.transform.parent = parent;
      SpriteRenderer spr = go.AddComponent<SpriteRenderer>();
      spr.sprite = sprite;
      if (typeof(Collider2D).IsAssignableFrom(colliderType)) {
        go.AddComponent(colliderType);
      }
      return go;
    }

    public static GameObject CreateAnimation(Vector3 position, RuntimeAnimatorController anim,
                                             string name, bool bAddBoxCollider = false) {
      var go = new GameObject(name);
      go.transform.position = position;
      go.AddComponent<SpriteRenderer>();
      Animator a = go.AddComponent<Animator>();
      a.runtimeAnimatorController = anim;
      if (bAddBoxCollider) {
        go.AddComponent<BoxCollider2D>();
      }
      return go;
    }

    public static GameObject CreateAnimation(Vector3 position, RuntimeAnimatorController anim,
                                             string name, Transform parent, bool bAddBoxCollider = false) {
      var go = new GameObject(name);
      go.transform.position = position;
      go.transform.parent = parent;
      go.AddComponent<SpriteRenderer>();
      Animator a = go.AddComponent<Animator>();
      a.runtimeAnimatorController = anim;
      if (bAddBoxCollider) {
        go.AddComponent<BoxCollider2D>();
      }
      return go;
    }

    public static GameObject CreateAnimation(Vector3 position, RuntimeAnimatorController anim,
                                             string name, Type colliderType) {
      var go = new GameObject(name);
      go.transform.position = position;
      go.AddComponent<SpriteRenderer>();
      Animator a = go.AddComponent<Animator>();
      a.runtimeAnimatorController = anim;
      if (typeof(Collider2D).IsAssignableFrom(colliderType)) {
        go.AddComponent(colliderType);
      }
      return go;
    }

    public static GameObject CreateAnimation(Vector3 position, RuntimeAnimatorController anim,
                                             string name, Transform parent, Type colliderType) {
      var go = new GameObject(name);
      go.transform.position = position;
      go.transform.parent = parent;
      go.AddComponent<SpriteRenderer>();
      Animator a = go.AddComponent<Animator>();
      a.runtimeAnimatorController = anim;
      if (typeof(Collider2D).IsAssignableFrom(colliderType)) {
        go.AddComponent(colliderType);
      }
      return go;
    }

    public static T CreateObjectByPrefab<T>(Vector3 position, GameObject prefab, string tag = "")
    where T : BaseMonoObject {
      return CreateObjectByPrefab(position, prefab, null, tag).GetComponent<T>();
    }

    public static T CreateObjectByPrefab<T>(Vector3 position, GameObject prefab, Transform parent,
                                            string tag = "")
    where T : MonoBehaviour {
      return CreateObjectByPrefab(position, prefab, parent, tag).GetComponent<T>();
    }

    public static GameObject CreateObjectByPrefab(Vector3 position, GameObject prefab, Transform parent,
                                                  string tag = "") {
      var go = GameObject.Instantiate(prefab);
      if (!String.IsNullOrEmpty(tag)) {
        go.tag = tag;
      }
      go.transform.position = position;
      if (parent != null) {
        go.transform.parent = parent;
      }
      return go;
    }

    public static T CreateObjectByScript<T>(Vector3 position, string name, string tag = "")
    where T : MonoBehaviour {
      var go = new GameObject(name);
      if (!String.IsNullOrEmpty(tag)) {
        go.tag = tag;
      }
      go.transform.position = position;
      return go.AddComponent<T>();
    }

    public static T CreateObjectByScript<T>(Vector3 position, string name, Transform parent, string tag = "")
    where T : MonoBehaviour {
      var go = new GameObject(name);
      if (!String.IsNullOrEmpty(tag)) {
        go.tag = tag;
      }
      go.transform.position = position;
      go.transform.parent = parent;
      return go.AddComponent<T>();
    }

    public static T CreateSpriteByScript<T>(Vector3 position, Sprite sprite, string name, string tag = "")
    where T : MonoBehaviour {
      var go = new GameObject(name);
      if (!String.IsNullOrEmpty(tag)) {
        go.tag = tag;
      }
      go.transform.position = position;
      var spr = go.AddComponent<SpriteRenderer>();
      spr.sprite = sprite;
      return go.AddComponent<T>();
    }

    public static T CreateSpriteByScript<T>(Vector3 position, Sprite sprite, string name, Transform parent, string tag = "")
    where T : MonoBehaviour {
      var go = new GameObject(name);
      if (!String.IsNullOrEmpty(tag)) {
        go.tag = tag;
      }
      go.transform.position = position;
      go.transform.parent = parent;
      var spr = go.AddComponent<SpriteRenderer>();
      spr.sprite = sprite;
      return go.AddComponent<T>();
    }

    public static T CreateAnimationByScript<T>(Vector3 position, RuntimeAnimatorController anim, string name,
                                               string tag = "")
    where T : MonoBehaviour {
      var go = new GameObject(name);
      if (!String.IsNullOrEmpty(tag)) {
        go.tag = tag;
      }
      go.transform.position = position;
      go.AddComponent<SpriteRenderer>();
      var a = go.AddComponent<Animator>();
      a.runtimeAnimatorController = anim;
      return go.AddComponent<T>();
    }

    public static T CreateAnimationByScript<T>(Vector3 position, RuntimeAnimatorController anim, string name,
                                               Transform parent, string tag = "")
    where T : MonoBehaviour {
      var go = new GameObject(name);
      if (!String.IsNullOrEmpty(tag)) {
        go.tag = tag;
      }
      go.transform.position = position;
      go.transform.parent = parent;
      go.AddComponent<SpriteRenderer>();
      Animator a = go.AddComponent<Animator>();
      a.runtimeAnimatorController = anim;
      return go.AddComponent<T>();
    }

    public static IEnumerator SleepGameObject(IGameObject obj, float time) {
      obj.IsControlable = false;
      yield return new WaitForSeconds(time);
      obj.IsControlable = true;
    }

    public static IEnumerator MultiGameObjectSpeed(IGameObject obj, float coef, float time) {
      obj.Speed *= coef;
      yield return new WaitForSeconds(time);
      obj.Speed /= coef;
    }

    public static IEnumerator SetGameObjectShield(IGameObject obj, float time) {
      obj.IsShield = true;
      yield return new WaitForSeconds(time);
      obj.IsShield = false;
    }

    public static float GetDegAngle(Vector2 vec) {
      float angle = Vector2.Angle(Vector2.right, vec);
      if (vec.y < 0) {
        angle *= -1f;
      }
      return angle;
    }
  }
}
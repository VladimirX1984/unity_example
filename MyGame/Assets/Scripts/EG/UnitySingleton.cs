using UnityEngine;
using System;
using System.Collections;

namespace EG {

public class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour {

  public static bool IsAutoCreate = true;

  public static T Instance {
    get {
      if (applicationIsQuitting) {
        DebugLogger.WriteWarning(
          String.Format("[UnitySingleton] Instance '{0}' already destroyed on application quit. Won't create again - returning null.",
                        typeof(T)));
        return null;
      }

      lock (_lock) {
        if (_instance == null) {
          _instance = (T)FindObjectOfType(typeof(T));

          if (FindObjectsOfType(typeof(T)).Length > 1) {
            DebugLogger.WriteError("[UnitySingleton] Something went really wrong - there should never be more than 1 singleton! Reopening the scene might fix it.");
            return _instance;
          }

          if (_instance == null) {
            if (!IsAutoCreate) {
              DebugLogger.WriteError("No object ([UnitySingleton] Instance) '{0}' int the scene.");
              return _instance;
            }
            GameObject singleton = new GameObject();
            _instance = singleton.AddComponent<T>();
            singleton.name = String.Format("(singleton) {0}", typeof(T).ToString());

            DontDestroyOnLoad(singleton);

            DebugLogger.WriteInfo("[UnitySingleton] An instance of {0} is needed in the scene, so '{1}' was created with DontDestroyOnLoad.",
                                  typeof(T), singleton);
          }
          else {
            DebugLogger.WriteVerbose("[UnitySingleton] Using instance already created: {0}",
                                     _instance.gameObject.name);
          }
        }

        return _instance;
      }
    }
  }
  private static T _instance;

  /// <summary>
  /// When Unity quits, it destroys objects in a random order.
  /// In principle, a UnitySingleton is only destroyed when application quits.
  /// If any script calls Instance after it have been destroyed,
  ///   it will create a buggy ghost object that will stay on the Editor scene
  ///   even after stopping playing the Application. Really bad!
  /// So, this was made to be sure we're not creating that buggy ghost object.
  /// </summary>
  public void OnDestroy() {
    applicationIsQuitting = true;
  }

  private static object _lock = new object();

  private static bool applicationIsQuitting = false;
}
}
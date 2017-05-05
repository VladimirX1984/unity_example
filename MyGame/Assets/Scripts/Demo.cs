using UnityEngine;
using System.Collections;

using EG;

public class Demo : MonoBehaviour {

  // Use this for initialization
  void Start() {
    DebugLogger.WriteInfo("Demo Start");
    GameManager[] gm = FindObjectsOfType<GameManager>();
    if (gm.Length > 0) {
      DestroyImmediate(gm[0].gameObject);
    }
  }

  // Update is called once per frame
  void Update() {
    //if (Input.GetButtonDown("Cancel")) {
    //Application.LoadLevel("level1");
    //}
  }
}

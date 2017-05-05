using UnityEngine;
using System.Collections;
using EG.InputTouch;

public class GameStart : MonoBehaviour {

  // Use this for initialization
  void Start() {
    _gameLogo = GameObject.Find("GameLogo").GetComponent<GameLogo>();
    _gameLogo.OnAnimEnded += OnAnimEnded;

    _input3D = new Input3D();
  }

  void Update() {
    _input3D.Update();
  }

  private GameLogo _gameLogo;
  private IInput3D _input3D;

  private void OnAnimEnded() {
    Application.LoadLevel("Level1");
  }
}

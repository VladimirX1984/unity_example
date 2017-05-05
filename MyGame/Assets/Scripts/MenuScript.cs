using UnityEngine;

public class MenuScript : MonoBehaviour {
  void onGUI() {
    const int buttonWidth = 64;
    const int buttonHeight = 64;
    Rect buttonRect = new Rect(Screen.width / 2 - (buttonWidth / 2),
                               Screen.height / 2 - (buttonHeight / 2),
                               buttonWidth, buttonHeight);
    if (GUI.Button(buttonRect, "Stage1")) {
      GameManager.Instance.StartLevel(1);
    }
  }
}

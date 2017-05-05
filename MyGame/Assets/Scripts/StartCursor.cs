using UnityEngine;
using System.Collections;

using EG.Objects;

public class StartCursor : BaseMonoObject {

  public int MenuItem {
    get { return _menuItem; }
  }
  private int _menuItem = 1;

  // Use this for initialization
  protected override void Start() {
    base.Start();
    __Transform.localPosition = new Vector3(-4.2f, 2.6f);
  }

  // Update is called once per frame
  void Update() {
    //"Horizontal";

    /*if (Input.GetButtonDown("Vertical")) {
      if (Input.GetAxis("Vertical") < 0f) {
        _menuItem++;
      }
      else {
        _menuItem--;
      }
      if (_menuItem > 3) {
        _menuItem = 1;
      }
      if (_menuItem < 1) {
        _menuItem = 3;
      }
      __Transform.localPosition = new Vector3(__Transform.localPosition.x, 1.2f - 0.6f * _menuItem);
      }*/
  }
}

using UnityEngine;

using EG.InputTouch;
using EG.Misc;
using EG.Players;

public class InputManager : MonoBehaviour {

  public MovementTouch MoveTouch {
    get { return _moveTouch; }
  }
  private MovementTouch _moveTouch;

  public FireTouch FireTouch {
    get { return _fireTouch; }
  }
  private FireTouch _fireTouch;

  public void SetPlayer(IPlayer player) {
    _player = player;
    _moveTouch.SetPlayer(player);
    _fireTouch.SetPlayer(player);
  }

  #region MonoBehaviour Events

  // Use this for initialization
  void Start() {
    _input3D = new Input3D();
    _input3D.NotifyTouchedObjects = true;
    var go = GameObject.Find("MovementTouch");
    _moveTouch = go.GetComponent<MovementTouch>();
    go = GameObject.Find("FireTouch");
    _fireTouch = go.GetComponent<FireTouch>();
  }

  // Update is called once per frame
  void Update() {
    if (Application.platform == RuntimePlatform.WindowsPlayer ||
        Application.platform == RuntimePlatform.WindowsEditor ||
        Application.platform == RuntimePlatform.OSXEditor ||
        Application.platform == RuntimePlatform.OSXPlayer) {
      Movement();
      if (Input.GetButtonDown("Fire1")) {
        if (_player != null) {
          _player.Attack();
        }
      }

      _time += Time.deltaTime;
      // тест
      if (_time.IsGreater(_maxTime)) {
        _input3D.Update();
        _time = 0.0f;
      }
      return;
    }
    if (Application.platform == RuntimePlatform.Android ||
        Application.platform == RuntimePlatform.IPhonePlayer) {
      if (Input.touchCount == 0) {
        return;
      }
      _time += Time.deltaTime;
      if (_time.IsGreater(_maxTime)) {
        _input3D.Update();
        _time = 0.0f;
      }
      return;
    }
  }

  /*void FixedUpdate() {
    _input3D.Update();
    }*/

  void OnGUI() {
    //Debug.LogFormat("OnGUI");
    Event ev = Event.current;
    if (ev == null) {
      return;
    }
    if (ev.type == EventType.KeyDown) {

    }
  }

  #endregion

  private IPlayer _player;

  private IInput3D _input3D;

  private float _time;
  private float _maxTime = 0.0199f;

  private void Movement() {
    float vert = Input.GetAxis("Vertical");
    float horiz = Input.GetAxis("Horizontal");
    if (_player != null) {
      _player.Movement(new Vector2(horiz, vert));
    }
  }
}

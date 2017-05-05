using System.Collections.Generic;
using UnityEngine;

namespace EG.InputTouch {
/// <summary>
/// Эмулируем прикосновения к экрану мышью
/// </summary>
public class MouseTouchProvider : ITouchProvider {

  private readonly List<Touch> _touches = new List<Touch>();
  private Vector2 _prevMouseClick;
  private float _prevMouseClickTime;
  private bool _clicked;

  public IEnumerable<Touch> Touches {
    get { return _touches; }
  }

  public void Update() {
    _touches.Clear();

    if (Input.GetMouseButton(0)) {
      _clicked = true;

      var touch = new Touch {
        Id = 1,
        Position = new Vector2(Input.mousePosition.x, Input.mousePosition.y),
        TouchFirstFrame = Input.GetMouseButtonDown(0)
      };
      if (!touch.TouchFirstFrame) {
        touch.DeltaPosition = touch.Position - _prevMouseClick;
        touch.DeltaTime = UnityEngine.Time.time - _prevMouseClickTime;
      }
      _touches.Add(touch);

      _prevMouseClick = touch.Position;
      _prevMouseClickTime = UnityEngine.Time.time;
    }
    else if (_clicked) {
      _clicked = false;

      var touch = new Touch {
        Id = 1,
        Position = new Vector2(Input.mousePosition.x, Input.mousePosition.y),
        DeltaTime = UnityEngine.Time.time - _prevMouseClickTime,
        TouchLastFrame = true
      };
      _touches.Add(touch);
    }
  }

}

}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EG.InputTouch {
/// <summary>
/// Класс, проецирующий прикосновения к экрану на игровой мир
/// </summary>
public class Input3D : IInput3D {

  private List<Touch3D> _touches = new List<Touch3D>();
  private readonly InputController _input = new InputController();

  public bool NotifyTouchedObjects { get; set; }

  public IEnumerable<Touch3D> Touches {
    get { return _touches; }
  }

  public void Update() {
    _input.Update();

    var newTouches = new List<Touch3D>();
    foreach (var touch2D in _input.Touches) {
      Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch2D.Position.x, touch2D.Position.y));
      RaycastHit[] hits = Physics.RaycastAll(ray);
      foreach (var hit in hits) {
        var touch3D = new Touch3D {
          Touch2D = touch2D,
          is3DHit = true,
          Hit = hit
        };
        var prevTouch = _touches.FirstOrDefault(t => t.Touch2D.Id == touch2D.Id);
        if (prevTouch != null) {
          touch3D.PrevTouch2D = prevTouch.Touch2D;
          touch3D.TouchLastFrame = touch2D.TouchLastFrame;
        }
        else {
          touch3D.TouchFirstFrame = true;
        }
        newTouches.Add(touch3D);
      }

      RaycastHit2D[] hits2D = Physics2D.GetRayIntersectionAll(ray);
      foreach (var hit in hits2D) {
        var touch3D = new Touch3D {
          Touch2D = touch2D,
          is3DHit = false,
          Hit2D = hit
        };
        var prevTouch = _touches.FirstOrDefault(t => t.Touch2D.Id == touch2D.Id);
        if (prevTouch != null) {
          touch3D.PrevTouch2D = prevTouch.Touch2D;
          touch3D.TouchLastFrame = touch2D.TouchLastFrame;
        }
        else {
          touch3D.TouchFirstFrame = true;
        }
        newTouches.Add(touch3D);
      }
    }

    _touches = newTouches;

    if (NotifyTouchedObjects) {
      foreach (var touch in _touches) {
        GameObject obj = null;
        if (touch.is3DHit) {
          obj = touch.Hit.collider.gameObject;
        }
        else {
          obj = touch.Hit2D.collider.gameObject;
        }

        if (obj) {
          MonoBehaviour[] comps = obj.GetComponents<MonoBehaviour>();
          foreach (var comp in comps) {
            if (typeof(IInputCatcher).IsAssignableFrom(comp.GetType())) {
              ((IInputCatcher)comp).WasHitted(touch);
            }
          }
        }
      }
    }
  }
}
}
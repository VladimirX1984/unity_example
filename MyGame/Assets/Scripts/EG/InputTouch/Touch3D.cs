using UnityEngine;

namespace EG.InputTouch {
/// <summary>
/// Проекция прикосновения на игровое поле
/// </summary>
public class Touch3D {

  public Touch Touch2D { get; set; }

  public Touch PrevTouch2D { get; set; }

  public RaycastHit Hit { get; set; }

  public RaycastHit2D Hit2D { get; set; }

  public bool is3DHit { get; set; }

  public bool TouchFirstFrame { get; set; }

  public bool TouchLastFrame { get; set; }

}
}
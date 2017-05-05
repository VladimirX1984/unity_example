using UnityEngine;

using EG.InputTouch;
using EG.Objects;
using EG.Players;

public class MovementTouch : BaseMonoObject, IInputCatcher {

  #region Реализация интерфейса IInputCatcher

  public void WasHitted(Touch3D touch) {
    if (_player == null) {
      return;
    }

    if (touch.TouchLastFrame) {
      return;
    }
    var pos = touch.Hit2D.point;
    var dir = pos - (Vector2)__Transform.position;
    _player.Movement(dir);
  }

  #endregion

  public void SetPlayer(IPlayer player) {
    _player = player;
    _player.IsAnyDir = true;
  }

  private IPlayer _player;
}

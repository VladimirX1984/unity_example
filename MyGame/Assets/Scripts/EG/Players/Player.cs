using UnityEngine;

using EG.Misc;
using EG.Objects;

namespace EG.Players {
  [RequireComponent(typeof(Animator))]
  [RequireComponent(typeof(SpriteRenderer))]
  public abstract class Player : BaseGameObject, IPlayer {

    #region Реализация интерфейса IPlayer

    public int Score {
      get { return _score; }
      set {
        if (_score != value) {
          var prev = _score;
          _score = value;
          if (OnParamChanged != null) {
            OnParamChanged("Score", prev, _score);
          }
        }
      }
    }
    private volatile int _score = 0;

    public bool IsAnyDir { get; set; }

    public void Movement(Vector2 vec) {
      if (__isInit) {
        return;
      }

      if (bStopped) {
        bStopped = false;
        return;
      }

      if (!IsControlable) {
        if (_lastHoriz.IsNotEqual(0f) || _lastVert.IsNotEqual(0f)) {
          StopMove();
        }
        return;
      }

      vec = vec.normalized;
      float horiz = vec.x, vert = vec.y;

      if (vert.IsEqual(0f) && horiz.IsEqual(0f)) {
        _lastHoriz = 0;
        _lastVert = 0;
      }

      if (!BeforeMovement(_lastDiscreteDirection)) {
        return;
      }

      if (vert.IsNotEqual(0f, 0.1f) && horiz.IsNotEqual(0f, 0.1f)) {
        if (vert > 0f && horiz > 0f) {
          DiscreteDirection = 1;
        }
        else if (vert > 0f && horiz < -0f) {
          DiscreteDirection = 3;
        }
        else if (vert < -0f && horiz < -0f) {
          DiscreteDirection = 5;
        }
        else if (vert < -0f && horiz > 0f) {
          DiscreteDirection = 7;
        }
        SetMoveDirection(new Vector2(horiz, vert));
        _lastHoriz = horiz;
        _lastVert = vert;
      }
      else if (vert.IsNotEqual(0f)) {
        DiscreteDirection = vert > 0f ? 2 : 6;
        _lastVert = vert;
        if (IsAnyDir) {
          SetMoveDirection(new Vector2(horiz, vert));
          _lastHoriz = horiz;
        }
        else {
          SetMoveDirection(new Vector2(0f, vert));
        }
      }
      else if (horiz.IsNotEqual(0f)) {
        DiscreteDirection = horiz > 0f ? 0 : 4;
        _lastHoriz = horiz;
        if (IsAnyDir) {
          SetMoveDirection(new Vector2(horiz, vert));
          _lastVert = vert;
        }
        else {
          SetMoveDirection(new Vector2(horiz, 0f));
        }
      }

      if (!BeforeTranslate(_lastDiscreteDirection)) {
        return;
      }

      Move();
    }

    #endregion

    public bool IsAnimByPhysics = true;

    public override void StopMove() {
      _lastVert = 0f;
      _lastHoriz = 0f;
      base.StopMove();
      bStopped = true;
    }

    #region MonoBehaviour Events

    protected virtual void Update() {
      if (IsAnimByPhysics) {
        return;
      }

      if (_lastHoriz == 0f && _lastVert == 0f) {
        StopMovingAnim();
      }
      else {
        StartMovingAnim();
      }
      if (_lastDiscreteDirection != DiscreteDirection) {
        _lastDiscreteDirection = DiscreteDirection;
        DebugLogger.WriteInfo("Player.Update DiscreteDirection = {0}", DiscreteDirection);
        ChangeDiscreteDirection();
      }

      if (_lastVert == 0f && _lastHoriz == 0f) {
        SetMoveDirection(Vector2.zero);
      }
    }

    protected virtual void FixedUpdate() {
      if (!IsAnimByPhysics) {
        return;
      }

      if (_lastHoriz == 0f && _lastVert == 0f) {
        StopMovingAnim();
      }
      else {
        StartMovingAnim();
      }
      if (_lastDiscreteDirection != DiscreteDirection) {
        _lastDiscreteDirection = DiscreteDirection;
        ChangeDiscreteDirection();
      }

      if (_lastVert == 0f && _lastHoriz == 0f) {
        SetMoveDirection(Vector2.zero);
      }
    }

    #endregion

    protected virtual void StartMovingAnim() {

    }

    protected virtual void StopMovingAnim() {

    }

    protected virtual bool BeforeMovement(int prevDirection) {
      return true;
    }

    protected virtual bool BeforeTranslate(int prevDirection) {
      return true;
    }

    protected override void DefineDirection(Vector2 vec) {
      float horiz = vec.x, vert = vec.y;
      if (vert.IsNotEqual(0f, 0.1f) && horiz.IsNotEqual(0f, 0.1f)) {
        if (vert > 0f && horiz > 0f) {
          DiscreteDirection = 1;
        }
        else if (vert > 0f && horiz < -0f) {
          DiscreteDirection = 3;
        }
        else if (vert < -0f && horiz < -0f) {
          DiscreteDirection = 5;
        }
        else if (vert < -0f && horiz > 0f) {
          DiscreteDirection = 7;
        }
      }
      else if (vert.IsNotEqual(0f)) {
        DiscreteDirection = vert > 0f ? 2 : 6;
      }
      else if (horiz.IsNotEqual(0f)) {
        DiscreteDirection = horiz > 0f ? 0 : 4;
      }
      ChangeDiscreteDirection();
    }

    private float _lastVert;
    private float _lastHoriz;
    private int _lastDiscreteDirection;

    private bool bStopped = false;
  }
}

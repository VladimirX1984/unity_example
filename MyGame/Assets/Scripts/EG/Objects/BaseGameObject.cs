using UnityEngine;
using System;
using System.Collections;

using EG.Kernel;
using EG.Misc;

namespace EG.Objects {

  public abstract class BaseGameObject : BaseMonoObject, IGameObject {

    #region Реализация интерфейса INamedObject

    public string Name {
      get { return name; }
      set { name = (value == null ? String.Empty : value); }
    }

    #endregion

    #region Реализация интерфейса IGameObject

    public int TypeId {
      get { return _typeId; }
      protected set { _typeId = value; }
    }
    private int _typeId;

    public float Health {
      get { return _health; }
      protected set {
        if (_health.IsNotEqual(value)) {
          var prev = _health;
          _health = value;
          if (OnParamChanged != null) {
            OnParamChanged("Health", prev, _health);
          }
        }
      }
    }
    private volatile float _health = 1f;

    public float Speed {
      get { return _speed; }
      set {
        if (_speed.IsNotEqual(value)) {
          var prev = _speed;
          _speed = value;
          if (OnParamChanged != null) {
            OnParamChanged("Speed", prev, _speed);
          }
        }
      }
    }
    private float _speed = 0f;

    public float Acceleration {
      get { return _acceleration; }
      set {
        if (_acceleration.IsNotEqual(value)) {
          var prev = _acceleration;
          _acceleration = value;
          if (OnParamChanged != null) {
            OnParamChanged("Acceleration", prev, _acceleration);
          }
        }
      }
    }
    private float _acceleration = 0f;

    public int DiscreteDirection {
      get { return _discreteDirection; }
      set { _discreteDirection = value; }
    }
    private int _discreteDirection = 0;

    public bool IsBonus { get; set; }

    public bool IsControlable { get; set; }

    public bool IsShield { get; set; }

    public bool IsAlive { get { return Health.IsGreater(0.0f); } }

    public float HitForce { get; set; }

    public virtual int Level {
      get { return _level; }
      set { _level = value; }
    }
    private int _level;

    public void AddObserver(ActionOnObject diedObject) {
      _DiedObject += diedObject;
    }

    public void RemoveObserver(ActionOnObject diedObject) {
      _DiedObject -= diedObject;
    }

    public Vector2 GetMoveDirection() {
      return _moveDir;
    }

    public void SetMoveDirection(float x, float y) {
      _moveDir.x = x;
      _moveDir.y = y;
      _moveDir.Normalize();
      if (OnMoveDirChanged != null) {
        OnMoveDirChanged(_moveDir);
      }
    }

    public void SetMoveDirection(Vector2 vec) {
      SetMoveDirection(vec.x, vec.y);
    }

    public void Move() {
      if (__isInit) {
        return;
      }

      if (!IsControlable) {
        return;
      }

      if (_speed.IsEqual(0.0f)) {
        return;
      }


      var vec = _moveDir * _speed * Time.deltaTime;
      var lastPos = new Vector3(__Transform.localPosition.x, __Transform.localPosition.y, __Transform.localPosition.z);
      __Transform.Translate(vec, Space.World);
      var moved = __Transform.localPosition - lastPos;
      if (Acceleration.IsNotEqual(0.0f) && _speed.IsGreater(0.0f)) {
        _speed += Acceleration * Time.deltaTime;
        if (_speed.IsLess(0.0f)) {
          _speed = 0.0f;
        }
      }
      if (moved != Vector3.zero && OnMoved != null) {
        OnMoved(moved);
      }
    }

    public virtual void StopMove() {
      _moveDir = Vector2.zero;
    }

    public void SetPosition(float x, float y) {
      if (__isInit) {
        return;
      }
      __Transform.localPosition = new Vector3(x, y, 0);
      if (OnPosSetted != null) {
        OnPosSetted(__Transform.localPosition);
      }
    }

    public void SetPosition(Vector2 pos) {
      SetPosition(pos.x, pos.y);
    }

    public virtual float Hit(float damage) {
      if (__isInit) {
        return 0f;
      }
      if (IsShield) {
        return 0f;
      }
      Health -= damage;
      if (Health.IsLess(0f)) {
        return Health;
      }
      return damage;
    }

    public void Attack() {
      if (__isInit) {
        return;
      }
      if (!IsControlable) {
        return;
      }
      AttackOn(Vector2.zero);
    }

    public void Attack(Vector2 vec) {
      if (__isInit) {
        return;
      }
      if (!IsControlable) {
        return;
      }
      vec = vec.normalized;
      DefineDirection(vec);
      AttackOn(vec);
    }

    public virtual void Reset() {

    }

    public virtual void SetColliderTrigger(bool bValue) {

    }

    public event Action<Vector2> OnMoved;
    public event Action<Vector2> OnPosSetted;

    #endregion

    public BaseGameObject() : base() {
      IsControlable = true;
      HitForce = 1.0f;
      Health = 1.0f;
    }

    protected bool __isInit = true;

    protected ActionOnObject _DiedObject;

    protected virtual void AttackOn(Vector2 vec) {

    }

    protected virtual void DefineDirection(Vector2 vec) {

    }

    protected virtual void ChangeDiscreteDirection() {

    }

    protected virtual void _Dead(int reason) {
      if (_DiedObject != null) {
        _DiedObject(this, reason);
      }
    }

    protected void SetXYSignMoveDir() {
      _moveDir *= -1f;
      if (OnMoveDirChanged != null) {
        OnMoveDirChanged(_moveDir);
      }
    }

    protected void SetXSignMoveDir() {
      _moveDir.x *= -1f;
      if (OnMoveDirChanged != null) {
        OnMoveDirChanged(_moveDir);
      }
    }

    protected void SetYSignMoveDir() {
      _moveDir.y *= -1f;
      if (OnMoveDirChanged != null) {
        OnMoveDirChanged(_moveDir);
      }
    }

    protected void SetXZeroMoveDir() {
      _moveDir.x = 0f;
      if (OnMoveDirChanged != null) {
        OnMoveDirChanged(_moveDir);
      }
    }

    protected void SetYZeroMoveDir() {
      _moveDir.y = 0f;
      if (OnMoveDirChanged != null) {
        OnMoveDirChanged(_moveDir);
      }
    }

    protected Action<Vector2> OnMoveDirChanged;

    #region MonoBehaviour Events

    // Use this for initialization
    protected override void Start() {
      base.Start();
      IsControlable = true;
      __isInit = false;
    }

    #endregion

    protected IEnumerator __SleepGameObject(float time) {
      return EGHelpers.SleepGameObject(this, time);
    }

    protected IEnumerator __SleepGameObject(IGameObject obj, float time) {
      return EGHelpers.SleepGameObject(obj, time);
    }

    protected IEnumerator __MultiSpeed(float coef, float time) {
      return EGHelpers.MultiGameObjectSpeed(this, coef, time);
    }

    private Vector2 _moveDir = Vector2.zero;
  }
}
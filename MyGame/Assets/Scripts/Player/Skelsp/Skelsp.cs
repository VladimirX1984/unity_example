using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using EG;
using EG.Enemy;
using EG.Neutral;
using EG.Players;

public class Skelsp : Player {

  public bool IsAnimUpdating {
    get { return _bAnimStarted; }
    set { _bAnimStarted = value; }
  }
  private bool _bAnimUpdating;

  public RuntimeAnimatorController rAnimController;

  public int Ammo {
    get { return _gun.Ammo; }
  }

  public Gun.Weapons CarringWeapo {
    get { return _gun.carringWeapo; }
  }

  public void OnStartFire() {
    DebugLogger.WriteVerbose("OnStartFire _bFire = {0}", _bFire);
    lock (_fireEndLock) {
      if (_fireEnd == null) {
        _fireEnd = StartCoroutine(FireWait());
      }
    }
  }

  public void KillEnemy(IEnemy enemy) {
    lock (_objectTriggerIdListLock) {
      if (_objectTriggerIdList.Remove(enemy.Id)) {
        if (!_objectTriggerIdList.Any()) {
          DebugLogger.WriteInfo("Skelsp.KillEnemy _collider2D.isTrigger = {0}", _collider2D.isTrigger);
          _collider2D.isTrigger = false;
        }
      }
    }
    switch (enemy.TypeId) {
      case (int)GameObjectType.Portal: {
        var portal = enemy as Portal;
        Score += (3000 + 500 * portal.Level);
      }
      break;
      case (int)GameObjectType.SmallMonster: {
        Score += 100;
      }
      break;
      case (int)GameObjectType.Tendrils: {
        Score += 200;
      }
      break;
      case (int)GameObjectType.IntelligentTendrils: {
        Score += 300;
      }
      break;
      case (int)GameObjectType.Mouth: {
        Score += 400;
      }
      break;
      case (int)GameObjectType.IntelligentMouth: {
        Score += 500;
      }
      break;
      case (int)GameObjectType.Beetle: {
        Score += 1000;
      }
      break;
    }
  }

  public override float Hit(float damage) {
    var residue = base.Hit(damage);
    DebugLogger.WriteVerbose("Skelsp.Hit Health = {0}", Health);
    if (!IsAlive) {
      _Dead(0);
    }
    return residue;
  }

  public override void Reset() {
    if (__isInit) {
      return;
    }
    SetPosition(0, 0);
    DiscreteDirection = 0;
    //_animator.SetInteger("fire", 0);
    ChangeDiscreteDirection();
  }

  public void ApplyGem(GameBonusType bonusType) {
    DebugLogger.WriteInfo("Skelsp.ApplyGem bonusType = {0}", bonusType.ToString());
    switch (bonusType) {
      case GameBonusType.UpgradeGun: {
        _gun.Upgrade();
      }
      break;
      case GameBonusType.ChildrenShield: {

      } break;
      case GameBonusType.Time: {
        (GameManager.Instance.GetGameLevel() as GameLevel).SleepEnemies((GameManager.Instance.GetGameLevel()
            as GameLevel).PlayerUpgrade.SleepTime);
      }
      break;
      case GameBonusType.Speed: {
        StartCoroutine(__MultiSpeed(2.0f,
                                    (GameManager.Instance.GetGameLevel() as GameLevel).PlayerUpgrade.SpeedTime));
      }
      break;
      case GameBonusType.Shield: {
        StartCoroutine(__GetShield((GameManager.Instance.GetGameLevel() as
                                    GameLevel).PlayerUpgrade.ShieldTime));
      }
      break;
      case GameBonusType.Bullet: {
        _gun.AddBullet();
      }
      break;
      case GameBonusType.Live: {
        Health += (GameManager.Instance.GetGameLevel() as GameLevel).PlayerUpgrade.Health;
      }
      break;
    }
  }

  public void SetGunType(Gun.Weapons weapon) {
    _gun.SetCarringWeapo(weapon);
  }

  public event Action<int> OnAmmoChanged;
  public event Action<Gun.Weapons> OnCarringWeapoChanged;

  protected override void _Dead(int reason) {
    DebugLogger.WriteInfo("Skelsp._Dead reason = {0}", reason);
    base._Dead(reason);
    _gun.StopFire();
    IsControlable = false;
    _animator.SetInteger("direction", 8);
    //Destroy(gameObject);
  }

  #region MonoBehaviour Events

  // Use this for initialization
  protected override void Start() {
    base.Start();
    TypeId = (int)GameObjectType.Skelsp;
    _collider2D = GetComponent<BoxCollider2D>();
    Rigidbody2D rig = GetComponent<Rigidbody2D>();
    rig.constraints = RigidbodyConstraints2D.FreezeRotation;
    Health = 30f;
    Speed = 1f;
    _animator = GetComponent<Animator>();
    _animator.runtimeAnimatorController = rAnimController;
    _animator.speed = 0;
    _gun = GetComponent<Gun>();
    _gun.Init(__Transform);
    _gun.OnAmmoChanged += _OnAmmoChanged;
    _gun.OnCarringWeapoChanged += _OnCarringWeapoChanged;
    _objectTriggerIdList = new List<float>();
    _objectTriggerIdListLock = new object();
    //isAnimByPhysics = false;
  }

  #region Collision

  void OnCollisionEnter2D(Collision2D other) {
    DebugLogger.WriteInfo("Skelsp.OnCollisionEnter2D other.gameObject.tag = {0}",
                          other.gameObject.tag);
    switch (other.gameObject.tag) {
      case GameLevel.BorderTagName: {
        StopMove();
      }
      break;
      case GameLevel.EnemyTagName: {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null && enemy.IsAlive) {
          lock (_objectTriggerIdListLock) {
            _collider2D.isTrigger = true;
          }
        }
      }
      break;
      case GameLevel.TerrainTagName: {
        var neutralObject = other.gameObject.GetComponent<NeutralObject>();
        if (neutralObject != null && neutralObject.TypeId == (int)GameObjectType.Tree) {
          StopMove();
          //StopMovingAnim();
        }
      }
      break;
    }
  }

  void OnCollisionStay2D(Collision2D other) {
    DebugLogger.WriteVerbose("Skelsp OnCollisionStay2D other.gameObject.tag = {0}",
                             other.gameObject.tag);
    switch (other.gameObject.tag) {
      case GameLevel.BorderTagName: {
        StopMovingAnim();
      }
      break;
      case GameLevel.EnemyTagName: {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null && enemy.IsAlive) {
          DebugLogger.WriteInfo("Skelsp.OnCollisionStay2D enemy");
          lock (_objectTriggerIdListLock) {
            _collider2D.isTrigger = true;
          }
        }
      }
      break;
      case GameLevel.TerrainTagName: {
        var neutralObject = other.gameObject.GetComponent<NeutralObject>();
        if (neutralObject != null && neutralObject.TypeId == (int)GameObjectType.Tree) {
          StopMove();
          StopMovingAnim();
        }
      }
      break;
    }
  }

  void OnTriggerEnter2D(Collider2D other) {
    //DebugLogger.WriteInfo("Skelsp.OnTriggerEnter2D other.gameObject.tag = {0}",
    //                    other.gameObject.tag);
    switch (other.gameObject.tag) {
      case GameLevel.BorderTagName: {
        var border = other.gameObject.GetComponent<Border>();
        BorderTriggerHandle(border);
      }
      break;
      case GameLevel.BulletTagName: {
      } break;
      case GameLevel.EnemyTagName: {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null && enemy.IsAlive) {
          lock (_objectTriggerIdListLock) {
            _objectTriggerIdList.Add(enemy.Id);
          }
          Hit(enemy.HitForce * Time.deltaTime);
        }
      }
      break;
      case GameLevel.TerrainTagName: {
        var neutralObject = other.gameObject.GetComponent<NeutralObject>();
        if (neutralObject != null && neutralObject.TypeId == (int)GameObjectType.Tree) {
          SetXYSignMoveDir();
          Move();
          StopMove();
          StopMovingAnim();
        }
      }
      break;
    }
  }

  void OnTriggerStay2D(Collider2D other) {
    switch (other.gameObject.tag) {
      case GameLevel.BorderTagName: {
        var border = other.gameObject.GetComponent<Border>();
        BorderTriggerHandle(border);
      }
      break;
      case GameLevel.BulletTagName: {
      } break;
      case GameLevel.EnemyTagName: {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null && enemy.IsAlive) {
          Hit(enemy.HitForce * Time.deltaTime);
        }
      }
      break;
      case GameLevel.TerrainTagName: {
        var neutralObject = other.gameObject.GetComponent<NeutralObject>();
        if (neutralObject != null && neutralObject.TypeId == (int)GameObjectType.Tree) {
          SetXYSignMoveDir();
          Move();
          StopMove();
          StopMovingAnim();
        }
      }
      break;
    }
  }

  void OnTriggerExit2D(Collider2D other) {
    DebugLogger.WriteVerbose("Skelsp.OnTriggerExit2D other.gameObject.tag = {0}", other.gameObject.tag);
    if (other.gameObject.tag == GameLevel.BulletTagName) {
      return;
    }
    if (other.gameObject.tag == GameLevel.EnemyTagName) {
      var enemy = other.gameObject.GetComponent<Enemy>();
      lock (_objectTriggerIdListLock) {
        _objectTriggerIdList.Remove(enemy.Id);
        if (!_objectTriggerIdList.Any()) {
          DebugLogger.WriteInfo("Skelsp.OnTriggerExit2D _collider2D.isTrigger = {0}", _collider2D.isTrigger);
          _collider2D.isTrigger = false;
        }
      }
    }
  }

  #endregion

  #endregion

  #region Переопределение методов класса Player

  protected override void StartMovingAnim() {
    if (!_bAnimStarted) {
      _bAnimStarted = true;
      _animator.speed = 1;
    }
  }

  protected override void StopMovingAnim() {
    if (_bAnimStarted) {
      _bAnimStarted = false;
      _animator.speed = 0;
    }
  }

  protected override bool BeforeMovement(int prevDirection) {
    if (IsAnimUpdating && prevDirection != DiscreteDirection && !_gun.IsMakeShot && !_gun.IsBurst) {
      DebugLogger.WriteVerbose("Skelsp.BeforeMovement ignore translate at begin");
      return false;
    }
    return true;
  }

  protected override bool BeforeTranslate(int prevDirection) {
    if (IsAnimUpdating && prevDirection != DiscreteDirection && !_gun.IsMakeShot && !_gun.IsBurst) {
      DebugLogger.WriteVerbose("Skelsp.BeforeTranslate ignore translate at end");
      return false;
    }
    return true;
  }

  protected override void AttackOn(Vector2 vec) {
    DebugLogger.WriteVerbose("AttackOn _gun.IsMakeShot = {0}; vec = {1}", _gun.IsMakeShot,
                             vec.ToString());
    _bFire = true;
    if (!_gun.IsMakeShot) {
      _gun.MakeShot(vec);
      //_animator.SetInteger("fire", 1);
      OnStartFire();
      _gun.StartFire();
    }
  }

  protected override void ChangeDiscreteDirection() {
    DebugLogger.WriteVerbose("Skelsp.ChangeDiscreteDirection _direction = {0}", DiscreteDirection);
    _animator.SetInteger("direction", DiscreteDirection);
    _gun.SetDirection(DiscreteDirection);
  }

  #endregion

  private Gun _gun;

  private volatile bool _bFire;

  private Animator _animator;

  private volatile bool _bAnimStarted;

  private Coroutine _fireEnd;

  private BoxCollider2D _collider2D;

  private object _objectTriggerIdListLock;
  private List<float> _objectTriggerIdList;

  private object _fireEndLock = new object();

  private void StopFire() {
    DebugLogger.WriteVerbose("StopFire bFire = {0}", _bFire);
    if (_bFire) {
      _bFire = false;
      lock (_fireEndLock) {
        if (_fireEnd != null) {
          StopCoroutine(_fireEnd);
          _fireEnd = null;
        }
      }
      //_animator.SetInteger("fire", 0);
      ChangeDiscreteDirection();
    }
  }

  private IEnumerator FireWait() {
    yield return new WaitForSeconds(0.5f);
    StopFire();
    lock (_fireEndLock) {
      _fireEnd = null;
    }
  }

  private void BorderTriggerHandle(Border border) {
    switch (border.name) {
      case "borderTop": {
        SetXZeroMoveDir();
        SetYSignMoveDir();
        Move();
      }
      break;
      case "borderBottom": {
        SetXZeroMoveDir();
        SetYSignMoveDir();
        Move();
      }
      break;
      case "borderLeft": {
        SetXSignMoveDir();
        SetYZeroMoveDir();
        Move();
      }
      break;
      case "borderRight": {
        SetXSignMoveDir();
        SetYZeroMoveDir();
        Move();
      }
      break;
    }

    StopMove();
    StopMovingAnim();
  }

  private IEnumerator __GetShield(float time) {
    return Helpers.SetShield(this, __Transform, time);
  }

  private void _OnAmmoChanged(int ammo) {
    if (OnAmmoChanged != null) {
      OnAmmoChanged(ammo);
    }
  }

  private void _OnCarringWeapoChanged(Gun.Weapons carringWeapo) {
    if (OnCarringWeapoChanged != null) {
      OnCarringWeapoChanged(carringWeapo);
    }
  }
}

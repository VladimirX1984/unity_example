using System;
using System.Collections;
using UnityEngine;

using EG;
using EG.Enemy;

using EG.Misc;

[RequireComponent(typeof(Rigidbody2D))]
public class Beetle : EnemyEmitter {

  #region Реализация абстрактного класса

  protected override void CreateEnemy() {
    ++_count;
    var vec = _collider2D.bounds.size;
    var moveDir = GetMoveDirection();
    vec.x *= moveDir.x;
    vec.y *= moveDir.y;

    if (Level == 0) {
      var enemy = EGHelpers.CreateObjectByPrefab<Monster>(__Transform.localPosition + vec,
                  GameManager.Instance.GetMonoGameLevel().prefabSmallMonster, __Transform.parent);
      StartCoroutine(OnCreateEnemy(enemy));
      enemy.Speed = Speed + UnityEngine.Random.Range(0.9f, 1.1f);
      enemy.SetMoveDirection(GetMoveDirection());
      return;
    }

    if (Level == 1) {
      var enemy = EGHelpers.CreateObjectByPrefab<Monster>(__Transform.localPosition + vec,
                  GameManager.Instance.GetMonoGameLevel().prefabTendrils, __Transform.parent);
      StartCoroutine(OnCreateEnemy(enemy));
      enemy.Level = _count % 3 == 0 ? 1 : 0;
      enemy.Speed = Speed + UnityEngine.Random.Range(0.7f, 0.9f);
      enemy.SetMoveDirection(GetMoveDirection());
      return;
    }
  }

  #endregion

  public override float Hit(float damage) {
    float residue = base.Hit(damage);
    if (!IsAlive) {
      IsControlable = false;
      Color c = spriteRender.color;
      c.a = 0.65f;
      spriteRender.color = c;
      StartCoroutine(Fade());
      float time = SpecialEffectsHelper.Instance.Explosion(__Transform.position, new Vector3(2, 2, 1), __Transform.parent);
      StartCoroutine(Delete(time));
      return residue;
    }
    return residue;
  }

  public void ApplyGem(GameBonusType bonusType) {
    DebugLogger.WriteInfo("Beetle.ApplyGem bonusType = {0}", bonusType.ToString());
    switch (bonusType) {
      case GameBonusType.UpgradeGun: {
      } break;
      case GameBonusType.ChildrenShield: {
      } break;
      case GameBonusType.Time: {
        __SleepGameObject(GameManager.Instance.GetPlayer(),
                          (GameManager.Instance.GetGameLevel() as GameLevel).EnemyUpgrade.SleepTime);
      }
      break;
      case GameBonusType.Speed: {
        StartCoroutine(__MultiSpeed(2.0f,
                                    (GameManager.Instance.GetGameLevel() as GameLevel).EnemyUpgrade.SpeedTime));
      }
      break;
      case GameBonusType.Shield: {
        StartCoroutine(__GetShield((GameManager.Instance.GetGameLevel() as
                                    GameLevel).EnemyUpgrade.ShieldTime));
      }
      break;
      case GameBonusType.Bullet: {
        //_gun.AddBullet();
      } break;
      case GameBonusType.Live: {
        Health += Health * (GameManager.Instance.GetGameLevel() as GameLevel).EnemyUpgrade.Health / 100.0f;
      }
      break;
    }
  }

  #region MonoBehaviour Events

  // Use this for initialization
  protected override void Start() {
    base.Start();
    Rigidbody2D rig = GetComponent<Rigidbody2D>();
    rig.constraints = RigidbodyConstraints2D.FreezeRotation;
    _collider2D = GetComponent<BoxCollider2D>();
    OnEnemyCreated += __OnEnemyCreated;
  }

  // Update is called once per frame
  protected override void Update() {
    if (!IsControlable) {
      return;
    }

    if (_bFollowToPlayer) {
      _time += Time.deltaTime;
      if (_time >= _changeDirTime) {
        _time = 0.0f;
        FindTargetByTag(GameLevel.PlayerTagName, true);
        MoveToTarget();
      }
    }

    base.Update();
  }

  protected virtual void OnCollisionEnter2D(Collision2D other) {
    DebugLogger.WriteInfo("OnCollisionEnter2D other.gameObject.tag = {0}", other.gameObject.tag);
    switch (other.gameObject.tag) {
      case GameLevel.BorderTagName: {
        HandleCollisionBorder(other);
      }
      break;
      case GameLevel.EnemyTagName: {
        HandleCollisionEnemy(other);
      }
      break;
      case GameLevel.PlayerTagName: {
        HandleCollisionPlayer(other);
      }
      break;
      case GameLevel.TerrainTagName: {
        HandleCollisionTerrain(other);
      }
      break;
    }
  }

  #endregion

  public Beetle()
  : base() {
    TypeId = (int)GameObjectType.Beetle;
    Health = 5f;
    CreateEnemyCountBySec = 0.4f;
    HitForce = 3f;
    OnMoveDirChanged += _OnMoveDirChanged;
    OnParamChanged += _OnParamChanged;
  }


  protected virtual void HandleCollisionBorder(Collision2D other) {
    DebugLogger.WriteVerbose("other.relativeVelocity = {0}", other.relativeVelocity.ToString());
    if (other.contacts.Length == 0) {
      DebugLogger.WriteWarning("Beetle.HandleCollisionBorder Border::other.contacts.Length == 0");
      return;
    }
    var normal = other.contacts[0].normal;
    if (normal.y.IsEqual(0f)) {
      SetXSignMoveDir();
    }
    else if (normal.x.IsEqual(0f)) {
      SetYSignMoveDir();
    }
  }

  protected virtual void HandleCollisionEnemy(Collision2D other) {
    var enemy = other.gameObject.GetComponent<Enemy>();
    if (enemy == null || enemy.TypeId == (int)GameObjectType.Portal) {
      return;
    }

    DebugLogger.WriteInfo("Beetle.HandleCollisionEnemy enemy.TypeId = {0}", enemy.TypeId);
    if (other.contacts.Length == 0) {
      DebugLogger.WriteWarning("Beetle.HandleCollisionEnemy Enemy::other.contacts.Length == 0");
      return;
    }
    var normal = other.contacts[0].normal;

    var otherMoveDir = enemy.GetMoveDirection();
    var angle_ = Vector2.Angle(GetMoveDirection(), otherMoveDir);

    DebugLogger.WriteVerbose("Beetle.HandleCollisionEnemy Id = {0}", Id);
    DebugLogger.WriteVerbose("Beetle.HandleCollisionEnemy angle_ = {0}; enemy.Speed = {1}; Speed = {2}",
                             angle_, enemy.Speed, Speed);

    if (Mathf.Abs(angle_).IsLess(45.0f)) {
      if (enemy.Speed.IsLess(Speed)) {
        DebugLogger.WriteVerbose("Beetle.HandleCollisionEnemy Id = {0}", Id);
        DebugLogger.WriteVerbose("Beetle.HandleCollisionEnemy normal = {0}", normal.ToString());
        if (normal.y.IsEqual(0f)) {
          SetXSignMoveDir();
        }
        else if (normal.x.IsEqual(0f)) {
          SetYSignMoveDir();
        }
        var speed = Speed;
        Speed = enemy.Speed;
        enemy.Speed = speed;
      }
      return;
    }

    if (normal.y.IsEqual(0f)) {
      SetXSignMoveDir();
    }
    else if (normal.x.IsEqual(0f)) {
      SetYSignMoveDir();
    }
  }

  protected virtual void HandleCollisionPlayer(Collision2D other) {
    _bFollowToPlayer = true;
  }

  protected virtual void HandleCollisionTerrain(Collision2D other) {
    if (other.contacts.Length == 0) {
      DebugLogger.WriteWarning("Beetle.HandleCollisionTerrain Terrain::other.contacts.Length == 0");
      return;
    }
    FindTargetByTag(GameLevel.PlayerTagName, true);
    MoveToTarget();
  }

  [NonSerialized]
  private float _time = 0f;
  [NonSerialized]
  private float _changeDirTime = 0.1f;
  [NonSerialized]
  private bool _bFollowToPlayer;

  private int _count = 0;
  private BoxCollider2D _collider2D;

  private IEnumerator __GetShield(float time) {
    return Helpers.SetShield(this, __Transform, time);
  }

  private void _OnMoveDirChanged(Vector2 moveDir) {
    //DebugLogger.WriteInfo("Beetle::_OnMoveDirChanged moveDir = {0}", moveDir.ToString());
    var angle = EGHelpers.GetDegAngle(moveDir);
    //DebugLogger.WriteInfo("Beetle::_OnMoveDirChanged angle = {0}", angle);
    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
  }

  private void _OnParamChanged(string propName, object oldValue, object newValue) {
    switch (propName) {
      case "Speed": {
        float fval = Convert.ToSingle(newValue);
        CreateEnemyCountBySec = fval / 4.0f;
      }
      break;
    }
  }

  private IEnumerator Delete(float time) {
    yield return new WaitForSeconds(time);
    DebugLogger.WriteInfo("Beetle.Delete IsBonus = {0}", IsBonus);
    if (IsBonus) {
      int type = UnityEngine.Random.Range(0, 7);
      var gem = EGHelpers.CreateAnimationByScript<Gem>(__Transform.position,
                GameManager.Instance.gemBonusAnims[type], "GemBonus" + type.ToString(), __Transform.parent,
                GameLevel.NeutralTagName);
      gem.Kind = (GemType)type;
    }
    Destroy(gameObject);
    _Dead(0);
    _DiedObject = null;
  }

  private IEnumerator Fade() {
    Color c = spriteRender.color;
    for (float i = 0.6f; i > 0.0f; i -= 0.3f) {
      c.a = i;
      spriteRender.color = c;
      yield return new WaitForSeconds(0.05f);
    }
  }

  private void __OnEnemyCreated(IEnemy enemy) {
    DebugLogger.WriteInfo("Beetle.__OnEnemyCreated Level = {0}", Level);
    if (Level == 0) {
      StartCoroutine(__StartEnemy(enemy, Speed + UnityEngine.Random.Range(0.1f, 0.2f), 12, 2.5f));
      return;
    }
    if (Level == 1) {
      StartCoroutine(__StartEnemy(enemy, Speed + UnityEngine.Random.Range(0.2f, 0.3f), 10, 2f));
      return;
    }
  }

  private IEnumerator __StartEnemy(IEnemy enemy, float speedEnd, int count, float time) {
    //enemy.SetColliderTrigger(true);
    float change = enemy.Speed - speedEnd;
    DebugLogger.WriteInfo("Beetle.__StartEnemy change = {0}", change);
    for (int i = 0; i < count; ++i) {
      yield return new WaitForSeconds(time / count);
      enemy.Speed -= (change / count);
      DebugLogger.WriteInfo("Beetle.__StartEnemy enemy.Speed = {0}", enemy.Speed);
    }
    //enemy.SetColliderTrigger(false);
  }
}

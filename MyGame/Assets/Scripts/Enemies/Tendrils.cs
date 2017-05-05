using UnityEngine;
using System.Collections;

using EG;
using EG.Enemy;
using EG.Misc;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Tendrils : Monster {

  public override void SetColliderTrigger(bool bValue) {
    var collider = GetComponent<BoxCollider2D>();
    collider.isTrigger = bValue;
  }

  #region MonoBehaviour Events

  // Use this for initialization
  protected override void Start() {
    base.Start();

    if (Level == 1) {
      spriteRender.color = new Color(1.0f, 0.4f, 0.8f);
      __Transform.localScale = new Vector3(1.2f, 1.2f, 1f);
    }
  }

  // Update is called once per frame
  void Update() {
    if (!IsControlable) {
      return;
    }

    if (_bFollowToPlayer || Level == 1) {
      _time += Time.deltaTime;
      if (_time >= _changeDirTime) {
        _time = 0.0f;
        FindTargetByTag(GameLevel.PlayerTagName, true);
        MoveToTarget();
      }
    }
    Move();
  }

  #endregion

  public override int Level {
    get { return base.Level; }
    set {
      base.Level = value;
      if (base.Level == 1) {
        TypeId = (int)GameObjectType.IntelligentTendrils;
      }
    }
  }

  public Tendrils() : base() {
    TypeId = (int)GameObjectType.Tendrils;
    Health = 1.5f;
    HitForce = 1f;
  }

  protected override void HandleCollisionEnemy(Collision2D other) {
    if (Level == 0) {
      base.HandleCollisionEnemy(other);
      return;
    }

    var enemy = other.gameObject.GetComponent<Enemy>();
    if (enemy == null || enemy.TypeId == (int)GameObjectType.Portal) {
      return;
    }

    if (other.contacts.Length == 0) {
      DebugLogger.WriteWarning("IntelligentTendrils.OnCollisionEnter2D Enemy::other.contacts.Length == 0");
      return;
    }
    var normal = other.contacts[0].normal;

    var otherMoveDir = enemy.GetMoveDirection();
    var angle_ = Vector2.Angle(GetMoveDirection(), otherMoveDir);

    DebugLogger.WriteVerbose("IntelligentTendrils.OnCollisionEnter2D Id = {0}", Id);
    DebugLogger.WriteVerbose("IntelligentTendrils.OnCollisionEnter2D angle_ = {0}; enemy.Speed = {1}; Speed = {2}",
                             angle_, enemy.Speed, Speed);

    if (Mathf.Abs(angle_).IsLess(45.0f)) {
      if (enemy.Speed.IsLess(Speed)) {
        DebugLogger.WriteVerbose("IntelligentTendrils.OnCollisionEnter2D Id = {0}", Id);
        DebugLogger.WriteVerbose("IntelligentTendrils.OnCollisionEnter2D normal = {0}", normal.ToString());
        var speed = Speed;
        Speed = enemy.Speed;
        enemy.Speed = speed;
      }
    }
  }

  protected override void HandleCollisionPlayer(Collision2D other) {
    _bFollowToPlayer = true;
    if (Level == 1) {
      _changeDirTime = 0.1f;
    }
  }

  private float _time = 0f;
  private bool _bFollowToPlayer;
  private float _changeDirTime = 0.15f;
}

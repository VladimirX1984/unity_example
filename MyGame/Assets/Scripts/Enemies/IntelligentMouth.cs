using UnityEngine;

using EG;
using EG.Enemy;
using EG.Misc;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class IntelligentMouth : Monster {

  #region MonoBehaviour Events

  protected override void Start() {
    base.Start();
    spriteRender.color = new Color(1.0f, 0.4f, 0.8f);
    __Transform.localScale = new Vector3(1.2f, 1.2f, 1f);
    var circleCollider2D = GetComponent<CircleCollider2D>();
    circleCollider2D.radius = 0.11f;
  }

  // Update is called once per frame
  void Update() {
    if (!IsControlable) {
      return;
    }

    _time += Time.deltaTime;
    if (_time >= _changeDirTime) {
      _time = 0.0f;
      FindTargetByTag(GameLevel.PlayerTagName, true);
      MoveToTarget();
    }
    Move();
  }

  #endregion

  public IntelligentMouth()
  : base() {
    TypeId = (int)GameObjectType.IntelligentMouth;
    Health = 3f;
    HitForce = 1.5f;
  }

  protected override void HandleCollisionEnemy(Collision2D other) {
    var enemy = other.gameObject.GetComponent<Enemy>();
    if (enemy == null || enemy.TypeId == (int)GameObjectType.Portal) {
      return;
    }

    if (other.contacts.Length == 0) {
      DebugLogger.WriteWarning("IntelligentMouth.OnCollisionEnter2D Enemy::other.contacts.Length == 0");
      return;
    }
    var normal = other.contacts[0].normal;

    var otherMoveDir = enemy.GetMoveDirection();
    var angle_ = Vector2.Angle(GetMoveDirection(), otherMoveDir);

    DebugLogger.WriteVerbose("IntelligentMouth.OnCollisionEnter2D Id = {0}", Id);
    DebugLogger.WriteVerbose("IntelligentMouth.OnCollisionEnter2D angle_ = {0}; enemy.Speed = {1}; Speed = {2}",
                             angle_, enemy.Speed, Speed);

    if (Mathf.Abs(angle_).IsLess(45.0f)) {
      if (enemy.Speed.IsLess(Speed)) {
        DebugLogger.WriteVerbose("IntelligentMouth.OnCollisionEnter2D Id = {0}", Id);
        DebugLogger.WriteVerbose("IntelligentMouth.OnCollisionEnter2D normal = {0}", normal.ToString());
        var speed = Speed;
        Speed = enemy.Speed;
        enemy.Speed = speed;
      }
    }
  }

  protected override void HandleCollisionPlayer(Collision2D other) {
    _changeDirTime = 0.1f;
  }

  private float _time = 0f;
  private float _changeDirTime = 0.12f;
}

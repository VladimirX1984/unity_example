using UnityEngine;

using System.Collections;

using EG;
using EG.Enemy;
using EG.Misc;

[RequireComponent(typeof(Rigidbody2D))]
public class Monster : Enemy {

  public override float Hit(float damage) {
    float residue = base.Hit(damage);
    if (!IsAlive) {
      IsControlable = false;
      Color c = spriteRender.color;
      c.a = 0.65f;
      spriteRender.color = c;
      StartCoroutine(Fade());
      float time = SpecialEffectsHelper.Instance.Explosion(__Transform.position, __Transform.parent);
      StartCoroutine(Delete(time));
      return residue;
    }
    return residue;
  }

  public void ApplyGem(GameBonusType bonusType) {
    DebugLogger.WriteInfo("Monster.ApplyGem bonusType = {0}", bonusType.ToString());
    switch (bonusType) {
      case GameBonusType.UpgradeGun: {
        //_gun.Upgrade();
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
        StartCoroutine(__SetShield((GameManager.Instance.GetGameLevel() as
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
  }

  protected virtual void OnCollisionEnter2D(Collision2D other) {
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

  protected virtual void HandleCollisionBorder(Collision2D other) {
    DebugLogger.WriteVerbose("other.relativeVelocity = {0}", other.relativeVelocity.ToString());
    if (other.contacts.Length == 0) {
      DebugLogger.WriteWarning("Monster.HandleCollisionBorder Border::other.contacts.Length == 0");
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

    if (other.contacts.Length == 0) {
      DebugLogger.WriteWarning("Monster.HandleCollisionEnemy Enemy::other.contacts.Length == 0");
      return;
    }
    var normal = other.contacts[0].normal;

    var otherMoveDir = enemy.GetMoveDirection();
    var angle_ = Vector2.Angle(GetMoveDirection(), otherMoveDir);

    DebugLogger.WriteVerbose("Monster.HandleCollisionEnemy Id = {0}", Id);
    DebugLogger.WriteVerbose("Monster.HandleCollisionEnemy angle_ = {0}; enemy.Speed = {1}; Speed = {2}",
                             angle_, enemy.Speed, Speed);

    if (Mathf.Abs(angle_).IsLess(45.0f)) {
      if (enemy.Speed.IsLess(Speed)) {
        DebugLogger.WriteVerbose("Monster.HandleCollisionEnemy Id = {0}", Id);
        DebugLogger.WriteVerbose("Monster.HandleCollisionEnemy normal = {0}", normal.ToString());
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

  }

  protected virtual void HandleCollisionTerrain(Collision2D other) {
    if (other.contacts.Length == 0) {
      DebugLogger.WriteWarning("Monster.HandleCollisionTerrain Terrain::other.contacts.Length == 0");
      return;
    }
    FindTargetByTag(GameLevel.PlayerTagName, true);
    MoveToTarget();
  }

  private IEnumerator Delete(float time) {
    yield return new WaitForSeconds(time);
    DebugLogger.WriteInfo("Monster.Delete IsBonus = {0}", IsBonus);
    if (IsBonus) {
      int type = Random.Range(0, 7);
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

  protected IEnumerator __SetShield(float time) {
    return Helpers.SetShield(this, __Transform, time);
  }
}

using UnityEngine;

using EG.Neutral;
using EG.Enemy;

public enum GemType {
  Yellow = 0,
  Gray = 1,
  Blue = 2,
  Red = 3,
  Purple = 4,
  Orange = 5,
  Green = 6
}

public enum GameBonusType {
  UpgradeGun = GemType.Yellow,
  ChildrenShield = GemType.Gray,
  Time = GemType.Blue,
  Speed = GemType.Red,
  Shield = GemType.Purple,
  Bullet = GemType.Orange,
  Live = GemType.Green       // Player
}

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Gem : NeutralObject {

  public GemType Kind {
    get { return _kind; }
    set {
      _kind = value;
      if (_kind == GemType.Gray) {
        int bonusType = Random.Range(0, 7);
        if (bonusType < 3) {
          BonusType = GameBonusType.ChildrenShield;
          return;
        }
        bonusType = Random.Range(0, 7);
        BonusType = (GameBonusType)bonusType;
        return;
      }
      BonusType = (GameBonusType)(int)_kind;
    }
  }
  private GemType _kind;

  public GameBonusType BonusType { get; private set; }

  public override float Hit(float damage) {
    float residue = base.Hit(damage);
    if (Health <= 0.0f) {
      Destroy(gameObject);
      _Dead(0);
      return residue;
    }
    return residue;
  }

  #region MonoBehaviour Events

  // Use this for initialization
  protected override void Start() {
    base.Start();
    TypeId = (int)GameObjectType.Bonus;
    Health = 1.0f;
    var collider = GetComponent<BoxCollider2D>();
    collider.isTrigger = true;
    collider.size = new Vector2(0.4f, 0.4f);
    maxTime = 30f;
  }

  void Update() {
    if (time >= maxTime) {
      Hit(1.0f);
    }
    time += Time.deltaTime;
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (!IsAlive) {
      return;
    }
    switch (other.gameObject.tag) {
      case GameLevel.BorderTagName: {
        } break;
      case GameLevel.BulletTagName: {
        } break;
      case GameLevel.EnemyTagName: {
          var enemy = other.gameObject.GetComponent<Enemy>();
          if (enemy != null) {
            if (enemy is Monster) {
              (enemy as Monster).ApplyGem(BonusType);
            }
            else if (enemy is Beetle) {
              (enemy as Beetle).ApplyGem(BonusType);
            }
          }
        }
        break;
      case GameLevel.PlayerTagName: {
          var player = other.gameObject.GetComponent<Skelsp>();
          if (player != null) {
            player.ApplyGem(BonusType);
          }
          Hit(1.0f);
        }
        break;
      case GameLevel.TerrainTagName: {
        } break;
    }
  }

  #endregion

  private float time;
  private float maxTime;
}


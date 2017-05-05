using UnityEngine;

using EG;
using EG.Enemy;
using EG.Misc;

public class Bullet : BaseShell {

  public void Init(Vector2 pos, float speed, float strength, Vector2 direction, float scale = 1.0f,
                   float acceleration = 0.0f) {
    DebugLogger.WriteVerbose("Bullet.Init pos = {0}", pos.ToString());
    transform.localPosition = pos;
    transform.localScale = new Vector3(scale, scale, 1f);
    SetMoveDirection(direction.normalized);
    Speed = speed;
    Acceleration = acceleration;
    HitForce = strength;
  }

  protected override void _Dead(int reason) {
    base._Dead(reason);
    Destroy(gameObject);
  }

  //void Awake() {
  //
  //}

  #region MonoBehaviour Events

  // Use this for initialization
  protected override void Start() {
    base.Start();
    TypeId = (int)GameObjectType.Bullet;
    CircleCollider2D collider2D = GetComponent<CircleCollider2D>();
    collider2D.isTrigger = true;
  }

  // Update is called once per frame
  void Update() {
    Move();
    Rect field = (GameManager.Instance.GetGameLevel() as GameLevel).Field;
    if (__Transform.localPosition.x > field.xMax || __Transform.localPosition.x < field.xMin ||
        __Transform.localPosition.y > field.yMax || __Transform.localPosition.y < field.yMin) {
      _Dead(1);
    }
  }

  #endregion

  public Bullet() : base() {
    HitForce = 1.0f;
  }

  protected override void HandleTriggerBorder(Collider2D other) {

  }

  protected override void HandleTriggerEnemy(Collider2D other) {
    var enemy = other.gameObject.GetComponent<Enemy>();
    if (enemy != null && enemy.IsAlive) {
      float residue = enemy.Hit(_ammoStrength);
      if (Speed.IsEqual(0.0f) || enemy.IsAlive || residue.IsGreater(0.0f)) {
        _Dead(0);
        return;
      }
      HitForce = Mathf.Abs(residue);
    }
  }

  protected override void HandleTriggerPlayer(Collider2D other) {

  }

  protected override void HandleTriggerTerrain(Collider2D other) {
    Tree tree = other.gameObject.GetComponent<Tree>();
    if (tree != null) {
      tree.Hit(_ammoStrength);
      _Dead(0);
    }
  }

  private float _ammoStrength { get { return HitForce; } }
}

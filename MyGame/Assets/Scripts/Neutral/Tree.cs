using UnityEngine;
using System.Collections;

using EG.Neutral;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Tree : NeutralObject {

  public override float Hit(float damage) {
    float residue = base.Hit(damage);
    if (Health <= 0.0f) {
      Destroy(gameObject);
      _Dead(0);
      return residue;
    }
    return residue;
  }

  // Use this for initialization
  protected override void Start() {
    base.Start();
    TypeId = (int)GameObjectType.Tree;
    __Transform.localScale = new Vector3(3, 3, 2);
    Health = 20.0f;
    Rigidbody2D rig = GetComponent<Rigidbody2D>();
    rig.constraints = RigidbodyConstraints2D.FreezeRotation;
    rig.mass = 10000;
    rig.angularDrag = 0;
    rig.isKinematic = false;
    //rig.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
  }
}

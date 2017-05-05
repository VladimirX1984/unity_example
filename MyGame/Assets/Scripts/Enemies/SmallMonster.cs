using UnityEngine;
using EG;

[RequireComponent(typeof(Rigidbody2D))]
public class SmallMonster : Monster {

  public override void SetColliderTrigger(bool bValue) {
    var collider = GetComponent<BoxCollider2D>();
    collider.isTrigger = bValue;
  }

  #region MonoBehaviour Events

  // Use this for initialization
  protected override void Start() {
    base.Start();

    __Transform.localScale = new Vector3(1.1f, 1.1f, 1);
  }

  // Update is called once per frame
  void Update() {
    if (!IsControlable) {
      return;
    }

    Move();
  }

  #endregion

  public SmallMonster() : base() {
    TypeId = (int)GameObjectType.SmallMonster;
    Health = 0.5f;
    HitForce = 0.75f;
  }
}

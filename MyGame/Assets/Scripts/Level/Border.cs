using UnityEngine;
using System.Collections;

using EG.Objects;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Border : BaseMonoObject {

  // Use this for initialization
  protected override void Start() {
    base.Start();
    var rigid = GetComponent<Rigidbody2D>();
    rigid.mass = 100000;
    rigid.gravityScale = 0;
    rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
  }

  // Update is called once per frame
  void Update() {

  }
}

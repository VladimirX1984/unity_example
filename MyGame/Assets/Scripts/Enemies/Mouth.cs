using UnityEngine;
using System.Collections;

using EG;
using EG.Enemy;
using EG.Misc;

[RequireComponent(typeof(Rigidbody2D))]
public class Mouth : Monster {

  #region MonoBehaviour Events

  // Update is called once per frame
  void Update() {
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

    Move();
  }

  #endregion

  public Mouth()
  : base() {
    Health = 3f;
    TypeId = (int)GameObjectType.Mouth;
    HitForce = 1.5f;
  }

  protected override void HandleCollisionPlayer(Collision2D other) {
    _bFollowToPlayer = true;
  }

  private float _time = 0f;
  private float _changeDirTime = 0.1f;
  private bool _bFollowToPlayer;
}

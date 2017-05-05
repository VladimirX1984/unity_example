using UnityEngine;

using EG;
using EG.Objects;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class BaseShell : BaseGameObject {

  #region MonoBehaviour Events

  // Use this for initialization
  protected override void Start() {
    base.Start();
    Rigidbody2D rig = GetComponent<Rigidbody2D>();
    rig.constraints = RigidbodyConstraints2D.FreezeRotation;
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (__isInit) {
      return;
    }
    DebugLogger.WriteVerbose("Bullet.OnTriggerEnter2D other.gameObject.tag = {0}",
                             other.gameObject.tag);
    switch (other.gameObject.tag) {
      case GameLevel.BorderTagName: {
        HandleTriggerBorder(other);
      } break;
      case GameLevel.EnemyTagName: {
        HandleTriggerEnemy(other);
      } break;
      case GameLevel.PlayerTagName: {
        HandleTriggerPlayer(other);
      } break;
      case GameLevel.TerrainTagName: {
        HandleTriggerTerrain(other);
      } break;
    }
  }

  #endregion

  protected virtual void HandleTriggerBorder(Collider2D other) {

  }

  protected virtual void HandleTriggerEnemy(Collider2D other) {

  }

  protected virtual void HandleTriggerPlayer(Collider2D other) {

  }

  protected virtual void HandleTriggerTerrain(Collider2D other) {

  }
}

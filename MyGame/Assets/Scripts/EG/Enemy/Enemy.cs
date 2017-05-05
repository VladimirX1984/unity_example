using UnityEngine;

using EG.Objects;

namespace EG.Enemy {
  [RequireComponent(typeof(SpriteRenderer))]
  public abstract class Enemy : BaseGameObject, IEnemy {

    protected SpriteRenderer spriteRender;

    #region MonoBehaviour Events

    protected override void Start() {
      base.Start();
      spriteRender = GetComponent<SpriteRenderer>();
    }

    #endregion

    protected void FindTarget(string targetName, bool always = true) {
      if (!always && _target != null) {
        return;
      }
      var go = GameObject.Find(targetName);
      _target = go != null ? go.transform : null;
    }

    protected void FindTargetByTag(string tagName, bool always = true) {
      if (!always && _target != null) {
        return;
      }
      var go = GameObject.FindGameObjectWithTag(tagName);
      _target = go != null ? go.transform : null;
    }

    protected void MoveToTarget() {
      if (_target == null) {
        DebugLogger.WriteError("Enemy _target is undefined");
        return;
      }
      Vector3 dirToTarget = _target.position - __Transform.position;
      float distance = dirToTarget.sqrMagnitude;
      //Меняем направление только если начали удаляться от цели
      if (_lastDistance > distance) {
        _lastDistance = distance;
        return;
      }
      SetMoveDirection(dirToTarget.normalized);
      _lastDistance = distance;
    }

    private Transform _target;
    private float _lastDistance;
  }
}
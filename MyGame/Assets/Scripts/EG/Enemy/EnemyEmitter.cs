using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace EG.Enemy {
  public abstract class EnemyEmitter : Enemy, IEnemyEmitter {

    #region Реализация интерфейса IEnemyEmitter

    public float CreateEnemyCountBySec { get; set; }

    #endregion

    #region MonoBehaviour Events

    // Use this for initialization
    protected override void Start() {
      base.Start();
    }

    protected virtual void Update() {
      if (!IsControlable) {
        return;
      }
      _time += Time.deltaTime;
      if (_time >= 1f / CreateEnemyCountBySec) {
        _time = 0f;
        CreateEnemy();
      }
      Move();
    }

    #endregion

    public override float Hit(float damage) {
      float residue = base.Hit(damage);
      if (!IsAlive) {
        _Dead(0);
      }
      return residue;
    }

    public event UnityAction<IEnemy> OnEnemyCreated;

    public EnemyEmitter() {
      CreateEnemyCountBySec = 1f;
    }

    protected abstract void CreateEnemy();

    protected IEnumerator OnCreateEnemy(IEnemy enemy) {
      yield return new WaitForEndOfFrame();
      if (OnEnemyCreated != null) {
        OnEnemyCreated(enemy);
      }
    }

    [NonSerialized]
    private float _time = 0f;
  }
}

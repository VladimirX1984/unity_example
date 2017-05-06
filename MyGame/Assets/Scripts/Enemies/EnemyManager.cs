using UnityEngine;
using UnityEngine.Events;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using EG;
using EG.Enemy;
using EG.Model;
using EG.Objects;
using EG.Players;

public class EnemyManager : BaseMonoManager {

  public void Init(IPlayer player) {
    _player = player;
  }

  public void AddEnemy(IEnemy enemy) {
    int number = GameManager.Instance.GetGameLevel().Number;
    if (enemy.TypeId == (int)GameObjectType.Portal) {
      enemy.IsBonus = number < 3 || (Random.Range(0, 19 + number) >= 7 + number);
      var portal = enemy as Portal;
      portal.OnEnemyCreated += OnEnemyCreated;
    }
    else if (enemy.TypeId == (int)GameObjectType.Beetle) {
      enemy.IsBonus = number < 3 || (Random.Range(0, 19 + number) >= 7 + number);
      var beetle = enemy as Beetle;
      beetle.OnEnemyCreated += OnEnemyCreated;
    }
    else {
      /*int r = Random.Range(0, 39 + number);
        DebugLogger.WriteInfo("EnemyManager.AddEnemy enemy.Id = {0}; r = {1}",
                            enemy.Id, r);*/
      enemy.IsBonus = (Random.Range(0, 39 + number) >= 23 + number);
    }
    //DebugLogger.WriteInfo("EnemyManager.AddEnemy enemy.Id = {0}; enemy.IsBonus = {1}; number = {2}",
      //                  enemy.Id, enemy.IsBonus ? 1 : 0, number);
    _enemyList.Add(enemy);
    _enemyIdList.Add(enemy.Id);
    enemy.AddObserver(_DiedObject);
  }

  public void SleepEnemies(float time) {
    foreach (IEnemy enemy in _enemyList) {
      enemy.IsControlable = false;
    }
    StartCoroutine(__AvakeEnemies(time));
  }

  public void Clear() {
    _enemyList.Clear();
    _enemyIdList.Clear();
  }

  public event UnityAction OnEnemyZero;

  public EnemyManager()
  : base("EnemyManager") {
    _enemyList = new List<IEnemy>();
    _enemyIdList = new List<float>();
  }

  private List<IEnemy> _enemyList;
  private List<float> _enemyIdList;
  private IPlayer _player;

  private void OnEnemyCreated(IEnemy enemy) {
    AddEnemy(enemy);
  }

  private IEnumerator __AvakeEnemies(float time) {
    yield return new WaitForSeconds(time);
    foreach (IEnemy enemy in _enemyList) {
      enemy.IsControlable = true;
    }
  }

  private void _DiedObject(IGameObject obj, int reason) {
    DebugLogger.WriteInfo("EnemyManager._DeadObject obj.Id = {0}; reason = {1}; _enemyIdList.Count = {2}",
                          obj.Id, reason, _enemyIdList.Count);
    _enemyList.Remove(obj as IEnemy);
    _enemyIdList.Remove(obj.Id);
    if (reason == 0) {
      (_player as Skelsp).KillEnemy(obj as IEnemy);
    }

    if (_enemyIdList.Any()) {
      return;
    }

    _enemyList.Clear();

    if (OnEnemyZero != null) {
      OnEnemyZero();
    }
  }
}

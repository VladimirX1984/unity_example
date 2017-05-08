using UnityEngine;

using System.Collections;

using EG;
using EG.Enemy;
using EG.Objects;

[RequireComponent(typeof(Rigidbody2D))]
public class Portal : EnemyEmitter {

  #region Animator

  public RuntimeAnimatorController animatorMouth;

  #endregion

  public override int Level {
    get { return base.Level; }
    set {
      base.Level = value;
      HitForce = 2f + 0.25f * base.Level;
      Health = 10f + 1.5f * base.Level;
    }
  }

  #region Реализация абстрактного класса

  protected override void CreateEnemy() {
    ++_count;

    if (Level == 0) {
      var enemy = EGHelpers.CreateObjectByPrefab<Monster>(__Transform.localPosition,
                                                          GameManager.Instance.GetMonoGameLevel().prefabSmallMonster, __Transform.parent);
      StartCoroutine(OnCreateEnemy(enemy));
      enemy.Speed = Random.Range(0.3f, 0.5f);
      enemy.SetMoveDirection(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
      return;
    }

    if (Level == 1) {
      var enemy = EGHelpers.CreateObjectByPrefab<Monster>(__Transform.localPosition,
                                                          GameManager.Instance.GetMonoGameLevel().prefabTendrils, __Transform.parent);
      StartCoroutine(OnCreateEnemy(enemy));
      enemy.Level = _count % 3 == 0 ? 1 : 0;
      enemy.Speed = Random.Range(0.4f, 0.6f);
      enemy.SetMoveDirection(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
      return;
    }

    if (Level == 2) {
      if (_count % 3 == 0) {
        var intMouth = EGHelpers.CreateAnimationByScript<IntelligentMouth>(__Transform.localPosition,
                       animatorMouth, "IntelligentMouth", __Transform.parent, GameLevel.EnemyTagName);
        StartCoroutine(OnCreateEnemy(intMouth));
        intMouth.Speed = Random.Range(0.4f, 0.5f);
        return;
      }

      var enemy = EGHelpers.CreateObjectByPrefab<Monster>(__Transform.localPosition,
                                                          GameManager.Instance.GetMonoGameLevel().prefabMouth, __Transform.parent);
      StartCoroutine(OnCreateEnemy(enemy));
      enemy.Speed = Random.Range(0.3f, 0.5f);
      enemy.SetMoveDirection(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
      return;
    }

    if (Level == 3) {
      if (_count % 3 == 0) {
        var intMouth = EGHelpers.CreateAnimationByScript<IntelligentMouth>(__Transform.localPosition,
                       animatorMouth, "IntelligentMouth", __Transform.parent, GameLevel.EnemyTagName);
        StartCoroutine(OnCreateEnemy(intMouth));
        intMouth.Speed = Random.Range(0.4f, 0.6f);
        return;
      }

      var enemy = EGHelpers.CreateObjectByPrefab<Monster>(__Transform.localPosition,
                                                          GameManager.Instance.GetMonoGameLevel().prefabMouth, __Transform.parent);
      StartCoroutine(OnCreateEnemy(enemy));
      enemy.Speed = Random.Range(0.4f, 0.7f);
      enemy.SetMoveDirection(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
      return;
    }

    if (Level == 4) {
      var enemy = EGHelpers.CreateObjectByPrefab<Beetle>(__Transform.localPosition,
                                                         GameManager.Instance.GetMonoGameLevel().prefabBeetle, __Transform.parent);
      StartCoroutine(OnCreateEnemy(enemy));
      enemy.Speed = Random.Range(0.3f, 0.5f);
      enemy.SetMoveDirection(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
      return;
    }

    if (Level == 5) {
      var enemy = EGHelpers.CreateObjectByPrefab<Beetle>(__Transform.localPosition,
                                                         GameManager.Instance.GetMonoGameLevel().prefabBeetle, __Transform.parent);
      StartCoroutine(OnCreateEnemy(enemy));
      enemy.Level = 1;
      enemy.Speed = Random.Range(0.3f, 0.5f);
      enemy.SetMoveDirection(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
      return;
    }
  }

  #endregion

  protected override void _Dead(int reason) {
    if (IsBonus) {
      int type = Random.Range(0, 7);
      var gem = EGHelpers.CreateAnimationByScript<Gem>(__Transform.position,
                                                       GameManager.Instance.gemBonusAnims[type], "GemBonus" + type.ToString(), __Transform.parent,
                                                       GameLevel.NeutralTagName);
      gem.Kind = (GemType)type;
    }
    Destroy(gameObject);
    base._Dead(reason);
    _DiedObject = null;
  }

  public Portal() : base() {
    TypeId = (int)GameObjectType.Portal;
    CreateEnemyCountBySec = 0.25f;
    Health = 10f;
    HitForce = 2f;
  }

  #region MonoBehaviour Events

  // Use this for initialization
  protected override void Start() {
    base.Start();
    _collider2D = GetComponent<CircleCollider2D>();
    _collider2D.isTrigger = true;
    __Transform.localScale = new Vector3(2.6f + 0.2f * Level, 2.6f + 0.2f * Level, 2.6f + 0.2f * Level);
    Rigidbody2D rig = GetComponent<Rigidbody2D>();
    rig.constraints = RigidbodyConstraints2D.FreezeRotation;
  }

  #endregion

  private int _count = 0;

  private CircleCollider2D _collider2D;
}

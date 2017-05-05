using UnityEngine;
using EG;

class SpecialEffectsHelper : UnitySingleton<SpecialEffectsHelper> {

  public ParticleSystem fireEffect;

  #region MonoBehaviour Events

  void Awake() {

  }

  void Update() {

  }

  #endregion

  public float Explosion(Vector3 position, Transform parent) {
    var ps = Create(fireEffect, position, parent);
    return ps.startLifetime;
  }

  public float Explosion(Vector3 position, Vector3 scale, Transform parent) {
    var ps = Create(fireEffect, position, scale, parent);
    return ps.startLifetime;
  }

  private ParticleSystem Create(ParticleSystem prefab, Vector3 position, Transform parent) {
    var newPS = GameObject.Instantiate(prefab);
    newPS.transform.position = position;
    newPS.transform.parent = parent;
    Destroy(newPS.gameObject, newPS.startLifetime);
    return newPS;
  }

  private ParticleSystem Create(ParticleSystem prefab, Vector3 position, Vector3 scale, Transform parent) {
    var newPS = GameObject.Instantiate(prefab);
    newPS.transform.position = position;
    newPS.transform.localScale = scale;
    newPS.transform.parent = parent;
    Destroy(newPS.gameObject, newPS.startLifetime);
    return newPS;
  }
}
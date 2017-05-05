using UnityEngine;
using System;
using System.Collections;

using EG;

public class Gun : MonoBehaviour {

  public enum Weapons {
    Pistol,             // пистолет
    Rifle,              // винтовка
    AssaultRifle,       // штурмовая винтовка
    ChainGun,
    AssaultRifle2,
    Grenada,
    PlasmaGun
  }

  public Projectile[] projectiles = new Projectile[7];

  public volatile Weapons carringWeapo;

  public volatile int icarringWeapo;

  public volatile int maxCarringWeapo;

  public int Direction {
    get { return _direction; }
    private set { _direction = value; }
  }
  private int _direction;

  public bool IsMakeShot {
    get { return _bMakeShot; }
  }
  private bool _bMakeShot;

  public bool IsBurst {
    get {
      return carringWeapo == Weapons.AssaultRifle || carringWeapo == Weapons.AssaultRifle2
             || carringWeapo == Weapons.PlasmaGun;
    }
  }

  public int Ammo {
    get { return projectiles[(int)carringWeapo].ammo; }
  }

  public void Upgrade() {
    if (maxCarringWeapo < typeCount - 1) {
      ++maxCarringWeapo;
      DebugLogger.WriteInfo("Gun.Upgrade maxCarringWeapo = {0}", maxCarringWeapo);
    }
    /*else {
      var projectile = projectiles[(int)carringWeapo];
      projectile.ammo = projectile.maxAmmo;
      if (OnAmmoChanged != null) {
        OnAmmoChanged(projectile.ammo);
      }
    }*/
  }

  public void SetCarringWeapo(Weapons weapo) {
    if ((int)weapo <= maxCarringWeapo) {
      carringWeapo = weapo;
      var projectile = projectiles[(int)carringWeapo];
      if (OnCarringWeapoChanged != null) {
        OnCarringWeapoChanged(carringWeapo);
      }
      if (OnAmmoChanged != null) {
        OnAmmoChanged(projectile.ammo);
      }
    }
  }

  public void AddBullet() {
    var projectile = projectiles[(int)carringWeapo];
    projectile.ammo += (int)UnityEngine.Mathf.Abs(0.25f * projectile.maxAmmo);
    if (projectile.ammo > projectile.maxAmmo) {
      projectile.ammo = projectile.maxAmmo;
    }
    if (OnAmmoChanged != null) {
      OnAmmoChanged(projectile.ammo);
    }
  }

  public void RemoveBullet() {
    var projectile = projectiles[(int)carringWeapo];
    projectile.ammo -= (int)UnityEngine.Mathf.Abs(0.2f * projectile.maxAmmo);
    if (projectile.ammo < 0) {
      projectile.ammo = 0;
    }
    if (OnAmmoChanged != null) {
      OnAmmoChanged(projectile.ammo);
    }
  }

  public void StartFire() {
    if (IsBurst) {
      __StartFire();
      return;
    }
    if (_waitStartFire == null) {
      _waitStartFire = StartCoroutine(__WaitStartFire());
    }
  }

  public void StopFire() {
    _bMakeShot = false;
    if (_waitStartFire != null) {
      StopCoroutine(_waitStartFire);
      _waitStartFire = null;
    }
  }

  public void Init(Transform transform) {
    _skelspTransform = transform;
    typeCount = Enum.GetNames(typeof(Weapons)).Length;
  }

  public void SetDirection(int direction) {
    _direction = direction;
  }

  public void MakeShot(Vector2 flyDirection) {
    _flyDirection = flyDirection;
    _bMakeShot = true;
  }

  public event Action<int> OnAmmoChanged;
  public event Action<Weapons> OnCarringWeapoChanged;

  #region MonoBehaviour Events

  /*void Start() {

    }*/

  #endregion

  private Transform _skelspTransform;

  private int typeCount = 0;

  private int _count;

  private Vector2[] _moves = new Vector2[8] {
    Vector2.right, Vector2.up + Vector2.right, Vector2.up, Vector2.up + Vector2.left,
    Vector2.left, Vector2.down + Vector2.left, Vector2.down, Vector2.down + Vector2.right
  };

  private bool Shot(Projectile bulletProj) {
    _bMakeShot = false;
    int icarringWeapo = (int)carringWeapo;
    if (bulletProj.ammo <= 0) {
      DebugLogger.WriteInfo("Gun.Shot bullet.ammo <= 0 icarringWeapo = {0}", icarringWeapo);
      if (icarringWeapo <= 0) {
        icarringWeapo = maxCarringWeapo + 1;
      }
      while (icarringWeapo > 0) {
        --icarringWeapo;
        if (projectiles[icarringWeapo].ammo > 0) {
          carringWeapo = (Weapons)icarringWeapo;
          if (OnCarringWeapoChanged != null) {
            OnCarringWeapoChanged(carringWeapo);
          }
          break;
        }
        if (icarringWeapo <= 0) {
          return false;
        }
      }
    }

    if (bulletProj.burst) {
      ++_count;
      if ((_count % 5) != 0) {
        return true;
      }
      if (_count > 19) {
        _count = 0;
        return true;
      }
    }

    Vector2 vDirection = _flyDirection;
    if (_flyDirection == Vector2.zero) {
      vDirection = _moves[_direction];
    }
    DebugLogger.WriteVerbose("Gun.Shot vDirection = {0}", vDirection.ToString());
    float coef = vDirection.y < 0f ? 0.1f : 0.15f;
    var pos = (Vector2)_skelspTransform.localPosition + coef * vDirection;
    Bullet bul = EG.EGHelpers.CreateObjectByPrefab<Bullet>(pos, bulletProj.prefab);

    bul.Init(pos, bulletProj.speed, bulletProj.strength, vDirection, bulletProj.scale,
             bulletProj.acceleration);
    --bulletProj.ammo;
    if (OnAmmoChanged != null) {
      OnAmmoChanged(bulletProj.ammo);
    }

    return true;
  }

  private bool __StartFire() {
    //DebugLogger.WriteVerbose("Gun.__StartFire _bMakeShot = {0}", _bMakeShot);
    if (!_bMakeShot) {
      return false;
    }

    if (!Shot(projectiles[(int)carringWeapo])) {
      DebugLogger.WriteInfo("Gun.__StartFire no bullet");
      return false;
    }
    return true;
  }

  private IEnumerator __WaitStartFire() {
    while (true) {
      yield return new WaitForSeconds(0.1f);
      if (!IsBurst) {
        if (__StartFire()) {
          yield return new WaitForSeconds(0.7f);
        }
      }
      else {
        _bMakeShot = false;
      }
    }
  }

  /*private IEnumerator WaitForTime() {
    while (_bMakeShot && !IsBurst) {
      yield return new WaitForSeconds(1f);
      if (_keyCount > 10 && _bMakeShot) {
        StopFire();
      }
    }
    }*/

  private Vector2 _flyDirection;

  private Coroutine _waitStartFire;
}

[Serializable]
public class Projectile {
  public GameObject prefab;
  public float speed = 5.0f;
  public float strength = 0.5f;
  public int ammo = 200;
  public bool whizbang = false;
  public bool burst = false;
  public float scale = 1f;
  public int maxAmmo = 200;
  public float acceleration = 0.0f;
}

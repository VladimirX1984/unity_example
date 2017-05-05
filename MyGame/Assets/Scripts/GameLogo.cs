using UnityEngine;
using System;
using System.Collections;

using EG;
using EG.InputTouch;
using EG.Objects;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class GameLogo : BaseMonoObject, IInputCatcher {

  #region Реализация интерфейса IInputCatcher

  public void WasHitted(Touch3D touch) {

    DebugLogger.WriteInfo("GameLogo.WasHitted touch.TouchLastFrame = {0}",
                          touch.TouchLastFrame.ToString());
    if (touch.TouchLastFrame) {
      return;
    }
    AnimEnded();
    StopCoroutine(_animCoroutine);
  }

  #endregion

  public event Action OnAnimEnded;

  // Use this for initialization
  protected override void Start() {
    base.Start();
    _spriteRender = GetComponent<SpriteRenderer>();
    _animCoroutine = StartCoroutine(Anim(this, _spriteRender));
  }

  private SpriteRenderer _spriteRender;
  private Coroutine _animCoroutine;
  private volatile bool _animEnded;

  private static IEnumerator Anim(GameLogo gameLogo, SpriteRenderer spriteRender) {
    Color c = spriteRender.color;
    while (c.a < 1.0f) {
      c.a += 0.1f;
      spriteRender.color = c;
      yield return new WaitForSeconds(0.2f);
    }
    while (c.a >= 0.1f) {
      c.a -= 0.1f;
      spriteRender.color = c;
      yield return new WaitForSeconds(0.2f);
    }
    gameLogo.AnimEnded();
  }

  private void AnimEnded() {
    DebugLogger.WriteInfo("GameLogo.AnimEnded _animEnded = {0}", _animEnded.ToString());
    if (!_animEnded && OnAnimEnded != null) {
      _animEnded = true;
      OnAnimEnded();
    }
  }
}

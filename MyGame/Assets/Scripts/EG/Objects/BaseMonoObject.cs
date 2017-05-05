using UnityEngine;
using System;

using EG.Kernel;
using EG.Misc;

namespace EG.Objects {

public abstract class BaseMonoObject : MonoBehaviour, IBaseMonoObject {

  #region Реализация интерфейса IBaseIDObject

  object IBaseIDObject.Id {
    get { return _id; }
  }

  #endregion

  #region Реализация интерфейса IBaseIDObject<TIdType>

  public float Id {
    get { return _id; }
    protected set { _id = value; }
  }
  private float _id;

  #endregion

  #region Реализация интерфейса IParamObject

  public Action<string, object, object> OnParamChanged { get; set; }

  #endregion

  #region Реализация интерфейса INotificationObject

  public virtual bool IsNotificationSended {
    get { return _isNotificationSended; }
    set { _isNotificationSended = value; }
  }
  private volatile bool _isNotificationSended;

  public virtual void SendNotification(int what, params object[] notificationArgs) {

  }

  #endregion

  public Transform __Transform {
    get { return _transform == null ? transform : _transform; }
    set { _transform = value; }
  }
  private Transform _transform;

  public BaseMonoObject() {
    Id = IDGenerator.Get<float>(this);
  }

  #region MonoBehaviour Events

  // Use this for initialization
  protected virtual void Start() {
    _transform = transform;
    if (Id.IsEqual(0f)) {
      Id = IDGenerator.Get<float>(this);
    }
  }

  #endregion
}

public abstract class BaseMonoObject<TIdType> : MonoBehaviour, IBaseMonoObject<TIdType> {

  #region Реализация интерфейса IBaseIDObject

  object IBaseIDObject.Id {
    get { return _id; }
  }

  #endregion

  #region Реализация интерфейса IBaseIDObject<TIdType>

  public TIdType Id {
    get { return _id; }
    protected set { _id = value; }
  }
  private TIdType _id;

  #endregion

  #region Реализация интерфейса IParamObject

  public Action<string, object, object> OnParamChanged { get; set; }

  #endregion

  #region Реализация интерфейса INotificationObject

  public virtual bool IsNotificationSended {
    get { return _isNotificationSended; }
    set { _isNotificationSended = value; }
  }
  private volatile bool _isNotificationSended;

  public virtual void SendNotification(int what, params object[] notificationArgs) {

  }

  #endregion

  public Transform __Transform {
    get { return _transform == null ? transform : _transform; }
    set { _transform = value; }
  }
  private Transform _transform = null;

  public BaseMonoObject() {
    Id = IDGenerator.Get<TIdType>(this);
  }

  #region MonoBehaviour Events

  // Use this for initialization
  protected virtual void Start() {
    _transform = transform;
    if ((object)Id == (object)default(TIdType)) {
      Id = IDGenerator.Get<TIdType>(this);
    }
  }

  #endregion
}
}
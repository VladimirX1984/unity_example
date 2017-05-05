using System;

using EG.Kernel;

namespace EG.Objects {

public class BaseIDObject<TIdType> : IBaseIDObject<TIdType>, INamedObject, IParamObject,
  INotificationObject {

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

  #region Реализация интерфейса INamedObject

  public virtual string Name {
    get { return (_name == null ? String.Empty : _name); }
    set {
      var prev = _name;
      _name = (value == null ? String.Empty : value);
      if (_name != prev) {
        if (IsNotificationSended) {
          if (OnParamChanged != null) {
            OnParamChanged("Name", prev, _name);
          }
        }
      }
    }
  }
  private string _name = String.Empty;

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

  public BaseIDObject() {
    Init();
  }

  public BaseIDObject(string name) {
    Init();
    Name = name;
  }

  public BaseIDObject(string name, TIdType id) {
    Id = id;
  }

  private void Init() {
    Id = IDGenerator.Get<TIdType>(this);
  }
}

public class BaseIDObjectEx<TIdType> : BaseIDObject<TIdType>, IBaseIDObjectEx<TIdType> {

  #region Реализация интерфейса IIDObjectBaseEx<TIdType>

  public void SetId(TIdType id) {
    Id = id;
  }

  #endregion

  public BaseIDObjectEx(string name)
  : base(name) {

  }

  public BaseIDObjectEx(string name, TIdType id)
  : base(name, id) {

  }
}
}

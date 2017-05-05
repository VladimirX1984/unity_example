using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;

using EG.Kernel;

namespace EG.Containers {
[Serializable]
[DebuggerDisplay("Count = {Count}")]
public class ElementGroup<TItem> : List<TItem>, IElementGroup<TItem> {

  #region Реализация интерфейса IGroup

  IEnumerator IGroup.GetEnumerator() {
    return base.GetEnumerator();
  }

  #endregion

  #region Реализация интерфейса IGroup<TItem>

  public new IEnumerator<TItem> GetEnumerator() {
    return base.GetEnumerator();
  }

  #endregion

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

  #region Реализация интерфейса ISerializable

  [SecurityPermissionAttribute(SecurityAction.LinkDemand,
                               Flags = SecurityPermissionFlag.SerializationFormatter)]
  public virtual void GetObjectData(SerializationInfo info, StreamingContext context) {
    info.AddValue("Id", Id);
    info.AddValue("Count", base.Count);
    int index = 0;
    foreach (var element in this) {
      info.AddValue("element_" + index, element, typeof(TItem));
      index++;
    }
  }

  protected ElementGroup(SerializationInfo info, StreamingContext context) {
    Id = info.GetSingle("Id");
    int count = info.GetInt32("Count");
    for (int index = 0; index < count; index++) {
      var element = (TItem)info.GetValue("element_" + index, typeof(TItem));
      Add(element);
    }
  }

  #endregion

  #region Реализация интерфейса ICloneable

  public virtual object Clone() {
    var elemGroup = new ElementGroup<TItem>();
    foreach (var elem in this) {
      elemGroup.Add((elem is ICloneable) ? (TItem)(elem as ICloneable).Clone() : elem);
    }
    return elemGroup;
  }

  #endregion

  public ElementGroup()
  : base() {
    Init();
  }

  public ElementGroup(IEnumerable<TItem> collection)
  : base(collection) {
    Init();
  }

  public ElementGroup(int capacity)
  : base(capacity) {
    Init();
  }

  private void Init() {
    Id = IDGenerator.Get<float>(this);
  }
}
}
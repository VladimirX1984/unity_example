using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;

using EG.Objects;

namespace EG.Containers {
[Serializable]
[DebuggerDisplay("Count = {Count}")]
public class ElementRefGroup<TItem> : BaseIDSerializableObject<float>, IElementRefGroup<TItem> {

  #region Реализация интерфейса IElementRefGroup<TItem>

  public IElementGroup<TItem> Elements { get; protected set; }

  public TItem this[int index] {
    get { return Elements[index]; }
    set { Elements[index] = value; }
  }

  public virtual void Clear() {
    Elements.Clear();
  }

  #endregion

  #region Реализация интерфейса IGroup

  public int Count { get { return Elements.Count; } }

  IEnumerator IGroup.GetEnumerator() {
    return Elements.GetEnumerator();
  }

  #endregion

  #region Реализация интерфейса ISerializable

  [SecurityPermissionAttribute(SecurityAction.LinkDemand,
                               Flags = SecurityPermissionFlag.SerializationFormatter)]
  public virtual void GetObjectData(SerializationInfo info, StreamingContext context) {
    try {
      info.AddValue("Id", Id);
      info.AddValue("elements", Elements, typeof(IElementGroup<TItem>));
    }
    catch (Exception ex) {
      DebugLogger.WriteError(ex);
    }
  }

  protected ElementRefGroup(SerializationInfo info, StreamingContext context) {
    Id = info.GetSingle("Id");
    Elements = (IElementGroup<TItem>)info.GetValue("elements", typeof(IElementGroup<TItem>));
  }

  #endregion

  #region Реализация интерфейса ICloneable

  public virtual object Clone() {
    var elemGroup = new ElementGroup<TItem>();
    foreach (var elem in Elements) {
      elemGroup.Add((elem is ICloneable) ? (TItem)(elem as ICloneable).Clone() : elem);
    }
    return new ElementRefGroup<TItem>(elemGroup);
  }

  #endregion

  public ElementRefGroup()
  : base() {
    Elements = new ElementGroup<TItem>();
  }

  public ElementRefGroup(int capacity)
  : base() {
    Elements = new ElementGroup<TItem>(capacity);
  }

  public ElementRefGroup(IEnumerable<TItem> items)
  : base() {
    Elements = new ElementGroup<TItem>(items);
  }

  public ElementRefGroup(IElementGroup<TItem> items, bool copy)
  : base() {
    Elements = copy ? new ElementGroup<TItem>(items) : items;
  }
}
}

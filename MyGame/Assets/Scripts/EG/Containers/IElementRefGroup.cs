using System;

namespace EG.Containers {

/// <summary>
/// Интерфейс, от которого реализуется группа элементов
/// </summary>
/// <typeparam name="TItem">тип элемента</typeparam>
public interface IElementRefGroup<TItem> : IGroup, ICloneable {

  IElementGroup<TItem> Elements { get; }

  TItem this[int index] { get; set; }

  void Clear();
}
}

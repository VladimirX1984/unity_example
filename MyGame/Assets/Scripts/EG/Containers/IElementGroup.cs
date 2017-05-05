using System;
using System.Collections.Generic;

using EG.Kernel;

namespace EG.Containers {
/// <summary>
/// Интерфейс, от которого реализуется группа элементов
/// </summary>
/// <typeparam name="TItem">тип элемента</typeparam>
public interface IElementGroup<TItem> : IGroup<TItem>, IBaseIDObject<float>, IList<TItem>,
  ICloneable {

  new int Count { get; }

  new IEnumerator<TItem> GetEnumerator();

  int FindIndex(Predicate<TItem> match);
  int FindIndex(int startIndex, Predicate<TItem> match);

  int FindLastIndex(Predicate<TItem> match);
  int FindLastIndex(int startIndex, Predicate<TItem> match);
}
}

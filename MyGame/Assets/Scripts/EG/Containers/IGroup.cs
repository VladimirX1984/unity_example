using System.Collections;
using System.Collections.Generic;

namespace EG.Containers {
/// <summary>
/// Интерфейс, от которого реализуются все группы, содержащие ноль или более элементов
/// </summary>
public interface IGroup : IBaseGroup {

  int Count { get; }

  IEnumerator GetEnumerator();
}

/// <summary>
/// Интерфейс, от которого реализуются все группы, имеющие однотипные элементы
/// </summary>
public interface IGroup<TItem> : IGroup {

  void Add(TItem item);

  bool Contains(TItem item);

  bool Remove(TItem item);

  new IEnumerator<TItem> GetEnumerator();
}
}

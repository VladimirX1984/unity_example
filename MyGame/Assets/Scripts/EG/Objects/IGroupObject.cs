using System.Collections.Generic;

using EG.Containers;

namespace EG.Objects {
/// <summary>
/// Интерфейс, от которого реализуются объекты-контейнеры, которые содержат группу объектов
/// </summary>
public interface IGroupObject : IGroup {
  IGroup Group { get; }
}

/// <summary>
/// Интерфейс, от которого реализуются объекты-контейнеры, которые содержат группу объектов
/// </summary>
public interface IGroupObject<TItem> : IGroup {
  IGroup<TItem> Group { get; }
}

/// <summary>
/// Интерфейс, от которого реализуются объекты-контейнеры, которые содержат группу объектов
/// </summary>
public interface IGroupObject<TGroup, TItem> : IGroup<TItem>
  where TGroup : IGroup<TItem> {
  TGroup Group { get; }

  /// <summary>
  /// Возвращает последовательность элементов всех уровней, если all = true, иначе только верхнего уровня
  /// </summary>
  /// <param name="all"><c>true</c>, взять элементы всех уровней</param>
  /// <returns></returns>
  IEnumerable<TItem> GetObjects(bool all = false);
}
}
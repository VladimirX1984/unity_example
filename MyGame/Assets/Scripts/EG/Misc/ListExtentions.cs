using System;
using System.Collections.Generic;

namespace EG.Misc {
public static class ListExtentions {
  /// <summary>
  /// Меняет местами два значения в динамическом массиве List
  /// </summary>
  /// <param name="list">Динамический массив в котором необходимо произвести обмен значений</param>
  /// <param name="indexA">индекс первого значения</param>
  /// <param name="indexB">Индекс второго значения</param>
  /// <returns><c>List</c>Функция возвращает ссылку на переданый массив</returns>
  public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB) {
    if (indexA >= list.Count || indexB >= list.Count || indexA < 0 || indexB < 0 || indexA == indexB) {
      return list;
    }

    T tmp = list[indexA];
    list[indexA] = list[indexB];
    list[indexB] = tmp;

    return list;
  }

  public static IList<TItem> Clone<TItem>(this IList<TItem> originalItems) {
    var items = (List<TItem>)Activator.CreateInstance(originalItems.GetType());
    items.AddRange(originalItems);
    return items;
  }

  public static IList<TItem> CloneElements<TItem>(this IList<TItem> originalItems)
  where TItem : ICloneable {
    var items = (List<TItem>)Activator.CreateInstance(originalItems.GetType());
    originalItems.EachAction(it => items.Add((TItem)it.Clone()));
    return items;
  }

  public static List<TItem> CloneElements<TItem>(this List<TItem> originalItems)
  where TItem : ICloneable {
    var items = new List<TItem>(originalItems);
    originalItems.ForEach(it => items.Add((TItem)it.Clone()));
    return items;
  }
}
}

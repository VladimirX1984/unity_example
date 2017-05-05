using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EG.Misc {
/// <summary>
/// Класс-расширение для добавления дополнительных методов при работе с перечислениями
/// </summary>
public static class EnumerableExtensions {
  /// <summary>
  /// Возвращает новый список элементов типа TItemType. Метод аналогичен методу Cast()
  /// </summary>
  /// <typeparam name="FromType">тип элемента в последовательности</typeparam>
  /// <typeparam name="TItemType">тип элемента</typeparam>
  /// <param name="originalItems">последовательность</param>
  /// <returns>список элементов типа TItemType</returns>
  public static List<TItemType> ToList<FromType, TItemType>(this IEnumerable<FromType>
      originalItems) {
    var items = new List<TItemType>();

    foreach (FromType item in originalItems) {
      if (!(item is TItemType)) {
        ExceptionGenerator.Run<InvalidCastException>("Элемент типа \"{0}\" нельзя преобразовать в \"{1}\"",
            item.GetType(), typeof(TItemType)); ;
      }

      items.Add((TItemType)(object)item);
    }
    return items;
  }

  /// <summary>
  /// Возвращает список свойства элементов типа TItemProp
  /// </summary>
  /// <typeparam name="FromType">тип элемента в последовательности</typeparam>
  /// <typeparam name="TItemProp">тип свойства</typeparam>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="propName">имя свойства</param>
  /// <returns>список свойства элементов типа TItemField</returns>
  public static List<TItemProp> ToItemPropList<FromType, TItemProp>(this IEnumerable<FromType>
      originalItems, string propName) {
    var items = new List<TItemProp>();

    if (!originalItems.Any()) {
      return items;
    }

    var propValue = originalItems.ElementAt(0).GetFlattenPropertyValue(propName);
    if (!typeof(TItemProp).IsAssignableFrom(propValue.GetType())) {
      ExceptionGenerator.Run<InvalidCastException>("Свойство \"{0}\" должно реализовывать интерфейс (класс) \"{1}\"",
          propName, typeof(TItemProp));
    }

    foreach (FromType item in originalItems) {
      var propVal = item.GetFlattenPropertyValue(propName);
      if (!(propVal is TItemProp)) {
        ExceptionGenerator.Run<InvalidCastException>("Свойство элемента типа \"{0}\" нельзя преобразовать в \"{1}\"",
            item.GetType(), typeof(TItemProp));
      }
      items.Add((TItemProp)propVal);
    }
    return items;
  }

  /// <summary>
  /// Возвращает список свойства элементов типа TItemField
  /// </summary>
  /// <typeparam name="FromType">тип элемента в последовательности</typeparam>
  /// <typeparam name="TItemField">тип свойства</typeparam>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="propName">имя свойства</param>
  /// <returns>список свойства элементов типа TItemField</returns>
  public static List<TItemField> ToItemFieldList<FromType, TItemField>(this IEnumerable<FromType>
      originalItems, string fieldName) {
    var items = new List<TItemField>();

    if (!originalItems.Any()) {
      return items;
    }

    var fieldValue = originalItems.ElementAt(0).GetFlattenFieldValue(fieldName);
    if (!typeof(TItemField).IsAssignableFrom(fieldValue.GetType())) {
      ExceptionGenerator.Run<InvalidCastException>("Поле \"{0}\" должно реализовывать интерфейс (класс) \"{1}\"",
          fieldName, typeof(TItemField));
    }

    foreach (FromType item in originalItems) {
      var fieldVal = item.GetFlattenFieldValue(fieldName);
      if (!(fieldVal is TItemField)) {
        ExceptionGenerator.Run<InvalidCastException>("Поле элемента типа \"{0}\" нельзя преобразовать в \"{1}\"",
            item.GetType(), typeof(TItemField));
      }
      items.Add((TItemField)fieldVal);
    }
    return items;
  }

  /// <summary>
  /// проверяет наличия элемента в последовательности, у которого значение свойства
  /// , имя которого определено вторым аргументом метода
  /// , равно значение свойства объекта, определяемого первым аргументом
  /// </summary>
  /// <typeparam name="TItem">тип элемента</typeparam>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="item">объект</param>
  /// <param name="propName">свойство по которому определяется наличие элемента в последовательности</param>
  /// <returns></returns>
  public static bool ContainsByPropName<TItem>(this IEnumerable<TItem> originalItems, TItem item,
      string propName) {
    if (item == null) {
      ExceptionGenerator.Run<ArgumentNullException>("Объект не определен");
    }

    if (String.IsNullOrEmpty(propName)) {
      ExceptionGenerator.Run<ArgumentNullException>("Имя свойства не определено или пустое");
    }

    var val = item.GetFlattenPropertyValue(propName);

    if (val == null) {
      ExceptionGenerator.Run<NullReferenceException>("Свойство \"{0}\" элемента не определено ",
          propName);
    }

    return originalItems.Any(it => {
      var itVal = it.GetFlattenPropertyValue(propName);
      return ObjectExtensions.IsEqual(val, itVal);
    });
  }

  /// <summary>
  /// проверяет наличия элемента в последовательности, у которого значение свойства
  /// , имя которого определено вторым аргументом метода
  /// , равно значение свойства объекта, определяемого первым аргументом
  /// </summary>
  /// <typeparam name="TItem">тип элемента</typeparam>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="item">объект</param>
  /// <param name="propName">свойство по которому определяется наличие элемента в последовательности</param>
  /// <returns></returns>
  public static bool ContainsByFieldName<TItem>(this IEnumerable<TItem> originalItems, TItem item,
      string fieldName) {
    if (item == null) {
      ExceptionGenerator.Run<ArgumentNullException>("Объект не определен");
    }

    if (String.IsNullOrEmpty(fieldName)) {
      ExceptionGenerator.Run<ArgumentNullException>("Имя поля не определено или пустое");
    }

    var val = item.GetFlattenFieldValue(fieldName);

    if (val == null) {
      ExceptionGenerator.Run<NullReferenceException>("Поле \"{0}\" элемента не определено ",
          fieldName);
    }

    return originalItems.Any(it => {
      var itVal = it.GetFlattenFieldValue(fieldName);
      return ObjectExtensions.IsEqual(val, itVal);
    });
  }

  /// <summary>
  /// Изменяет значение свойства у элементов перечисления
  /// </summary>
  /// <typeparam name="TItem">тип элемента</typeparam>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="propertyName">имя свойства</param>
  /// <param name="propertyValue">новое значение свойства</param>
  /// <returns>возвращает true, если значения свойства всех элементов изменены</returns>
  public static bool ChangeItemsPropValue<TItem>(this IEnumerable<TItem> originalItems,
      string propertyName, object propertyValue) {
    if (String.IsNullOrEmpty(propertyName)) {
      ExceptionGenerator.Run<ArgumentNullException>("Имя свойства не определено или пустое");
    }

    var bAllChanged = true;
    foreach (TItem item in originalItems) {
      PropertyInfo propInfo = item.GetFlattenProperty(propertyName);
      if (propInfo == null) {
        bAllChanged = false;
        continue;
      }
      var propValue = item.GetPropertyValue(propInfo);

      object convertedValue = null;
      if (!propInfo.PropertyType.TryConvert(propertyValue, ref convertedValue)) {
        bAllChanged = false;
        continue;
      }

      if (ObjectExtensions.IsEqual(propValue, convertedValue)) {
        continue;
      }

      item.SetPropertyValue(propInfo, convertedValue);
    }

    return bAllChanged;
  }

  /// <summary>
  /// Изменяет значение свойства у элементов перечисления, которые выбраны по условию predicate
  /// </summary>
  /// <typeparam name="TItem">тип элемента</typeparam>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="propertyName">имя свойства</param>
  /// <param name="propertyValue">новое значение свойства</param>
  /// <param name="predicate">условие выбора элементов, у которых будут изменены свойства</param>
  /// <returns>возвращает true, если значения свойства всех выбранных элементов изменены</returns>
  public static bool ChangeItemsPropValue<TItem>(this IEnumerable<TItem> originalItems,
      string propertyName, object propertyValue, Func<TItem, bool> predicate) {
    if (predicate == null) {
      return originalItems.ChangeItemsPropValue<TItem>(propertyName, propertyValue);
    }
    var items = originalItems.Where(predicate);
    return items.ChangeItemsPropValue<TItem>(propertyName, propertyValue);
  }

  /// <summary>
  /// Изменяет значение свойства у элемента
  /// </summary>
  /// <typeparam name="TItem">тип элемента</typeparam>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="index">индекс элемента</param>
  /// <param name="propertyName">имя свойства</param>
  /// <param name="propertyValue">новое значение свойства</param>
  /// <returns>возвращает true, если значение изменено</returns>
  public static bool ChangeItemPropValue<TItem>(this IEnumerable<TItem> originalItems, int index,
      string propertyName, object propertyValue) {
    if (String.IsNullOrEmpty(propertyName)) {
      ExceptionGenerator.Run<ArgumentNullException>("Имя свойства не определено или пустое");
    }

    if (index < 0 || index >= originalItems.Count()) {
      ExceptionGenerator.Run<ArgumentOutOfRangeException>("Индекс должен быть не меньше 0 и не больше {0}",
          originalItems.Count());
    }

    TItem item = originalItems.ElementAt(index);

    PropertyInfo propInfo = item.GetProperty(propertyName,
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
    if (propInfo == null) {
      ExceptionGenerator.Run<MemberNotFoundException>("Свойство \"{0}\" не найдено",
          propertyName);
    }
    var propValue = item.GetPropertyValue(propInfo);

    object convertedValue = null;
    if (!propInfo.PropertyType.TryConvert(propertyValue, ref convertedValue)) {
      ExceptionGenerator.Run<InvalidCastException>(
        "Невозможно преобразовать из типа \"{0}\" в тип \"{1}\"",
        propValue.GetType(), propInfo.PropertyType);
      return false;
    }

    if (ObjectExtensions.IsEqual(propValue, convertedValue)) {
      return false;
    }

    item.SetPropertyValue(propInfo, convertedValue);
    return true;
  }

  /// <summary>
  /// Выполнить действие над каждым элементом последовательности
  /// </summary>
  /// <typeparam name="TItem">элемент</typeparam>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="action">действие</param>
  public static void EachAction<TItem>(this IEnumerable<TItem> originalItems, Action<TItem> action) {
    foreach (var it in originalItems) {
      action(it);
    }
  }

  /// <summary>
  /// Выполнить действие над теми элементами последовательности, которые подпадаюь под условие, заданное предикатом
  /// </summary>
  /// <typeparam name="TItem">элемент</typeparam>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="action">действие</param>
  /// <param name="predicate">условие, при котором будет выполняться действие над элементом</param>
  public static void EachAction<TItem>(this IEnumerable<TItem> originalItems, Action<TItem> action,
                                       Predicate<TItem> predicate) {
    foreach (var it in originalItems) {
      if (predicate(it)) {
        action(it);
      }
    }
  }

  /// <summary>
  /// Выполнить действие над элементами последовательности до тех пор, пока выполняется условие, заданное предикатом
  /// </summary>
  /// <typeparam name="TItem">элемент</typeparam>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="action">действие</param>
  public static void While<TItem>(this IEnumerable<TItem> originalItems,
                                  ReturnAction<TItem, bool> action) {
    foreach (var it in originalItems) {
      if (action(it) == false) {
        break;
      }
    }
  }

  /// <summary>
  /// Выполнить действие над элементами последовательности до тех пор, пока выполняется условие, заданное предикатом
  /// </summary>
  /// <typeparam name="TItem">элемент</typeparam>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="action">действие</param>
  /// <param name="predicate">условие, при котором будет продолжаться обход</param>
  public static void While<TItem>(this IEnumerable<TItem> originalItems, Action<TItem> action,
                                  Predicate<TItem> predicate) {
    foreach (var it in originalItems) {
      if (predicate(it) == false) {
        break;
      }
      action(it);
    }
  }

  /// <summary>
  /// Возвращает индекс первого элемента item в последовательности
  /// </summary>
  /// <typeparam name="TItem">тип элемента</typeparam>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="item">элемент</param>
  /// <returns>возвращает индекс элемента в последовательности</returns>
  public static int FirstIndex<TItem>(this IEnumerable<TItem> originalItems, TItem item) {
    int index = 0;
    foreach (var it in originalItems) {
      if (it.IsEquals(item)) {
        return index;
      }
      ++index;
    }
    return -1;
  }

  /// <summary>
  /// Возвращает индекс первого найденного элемента по заданному условию. Если элемент не найден, то выдается исключение
  /// </summary>
  /// <typeparam name="TItem">тип элемента</typeparam>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="predicate">условие поиска</param>
  /// <returns>возвращает индекс найденного элемента</returns>
  public static int FirstIndex<TItem>(this IEnumerable<TItem> originalItems,
                                      Func<TItem, bool> predicate) {
    var item = originalItems.First(predicate);
    return originalItems.FirstIndex(item);
  }

  /// <summary>
  /// Возвращает индекс первого найденного элемента по заданному условию. Если элемент не найден, то возвращает значение по умолчанию для типа
  /// </summary>
  /// <typeparam name="TItem">тип элемента</typeparam>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="predicate">условие поиска</param>
  /// <returns>возвращает индекс найденного элемента</returns>
  public static int FirstIndexOrDefault<TItem>(this IEnumerable<TItem> originalItems,
      Func<TItem, bool> predicate) {
    var item = originalItems.FirstOrDefault(predicate);
    return originalItems.FirstIndex(item);
  }

  public static KeyValuePair<int, TItem> FirstPair<TItem>(this IEnumerable<TItem> originalItems) {
    var item = originalItems.First();
    return new KeyValuePair<int, TItem>(originalItems.FirstIndex(item), item);
  }

  public static KeyValuePair<int, TItem> FirstPair<TItem>(this IEnumerable<TItem> originalItems,
      Func<TItem, bool> predicate) {
    var item = originalItems.First(predicate);
    return new KeyValuePair<int, TItem>(originalItems.FirstIndex(item), item);
  }

  public static KeyValuePair<int, TItem> FirstPairOrDefault<TItem>(this IEnumerable<TItem>
      originalItems) {
    var item = originalItems.FirstOrDefault();
    return new KeyValuePair<int, TItem>(originalItems.FirstIndex(item), item);
  }

  public static KeyValuePair<int, TItem> FirstPairOrDefault<TItem>(this IEnumerable<TItem>
      originalItems, Func<TItem, bool> predicate) {
    var item = originalItems.FirstOrDefault(predicate);
    return new KeyValuePair<int, TItem>(originalItems.FirstIndex(item), item);
  }

  /// <summary>
  /// Возвращает элемент последовательности по индексу. Если индекс выходит за диапазон последовательности, то выдает исключение
  /// </summary>
  /// <typeparam name="TItem">тип элемента</typeparam>
  /// <param name="originalItems">последовательность</param>
  /// <param name="index">индекс</param>
  /// <returns>элемент</returns>
  public static TItem Get<TItem>(this IEnumerable<TItem> originalItems, int index) {
    if (index < 0 || index >= originalItems.Count()) {
      ExceptionGenerator.Run<ArgumentOutOfRangeException>("Индекс \"{0}\" должен быть не меньше \"0\" и меньше \"{1}\"",
          index, originalItems.Count());
    }

    return originalItems.ElementAt(index);
  }

  /// <summary>
  /// Возвращает элемент последовательности по индексу. Если элемент не найден, то возвращается значение по умолчанию для типа
  /// </summary>
  /// <typeparam name="TItem">тип элемента</typeparam>
  /// <param name="originalItems">последовательность</param>
  /// <param name="index">индекс</param>
  /// <returns>элемент</returns>
  public static TItem GetOrDefault<TItem>(this IEnumerable<TItem> originalItems, int index) {
    return originalItems.ElementAtOrDefault(index);
  }

  /// <summary>
  /// Возвращает индекс последнего элемента item в последовательности
  /// </summary>
  /// <typeparam name="TItem">тип элемента</typeparam>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="item">элемент</param>
  /// <returns>возвращает индекс элемента в последовательности</returns>
  public static int LastIndex<TItem>(this IEnumerable<TItem> originalItems, TItem item) {
    int index = originalItems.Count() - 1;
    var items = originalItems.Reverse();
    foreach (var it in items) {
      if (it.IsEquals(item)) {
        return index;
      }
      index--;
    }
    return -1;
  }

  /// <summary>
  /// Возвращает индекс последнего найденного элемента по заданному условию. Если элемент не найден, то выдается исключение
  /// </summary>
  /// <typeparam name="TItem">тип элемента</typeparam>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="predicate">условие поиска</param>
  /// <returns>возвращает индекс найденного элемента</returns>
  public static int LastIndex<TItem>(this IEnumerable<TItem> originalItems,
                                     Func<TItem, bool> predicate) {
    var item = originalItems.Last(predicate);
    return originalItems.LastIndex(item);
  }

  public static KeyValuePair<int, TItem> LastPair<TItem>(this IEnumerable<TItem> originalItems) {
    var item = originalItems.Last();
    return new KeyValuePair<int, TItem>(originalItems.LastIndex(item), item);
  }

  public static KeyValuePair<int, TItem> LastPair<TItem>(this IEnumerable<TItem> originalItems,
      Func<TItem, bool> predicate) {
    var item = originalItems.Last(predicate);
    return new KeyValuePair<int, TItem>(originalItems.LastIndex(item), item);
  }

  public static KeyValuePair<int, TItem> LastPairOrDefault<TItem>(this IEnumerable<TItem>
      originalItems) {
    var item = originalItems.LastOrDefault();
    return new KeyValuePair<int, TItem>(originalItems.LastIndex(item), item);
  }

  public static KeyValuePair<int, TItem> LastPairOrDefault<TItem>(this IEnumerable<TItem>
      originalItems, Func<TItem, bool> predicate) {
    var item = originalItems.LastOrDefault(predicate);
    return new KeyValuePair<int, TItem>(originalItems.LastIndex(item), item);
  }

  public static KeyValuePair<int, TItem> SinglePair<TItem>(this IEnumerable<TItem> originalItems) {
    var item = originalItems.Single();
    return new KeyValuePair<int, TItem>(originalItems.FirstIndex(item), item);
  }

  public static KeyValuePair<int, TItem> SinglePair<TItem>(this IEnumerable<TItem> originalItems,
      Func<TItem, bool> predicate) {
    var item = originalItems.Single(predicate);
    return new KeyValuePair<int, TItem>(originalItems.FirstIndex(item), item);
  }

  public static KeyValuePair<int, TItem> SinglePairOrDefault<TItem>(this IEnumerable<TItem>
      originalItems) {
    var item = originalItems.SingleOrDefault();
    return new KeyValuePair<int, TItem>(originalItems.FirstIndex(item), item);
  }

  public static KeyValuePair<int, TItem> SinglePairOrDefault<TItem>(this IEnumerable<TItem>
      originalItems, Func<TItem, bool> predicate) {
    var item = originalItems.SingleOrDefault(predicate);
    return new KeyValuePair<int, TItem>(originalItems.FirstIndex(item), item);
  }

  /// <summary>
  /// Возвращает true, если элемент в последовательности найден
  /// </summary>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="predicate">условие поиска</param>
  /// <returns>возвращает true, если элемент в последовательности найден</returns>
  public static bool Any(this IEnumerable originalItems, Func<object, bool> predicate) {
    foreach (var it in originalItems) {
      if (predicate(it)) {
        return true;
      }
    }
    return false;
  }

  /// <summary>
  /// Возвращает первый элемент, если элемент в последовательности найден
  /// </summary>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="predicate">условие поиска</param>
  /// <returns>возвращает первый элемент, если элемент в последовательности найден</returns>
  public static object First(this IEnumerable originalItems, Func<object, bool> predicate) {
    foreach (var it in originalItems) {
      if (predicate(it)) {
        return it;
      }
    }
    ExceptionGenerator.Run<InvalidOperationException>("Элемент не найден в последовательности");
    return null;
  }

  /// <summary>
  /// Возвращает последний элемент, если элемент в последовательности найден
  /// </summary>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="predicate">условие поиска</param>
  /// <returns>возвращает последний элемент, если элемент в последовательности найден</returns>
  public static object Last(this IEnumerable originalItems, Func<object, bool> predicate) {
    var items = new List<object>();
    foreach (var it in originalItems) {
      if (predicate(it)) {
        items.Add(it);
      }
    }
    if (items.Count == 0) {
      ExceptionGenerator.Run<InvalidOperationException>("Элемент не найден в последовательности");
    }
    return items.Last();
  }

  /// <summary>
  /// Возвращает размер последовательности
  /// </summary>
  /// <param name="originalItems">последовательность элементов</param>
  /// <returns>число элементов</returns>
  public static int Size(this IEnumerable originalItems) {
    var size = 0;
    foreach (var it in originalItems) {
      size++;
    }
    return size;
  }

  /// <summary>
  /// Возвращает размер последовательности
  /// </summary>
  /// <param name="originalItems">последовательность элементов</param>
  /// <param name="predicate">условие поиска</param>
  /// <returns>число элементов</returns>
  public static int Size(this IEnumerable originalItems, Func<object, bool> predicate) {
    var size = 0;
    foreach (var it in originalItems) {
      if (predicate(it)) {
        size++;
      }
    }
    return size;
  }
}
}

using System;
using System.Collections;

using EG.Containers;
using EG.Misc;
using EG.Objects;

namespace EG.Model {
/// <summary>
/// Менеджер групп элементов.
/// Содержит основные операции нахождения элемента в группе (список, дерево и так далее) и в последовательности (список, словарь и так далее)
/// </summary>
public sealed class ElementGroupManager : BaseManager, IManager {
  /// <summary>
  /// проверяет наличия элемента в группе, у которого значение свойства
  /// , имя которого определено вторым аргументом метода
  /// , равно значению, определяемого третьим аргументом
  /// </summary>
  /// <param name="group">группа элементов</param>
  /// <param name="propName">имя свойства</param>
  /// <param name="value">значение</param>
  /// <param name="recursive"><c>true</c>, ресурсивный обход элементов, если элемент является также группой</param>
  /// <param name="comparer">функция сравнения значений</param>
  /// <param name="contains">дополнительная функция обхода элементов у негруппового объекта</param>
  /// <returns></returns>
  public static bool ContainsByPropName(IBaseGroup group, string propName, object value,
                                        bool recursive = false, ReturnAction<object, object, bool> comparer = null,
                                        ReturnAction<object, object, bool> contains = null) {
    if (group == null) {
      ExceptionGenerator.Run<NullReferenceException>("Группа не определена");
    }

    if (group is IGroupObject) {
      var groupObject = group as IGroupObject;
      if (groupObject.Group != null) {
        foreach (var obj in groupObject.Group) {
          if (IsObjectHavingValue(obj, propName, value, recursive, comparer, contains)) {
            return true;
          }
        }
      }
    }
    else if (group is IGroupOfOne) {
      var groupOfOne = group as IGroupOfOne;
      if (groupOfOne.Element != null) {
        if (IsObjectHavingValue(groupOfOne.Element, propName, value, recursive, comparer, contains)) {
          return true;
        }
      }
    }
    else if (group is IGroup) {
      var group__ = group as IGroup;
      foreach (var obj in group__) {
        if (IsObjectHavingValue(obj, propName, value, recursive, comparer, contains)) {
          return true;
        }
      }
    }
    return false;
  }

  /// <summary>
  /// проверяет наличия элемента в группе, у которого значение свойства
  /// , имя которого определено вторым аргументом метода
  /// , равно значению, определяемого третьим аргументом
  /// </summary>
  /// <param name="group">группа элементов</param>
  /// <param name="propName">имя свойства</param>
  /// <param name="value">значение</param>
  /// <param name="recursive"><c>true</c>, ресурсивный обход элементов, если элемент является также группой</param>
  /// <param name="comparer">функция сравнения значений</param>
  /// <param name="contains">дополнительная функция обхода элементов у негруппового объекта</param>
  /// <returns></returns>
  public static bool ContainsByPropName(IEnumerable group, string propName, object value,
                                        bool recursive = false, ReturnAction<object, object, bool> comparer = null,
                                        ReturnAction<object, object, bool> contains = null) {
    if (group == null) {
      ExceptionGenerator.Run<NullReferenceException>("Последовательность не определена");
    }

    foreach (var obj in group) {
      if (IsObjectHavingValue(obj, propName, value, recursive, comparer, contains)) {
        return true;
      }
    }
    return false;
  }

  private static bool IsObjectHavingValue(object obj, string propName, object value,
                                          bool recursive = false, ReturnAction<object, object, bool> comparer = null,
                                          ReturnAction<object, object, bool> contains = null) {
    if (obj == null) {
      return false;
    }
    var propertyInfo = obj.GetFlattenProperty(propName);
    object propVal = null;
    if (propertyInfo != null) {
      propVal = obj.GetPropertyValue(propertyInfo, ObjectExtensions.AccessAttr.Public);
    }
    else {
      var fieldInfo = obj.GetFlattenField(propName);
      if (fieldInfo != null) {
        propVal = obj.GetFieldValue(fieldInfo);
      }
    }

    if (propVal != null) {
      if (comparer != null) {
        if (comparer(propVal, value)) {
          return true;
        }
      }
      else {
        if (ObjectExtensions.IsEqual(value, propVal)) {
          return true;
        }
      }
    }
    if (recursive) {
      if (obj is IBaseGroup) {
        if (ContainsByPropName(obj as IBaseGroup, propName, value, recursive, comparer, contains)) {
          return true;
        }
      }
      else if (obj is IEnumerable) {
        if (ContainsByPropName(obj as IEnumerable, propName, value, recursive, comparer, contains)) {
          return true;
        }
      }
    }
    if (contains != null) {
      if (contains(obj, value)) {
        return true;
      }
    }
    return false;
  }

  public ElementGroupManager()
  : base("ElementGroupManager") {

  }
}
}

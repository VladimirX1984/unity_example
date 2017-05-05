using System;
using System.Collections;
using System.Reflection;

namespace EG.Misc {
public static class ObjectExtensions {

  public static bool IsEqual(object obj1, object obj2, bool bDecimalExact = false) {
    if (obj1 == null && obj2 == null) {
      return true;
    }

    if (obj1 == null || obj2 == null) {
      return false;
    }

    if (obj1 is IList) {
      if (!obj1.GetType().Name.IsEqual(obj2.GetType().Name)) {
        return false;
      }
      IList list1 = obj1 as IList;
      IList list2 = obj2 as IList;
      if (list1.Count != list2.Count) {
        return false;
      }

      for (var i = 0; i < list1.Count; ++i) {
        if (!ObjectExtensions.IsEqual(list1[i], list2[i])) {
          return false;
        }
      }
      return true;
    }

    Type type1 = obj1.GetType();
    Type type2 = obj2.GetType();
    if (type1.IsPrimitive && type1.IsValueType && type2.IsPrimitive && type2.IsValueType) {
      if (type1 == typeof(float)) {
        if (bDecimalExact) {
          return Convert.ToSingle(obj1).IsEqual(Convert.ToSingle(obj2), 0.0000001f);
        }
        return Convert.ToSingle(obj1).IsEqual(Convert.ToSingle(obj2));
      }
      if (type1 == typeof(double) && !bDecimalExact) {
        return Convert.ToDouble(obj1).IsEqual(Convert.ToDouble(obj2));
      }
    }

    return obj1.Equals(obj2);
  }

  public static bool IsEquals(this object obj1, object obj2) {
    return ObjectExtensions.IsEqual(obj1, obj2);
  }

  public enum AccessAttr {
    Public,
    NonPublic,
    All
  }

  #region Работа с полями объекта

  /// <summary>
  /// Получить поле у объекта по типу поля fieldType
  /// </summary>
  /// <param name="obj"></param>
  /// <param name="fieldType">тип поля</param>
  /// <param name="bindingFlags">условие поиска поля</param>
  /// <exception cref="NullReferenceException"></exception>
  /// <exception cref="System.Data.NoNullAllowedException"></exception>
  /// <returns>объект-свойство</returns>
  public static FieldInfo GetField(this object obj, Type fieldType, BindingFlags bindingFlags) {
    if (obj == null) {
      ExceptionGenerator.Run<NullReferenceException>("Аргумент \"obj\" равен null");
    }

    Type type = obj.GetType();
    FieldInfo[] fields = type.GetFields(bindingFlags);
    foreach (var fieldInfo in fields) {
      if (fieldInfo.FieldType == fieldType) {
        return fieldInfo;
      }
    }
    ExceptionGenerator.Run<ApplicationException>("Ошибка при получении поля типа \"{0}\" объекта типа \"{1}\"",
        fieldType, type);
    return null;
  }

  /// <summary>
  /// Получить поле fieldName у объекта
  /// </summary>
  /// <param name="obj">объект</param>
  /// <param name="fieldName">имя поля</param>
  /// <param name="bindingFlags">условие поиска поля</param>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="TargetException"></exception>
  /// <exception cref="Exception"></exception>
  /// <returns>значение поля</returns>
  public static FieldInfo GetField(this object obj, string fieldName, BindingFlags bindingFlags) {
    if (obj == null) {
      ExceptionGenerator.Run<NullReferenceException>("Аргумент \"obj\" равен null");
    }

    if (String.IsNullOrEmpty(fieldName)) {
      ExceptionGenerator.Run<ArgumentNullException>("Аргумент \"fieldName\" равен null или пустой");
    }

    Type type = obj.GetType();

    try {
      FieldInfo fieldInfo = null;
      fieldInfo = type.GetField(fieldName, bindingFlags);
      return fieldInfo;
    }
    catch (Exception ex) {
      throw ExceptionGenerator.Run(ex,
                                   "Ошибка при получении поля \"{0}\" объекта типа \"{1}\"",
                                   fieldName, type);
    }
  }

  /// <summary>
  /// Получить поле определенного типа
  /// </summary>
  /// <param name="obj">объект</param>
  /// <param name="fieldName">имя поля определенного типа</param>
  /// <param name="nestedType">тип определенного типа</param>
  /// <returns>объект-поле</returns>
  public static FieldInfo GetField(this object obj, string fieldName, Type nestedType) {
    if (obj == null) {
      ExceptionGenerator.Run<NullReferenceException>("Аргумент \"obj\" равен null");
    }

    if (String.IsNullOrEmpty(fieldName)) {
      ExceptionGenerator.Run<ArgumentNullException>("Аргумент \"fieldName\" равен null или пустой метода объекта типа {0}",
          obj.GetType());
    }

    if (nestedType == null) {
      ExceptionGenerator.Run<ArgumentNullException>("Аргумент \"nestedType\" равен null (имя свойства вложенного типа \"{0}\")",
          fieldName);
    }

    Type type = obj.GetType();

    try {
      FieldInfo fieldInfo = type.GetField(fieldName, nestedType);
      return fieldInfo;
    }
    catch (Exception ex) {
      throw ExceptionGenerator.Run(ex,
                                   "Ошибка при получении значения поля \"{0}\" объекта типа \"{1}\"",
                                   fieldName, type);
    }
  }

  public static FieldInfo GetField(this object obj, string fieldName) {
    return obj.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
  }

  public static FieldInfo GetFlattenField(this object obj, string fieldName) {
    return obj.GetField(fieldName,
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
  }

  #region Получение значения поля по его имени

  public static object GetFieldValue(this object obj, string fieldName) {
    return obj.GetFieldValue(fieldName, BindingFlags.Public | BindingFlags.Instance);
  }

  public static object GetFlattenFieldValue(this object obj, string fieldName) {
    return obj.GetFieldValue(fieldName,
                             BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
  }

  public static object GetStaticFieldValue(this object obj, string fieldName) {
    return obj.GetFieldValue(fieldName, BindingFlags.Public | BindingFlags.Static);
  }

  public static object GetFlattenStaticFieldValue(this object obj, string fieldName) {
    return obj.GetFieldValue(fieldName,
                             BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
  }

  /// <summary>
  /// Получить значение поля fieldName у объекта
  /// </summary>
  /// <param name="obj">объект</param>
  /// <param name="fieldName">имя поля</param>
  /// <param name="bindingFlags">условие поиска поля</param>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="Exception"></exception>
  /// <returns>значение поля</returns>
  public static object GetFieldValue(this object obj, string fieldName, BindingFlags bindingFlags) {
    Type type = obj.GetType();
    try {
      FieldInfo fieldInfo = obj.GetField(fieldName, bindingFlags);

      if (fieldInfo == null) {
        ExceptionGenerator.Run<NullReferenceException>("Ошибка при получении поля с именем \"{0}\" объекта типа \"{1}\"",
            fieldName, type);
      }

      return fieldInfo.GetValue(obj);
    }
    catch (Exception ex) {
      throw ExceptionGenerator.Run(ex,
                                   "Ошибка при получении значения поля \"{0}\" объекта типа \"{1}\"",
                                   fieldName, type);
    }
  }

  #endregion

  #region Получение значения поля

  /// <summary>
  /// Получение значение поля fieldInfo у объекта
  /// </summary>
  /// <param name="obj">объект</param>
  /// <param name="fieldName">поле</param>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="Exception"></exception>
  /// <returns>объект-поле</returns>
  public static object GetFieldValue(this object obj, FieldInfo fieldInfo) {
    Type type = obj.GetType();

    if (fieldInfo == null) {
      ExceptionGenerator.Run<ArgumentNullException>("Ошибка при получении значения поля: поле объекта типа \"{0}\" неопределено",
          type);
    }

    try {
      return fieldInfo.GetValue(obj);
    }
    catch (Exception ex) {
      throw ExceptionGenerator.Run(ex,
                                   "Ошибка при установке значения поля \"{0}\" объекта типа \"{1}\"",
                                   fieldInfo.Name, type);
    }
  }

  #endregion

  #region Установка значения поля по его имени

  public static void SetFieldValue(this object obj, string fieldName, object value) {
    obj.SetFieldValue(fieldName, value, BindingFlags.Public | BindingFlags.Instance);
  }

  public static void SetFlattenFieldValue(this object obj, string fieldName, object value) {
    obj.SetFieldValue(fieldName, value,
                      BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
  }

  public static void SetStaticFieldValue(this object obj, string fieldName, object value) {
    obj.SetFieldValue(fieldName, value, BindingFlags.Public | BindingFlags.Static);
  }

  public static void SetFlattenStaticFieldValue(this object obj, string fieldName, object value) {
    obj.SetFieldValue(fieldName, value,
                      BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
  }

  /// <summary>
  /// Установить значение поля fieldName у объекта
  /// </summary>
  /// <param name="obj">объект</param>
  /// <param name="fieldName">имя поля</param>
  /// <param name="bindingFlags">условие поиска поля</param>
  /// <exception cref="Exception"></exception>
  public static void SetFieldValue(this object obj, string fieldName, object value,
                                   BindingFlags bindingFlags) {
    Type type = obj.GetType();
    try {
      FieldInfo fieldInfo = obj.GetField(fieldName, bindingFlags);
      fieldInfo.SetValue(obj, value);
    }
    catch (Exception ex) {
      throw ExceptionGenerator.Run(ex,
                                   "Ошибка при установке значения поля \"{0}\" объекта типа \"{1}\"",
                                   fieldName, type);
    }
  }

  #endregion

  #region Установка значения поля

  /// <summary>
  /// Установить значение поля fieldInfo у объекта
  /// </summary>
  /// <param name="obj">объект</param>
  /// <param name="fieldName">поле</param>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="Exception"></exception>
  public static void SetFieldValue(this object obj, FieldInfo fieldInfo, object value) {
    Type type = obj.GetType();

    if (fieldInfo == null) {
      ExceptionGenerator.Run<ArgumentNullException>("Ошибка при установке значения поля: поле объекта типа \"{0}\" неопределено",
          type);
    }

    try {
      fieldInfo.SetValue(obj, value);
    }
    catch (Exception ex) {
      throw ExceptionGenerator.Run(ex,
                                   "Ошибка при установке значения поля \"{0}\" объекта типа \"{1}\"",
                                   fieldInfo.Name, type);
    }
  }

  #endregion

  #endregion

  #region Работа с свойствами объекта

  /// <summary>
  /// Получить свойство у объекта по типу свойству propertyType
  /// </summary>
  /// <param name="obj"></param>
  /// <param name="propertyType">тип свойства</param>
  /// <param name="bindingFlags">условие поиска свойства</param>
  /// <exception cref="NullReferenceException"></exception>
  /// <exception cref="System.Data.NoNullAllowedException"></exception>
  /// <returns>объект-свойство</returns>
  public static PropertyInfo GetProperty(this object obj, Type propertyType,
                                         BindingFlags bindingFlags) {
    if (obj == null) {
      ExceptionGenerator.Run<NullReferenceException>("Аргумент \"obj\" равен null");
    }

    Type type = obj.GetType();
    PropertyInfo[] props = type.GetProperties(bindingFlags);
    foreach (var propInfo in props) {
      if (propInfo.PropertyType == propertyType) {
        return propInfo;
      }
    }
    ExceptionGenerator.Run<ApplicationException>("Ошибка при получении свойства типа \"{0}\" объекта типа \"{1}\"",
        propertyType, type);
    return null;
  }

  /// <summary>
  /// Получить свойство у объекта по имени свойству propertyName
  /// </summary>
  /// <param name="obj">объект</param>
  /// <param name="propertyName">имя свойства</param>
  /// <param name="bindingFlags">условие поиска свойства</param>
  /// <exception cref="NullReferenceException"></exception>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="Exception"></exception>
  /// <returns>объект-свойство</returns>
  public static PropertyInfo GetProperty(this object obj, string propertyName,
                                         BindingFlags bindingFlags) {
    if (obj == null) {
      ExceptionGenerator.Run<NullReferenceException>("Аргумент \"obj\" равен null");
    }

    if (String.IsNullOrEmpty(propertyName)) {
      ExceptionGenerator.Run<ArgumentNullException>("Аргумент \"propertyName\" равен null или пустой");
    }

    Type type = obj.GetType();

    try {
      PropertyInfo propertyInfo = type.GetProperty(propertyName, bindingFlags);
      return propertyInfo;
    }
    catch (Exception ex) {
      throw ExceptionGenerator.Run(ex,
                                   "Ошибка при получении значения свойства \"{0}\" объекта типа \"{1}\"",
                                   propertyName, type);
    }
  }

  /// <summary>
  /// Получить свойство определенного типа
  /// </summary>
  /// <param name="obj">объект</param>
  /// <param name="propertyName">имя свойства определенного типа</param>
  /// <param name="nestedType">тип определенного типа</param>
  /// <returns>объект-свойство</returns>
  public static PropertyInfo GetProperty(this object obj, string propertyName, Type nestedType) {
    if (obj == null) {
      ExceptionGenerator.Run<NullReferenceException>("Аргумент \"obj\" равен null");
    }

    if (String.IsNullOrEmpty(propertyName)) {
      ExceptionGenerator.Run<ArgumentNullException>("Аргумент \"propertyName\" равен null или пустой метода объекта типа {0}",
          obj.GetType());
    }

    if (nestedType == null) {
      ExceptionGenerator.Run<ArgumentNullException>("Аргумент \"nestedType\" равен null (имя свойства вложенного типа \"{0}\")",
          propertyName);
    }

    Type type = obj.GetType();

    try {
      PropertyInfo propertyInfo = type.GetProperty(propertyName, nestedType);
      return propertyInfo;
    }
    catch (Exception ex) {
      throw ExceptionGenerator.Run(ex,
                                   "Ошибка при получении значения свойства \"{0}\" объекта типа \"{1}\"",
                                   propertyName, type);
    }
  }

  public static PropertyInfo GetProperty(this object obj, string propertyName) {
    return obj.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
  }

  public static PropertyInfo GetFlattenProperty(this object obj, string propertyName) {
    return obj.GetProperty(propertyName,
                           BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
  }

  #region Получение значение свойства по его имени

  public static object GetPropertyValue(this object obj, string propertyName) {
    return obj.GetPropertyValue(propertyName, BindingFlags.Public | BindingFlags.Instance);
  }

  public static object GetFlattenPropertyValue(this object obj, string propertyName) {
    return obj.GetPropertyValue(propertyName,
                                BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
  }

  public static object GetStaticPropertyValue(this object obj, string propertyName) {
    return obj.GetPropertyValue(propertyName, BindingFlags.Public | BindingFlags.Static);
  }

  public static object GetFlattenStaticPropertValue(this object obj, string propertyName) {
    return obj.GetPropertyValue(propertyName,
                                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
  }

  /// <summary>
  /// Получить значение свойства propertyName у объекта
  /// </summary>
  /// <param name="obj">объект</param>
  /// <param name="propertyName">имя свойства</param>
  /// <param name="bindingFlags">условие поиска свойства</param>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="Exception"></exception>
  /// <returns>значение свойства</returns>
  public static object GetPropertyValue(this object obj, string propertyName,
                                        BindingFlags bindingFlags) {
    Type type = obj.GetType();
    PropertyInfo propertyInfo = obj.GetProperty(propertyName, bindingFlags);

    if (propertyInfo == null) {
      ExceptionGenerator.Run<NullReferenceException>("Ошибка при получении свойства с именем \"{0}\" объекта типа \"{1}\"",
          propertyName, type);
    }

    return obj.GetPropertyValue(propertyInfo, AccessAttr.Public);
  }

  #endregion

  #region Получение значение свойства

  public static object GetPropertyValue(this object obj, PropertyInfo propertyInfo) {
    return obj.GetPropertyValue(propertyInfo, AccessAttr.All);
  }

  public static object GetPropertyValue(this object obj, PropertyInfo propertyInfo, bool nonPublic) {
    return obj.GetPropertyValue(propertyInfo, nonPublic ? AccessAttr.NonPublic : AccessAttr.Public);
  }

  /// <summary>
  /// Получить значение свойства propertyInfo у объекта
  /// </summary>
  /// <param name="obj">объект</param>
  /// <param name="propertyInfo">свойство</param>
  /// <param name="accessAttr">доступ к селектору свойства</param>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="Exception"></exception>
  /// <returns>значение свойства</returns>
  public static object GetPropertyValue(this object obj, PropertyInfo propertyInfo,
                                        AccessAttr accessAttr) {
    Type type = obj.GetType();

    if (propertyInfo == null) {
      ExceptionGenerator.Run<ArgumentNullException>("Ошибка при получении значения свойства: свойство объекта типа \"{0}\" неопределено",
          type);
    }

    try {
      MethodInfo method = null;
      if (accessAttr == AccessAttr.All) {
        method = propertyInfo.GetGetMethod();
        if (method == null) {
          method = propertyInfo.GetGetMethod(true);
        }
        return method.Invoke(obj, null);
      }
      method = propertyInfo.GetGetMethod(accessAttr == AccessAttr.NonPublic);
      return method.Invoke(obj, null);
    }
    catch (Exception ex) {
      throw ExceptionGenerator.Run(ex,
                                   "Ошибка при получении значения свойства \"{0}\" объекта типа \"{1}\"",
                                   propertyInfo.Name, type);
    }
  }

  #endregion

  #region Установка значения свойства по его имени

  public static void SetPropertyValue(this object obj, string propertyName, object value) {
    obj.SetPropertyValue(propertyName, value, BindingFlags.Public | BindingFlags.Instance);
  }

  public static void SetFlattenPropertyValue(this object obj, string propertyName, object value) {
    obj.SetPropertyValue(propertyName, value,
                         BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
  }

  public static void SetStaticPropertyValue(this object obj, string propertyName, object value) {
    obj.SetPropertyValue(propertyName, value, BindingFlags.Public | BindingFlags.Static);
  }

  public static void SetFlattenStaticPropertValue(this object obj, string propertyName,
      object value) {
    obj.SetPropertyValue(propertyName, value,
                         BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
  }

  /// <summary>
  /// Установить значение свойства propertyName у объекта
  /// </summary>
  /// <param name="obj">объект</param>
  /// <param name="propertyName">имя свойства</param>
  /// <param name="bindingFlags">условие поиска свойства</param>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="Exception"></exception>
  /// <returns></returns>
  public static void SetPropertyValue(this object obj, string propertyName, object value,
                                      BindingFlags bindingFlags) {
    PropertyInfo propertyInfo = obj.GetProperty(propertyName, bindingFlags);
    obj.SetPropertyValue(propertyInfo, value, AccessAttr.Public);
  }

  /// <summary>
  /// Установить значение свойства propertyName у объекта
  /// </summary>
  /// <param name="obj">объект</param>
  /// <param name="propertyName">имя свойства</param>
  /// <param name="bindingFlags">условие поиска свойства</param>
  /// <param name="accessAttr">доступ к селектору свойства</param>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="Exception"></exception>
  /// <returns></returns>
  public static void SetPropertyValue(this object obj, string propertyName, object value,
                                      BindingFlags bindingFlags, AccessAttr accessAttr) {
    PropertyInfo propertyInfo = obj.GetProperty(propertyName, bindingFlags);
    obj.SetPropertyValue(propertyInfo, value, accessAttr);
  }

  #endregion

  #region Установка значения свойства

  public static void SetPropertyValue(this object obj, PropertyInfo propertyInfo, object value,
                                      bool nonPublic) {
    obj.SetPropertyValue(propertyInfo, value, nonPublic ? AccessAttr.NonPublic : AccessAttr.Public);
  }

  public static void SetPropertyValue(this object obj, PropertyInfo propertyInfo, object value) {
    obj.SetPropertyValue(propertyInfo, value, AccessAttr.All);
  }

  /// <summary>
  /// Установить значение свойства propertyName у объекта
  /// </summary>
  /// <param name="obj">объект</param>
  /// <param name="propertyInfo">свойство</param>
  /// <param name="accessAttr">доступ к селектору свойства</param>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="Exception"></exception>
  /// <returns></returns>
  public static void SetPropertyValue(this object obj, PropertyInfo propertyInfo, object value,
                                      AccessAttr accessAttr) {
    Type type = obj.GetType();

    if (propertyInfo == null) {
      ExceptionGenerator.Run<ArgumentNullException>("Ошибка при установке значения свойства: свойство объекта типа \"{0}\" неопределено",
          type);
    }

    try {
      if (accessAttr == AccessAttr.All) {
        MethodInfo method = propertyInfo.GetSetMethod();
        if (method == null) {
          method = propertyInfo.GetSetMethod(true);
        }
        object[] args = new object[1] { value };
        method.Invoke(obj, args);
      }
      else {
        MethodInfo method = propertyInfo.GetSetMethod(accessAttr == AccessAttr.NonPublic);
        object[] args = new object[1] { value };
        method.Invoke(obj, args);
      }
    }
    catch (Exception ex) {
      throw ExceptionGenerator.Run(ex,
                                   "Ошибка при установке значения свойства \"{0}\" объекта типа \"{1}\"",
                                   propertyInfo.Name, type);
    }
  }

  #endregion

  #endregion

  #region Выполнение метода

  public static object GetMethodValue(this object obj, string methodName, params object[] args) {
    return obj.GetMethodValue(methodName, BindingFlags.Public | BindingFlags.Instance, args);
  }

  public static object GetFlattenMethodValue(this object obj, string methodName) {
    return obj.GetMethodValue(methodName,
                              BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
  }

  public static object GetStaticMethodValue(this object obj, string methodName) {
    return obj.GetMethodValue(methodName, BindingFlags.Public | BindingFlags.Static);
  }

  public static object GetFlattenStaticMethodValue(this object obj, string methodName) {
    return obj.GetMethodValue(methodName,
                              BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
  }

  /// <summary>
  /// Выполнить метод methodName у объекта
  /// </summary>
  /// <param name="obj">объект</param>
  /// <param name="methodName">имя метода</param>
  /// <param name="bindingFlags">условие поиска метода</param>
  /// <param name="args">аргументы выполняемого метода</param>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="Exception"></exception>
  /// <returns></returns>
  public static object GetMethodValue(this object obj, string methodName, BindingFlags bindingFlags,
                                      params object[] args) {
    if (obj == null) {
      ExceptionGenerator.Run<ArgumentNullException>("Аргумент \"obj\" равен null");
    }

    if (String.IsNullOrEmpty(methodName)) {
      ExceptionGenerator.Run<ArgumentNullException>("Аргумент \"methodName\" равен null или пустой");
    }

    Type type = obj.GetType();

    try {
      MethodInfo methodInfo = null;
      methodInfo = type.GetMethod(methodName, bindingFlags);
      if (methodInfo == null) {
        ExceptionGenerator.Run<ArgumentNullException>("Метод \"{0}\" в объекте типа \"{1}\" не найдено по условиям поиска \"{2}\"",
            methodName, type, bindingFlags.ToString());
      }

      return methodInfo.Invoke(obj, args);
    }
    catch (Exception ex) {
      throw ExceptionGenerator.Run(ex,
                                   "Ошибка при выполнения метода \"{0}\" объекте типа \"{1}\"",
                                   methodName, type);
    }
  }

  #endregion

  /// <summary>
  /// Получить вложенный тип
  /// </summary>
  /// <param name="obj">объект, у которого получает вложенный тип</param>
  /// <param name="nestedTypeName">имя вложенного типа</param>
  /// <param name="bindingFlags">условие поиска вложенного типа</param>
  /// <returns>тип вложенного объекта</returns>
  public static Type GetNestedType(this object obj, string nestedTypeName,
                                   BindingFlags bindingFlags) {
    if (obj == null) {
      ExceptionGenerator.Run<NullReferenceException>("Аргумент \"obj\" равен null");
    }

    if (String.IsNullOrEmpty(nestedTypeName)) {
      ExceptionGenerator.Run<ArgumentNullException>("Аргумент \"nestedTypeName\" равен null или пустой метода объекта типа {0}",
          obj.GetType());
    }

    Type type = obj.GetType();

    try {
      Type nestedType = type.GetNestedType(nestedTypeName, bindingFlags);
      return nestedType;
    }
    catch (Exception ex) {
      throw ExceptionGenerator.Run(ex,
                                   "Ошибка при получении вложенного типа \"{0}\" объекта типа \"{1}\"",
                                   nestedTypeName, type);
    }
  }

  /// <summary>
  /// Получить вложенный тип
  /// </summary>
  /// <param name="obj">объект, у которого получает вложенный тип</param>
  /// <param name="nestedTypeName">имя вложенного типа</param>
  /// <param name="accessAttr">доступ к вложенному типу</param>
  /// <returns></returns>
  public static Type GetNestedType(this object obj, string nestedTypeName, AccessAttr accessAttr) {
    Type nestedType = obj.GetNestedType(nestedTypeName,
                                        accessAttr == AccessAttr.NonPublic ? BindingFlags.NonPublic : BindingFlags.Public);
    if (nestedType == null) {
      if (accessAttr == AccessAttr.All) {
        return obj.GetNestedType(nestedTypeName, BindingFlags.NonPublic);
      }
      return null;
    }
    return nestedType;
  }
}
}

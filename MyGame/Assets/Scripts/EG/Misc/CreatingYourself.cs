using System;

using EG.Kernel;

namespace EG.Misc {
/// <summary>
/// Класс, умеющий создавать самого себя
/// </summary>
/// <typeparam name="T">тип создаваемого класса</typeparam>
public class CreatingYourself<T> : IBaseObject
  where T : CreatingYourself<T>, new() {
  /// <summary>
  /// Ссылка на экземпляр класса
  /// </summary>
  private static T _instance;

  /// <summary>
  /// Создать экземпляр класса.
  /// </summary>
  public static void Create() {
    _instance = new T();
  }

  /// <summary>
  /// Создать экземпляр класса.
  /// </summary>
  /// <param name="arg1">объект</param>
  public static void Create(object arg1) {
    object[] args = new object[1] { arg1 };
    _instance = (T)Activator.CreateInstance(typeof(T), args);
  }

  /// <summary>
  /// Создать экземпляр класса.
  /// </summary>
  /// <param name="arg1">объект</param>
  /// <param name="arg2">объект</param>
  public static void Create(object arg1, object arg2) {
    object[] args = new object[2] { arg1, arg2 };
    _instance = (T)Activator.CreateInstance(typeof(T), args);
  }

  /// <summary>
  /// Создать экземпляр класса.
  /// </summary>
  /// <param name="arg1">объект</param>
  /// <param name="arg2">объект</param>
  /// <param name="arg3">объект</param>
  public static void Create(object arg1, object arg2, object arg3) {
    object[] args = new object[3] { arg1, arg2, arg3};
    _instance = (T)Activator.CreateInstance(typeof(T), args);
  }

  /// <summary>
  /// Создать экземпляр класса.
  /// </summary>
  /// <param name="args">массив аргументов</param>
  public static void Create(params object[] args) {
    _instance = (T)Activator.CreateInstance(typeof(T), args);
  }

  /// <summary>
  /// Получить ссылку на экземпляр класса.
  /// </summary>
  /// <returns>Ссылка на экземпляр класса</returns>
  public static T GetInstance() {
    if (_instance == null) {
      Create();
    }
    return _instance;
  }

  /// <summary>
  /// Удалить экземпляр класса.
  /// </summary>
  public static void Delete() {
    _instance = default(T);
  }
}
}

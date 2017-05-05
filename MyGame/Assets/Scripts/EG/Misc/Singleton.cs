using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace EG.Misc {
/// <summary>
/// Шаблон для "превращения" класса в одиночку
/// </summary>
/// <typeparam name="T">Тип класса одиночки</typeparam>
public class Singleton<T> where T : new() {
  /// <summary>
  /// Создать единственный экземпляр класса.
  /// </summary>
  public static void Create() {
    Debug.Assert(_instance == null, "_instance == null",
                 "Ожидается, что _instance будет null");

    if (_instance != null) {
      ExceptionGenerator.Run<DuplicateNameException>("Ожидается, что _instance типа {0} будет null",
          typeof(T));
    }

    InternalCreate();
  }

  /// <summary>
  /// Внутренний метод для создания экземпляра класса.
  /// </summary>
  private static void InternalCreate() {
    DebugLogger.WriteMessage(LogZone.Verbose,
                             "Создание нового экземпляра класса синглетона типа {0}",
                             typeof(T).ToString());

    _instance = new T();
  }

  /// <summary>
  /// Удалить единственный экземпляр класса.
  /// </summary>
  public static void Delete() {
    Debug.Assert(_instance != null, "_instance != null",
                 "Ожидается, что _instance будет не null");

    if (_instance == null) {
      ExceptionGenerator.Run<DuplicateNameException>("Ожидается, что _instance типа {0} будет null",
          typeof(T));
    }

    InternalDelete();
  }

  /// <summary>
  /// Внутренний метод для удаления экземпляра класса.
  /// </summary>
  private static void InternalDelete() {
    DebugLogger.WriteMessage(LogZone.Verbose,
                             "Удаление экземпляра класса синглетона типа {0}",
                             typeof(T).ToString());

    _instance = default(T);
  }

  /// <summary>
  /// Получить ссылку на единственный экземпляр класса.
  /// </summary>
  /// <returns>Ссылка на экземпляр класса</returns>
  public static T GetInstance() {
    if (_instance == null) {
      InternalCreate();
    }

    return _instance;
  }

  /// <summary>
  /// Ссылка на единственный экземпляр класса // really ? wtf !!
  /// </summary>
  private static T _instance;
}

/// <summary>
/// Шаблон для "превращения" класса в одиночку
/// </summary>
/// <typeparam name="T">Тип класса одиночки</typeparam>
public class NamedSingleton<T> : Singleton<T> where T : new() {
  public static string Name {
    get { return (_name == null ? String.Empty : _name); }
    set { _name = (value == null ? String.Empty : value); }
  }
  private static string _name = String.Empty;
}

public class DuplicateNameException : ApplicationException {
  public DuplicateNameException()
  : base("Дубликат объекта") {

  }

  public DuplicateNameException(string message)
  : base(message) {

  }

  public DuplicateNameException(SerializationInfo info, StreamingContext context)
  : base(info, context) {

  }

  public DuplicateNameException(string message, Exception innerException)
  : base(message, innerException) {

  }
}
}

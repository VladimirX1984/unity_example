namespace EG.Kernel {
/// <summary>
/// Интерфейс, от которого реализуются объекты с идентификаторами
/// </summary>
public interface IBaseIDObject : IBaseObject {
  object Id { get; }
}

/// <summary>
/// Интерфейс, от которого реализуются объекты с идентификаторами
/// </summary>
public interface IBaseIDObject<T> : IBaseIDObject {
  new T Id { get; }
}

/// <summary>
/// Интерфейс, от которого реализуются объекты с изменяемыми идентификаторами, с именами
/// </summary>
public interface IBaseIDObjectEx<T> : IBaseIDObject<T> {
  void SetId(T id);
}
}

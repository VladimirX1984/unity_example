namespace EG.Kernel {
/// <summary>
/// Интерфейс, от которого реализуются все конвертируемые объекты
/// </summary>
public interface IConvertableObject {
  object ToObject();
}

/// <summary>
/// Интерфейс, от которого реализуются все конвертируемые объекты
/// </summary>
public interface IConvertableObject<T> {
  T ToObject();
}
}

namespace EG.Containers {
/// <summary>
/// Интерфейс, от которого реализуются все группы, содержащие один элемент
/// </summary>
public interface IGroupOfOne : IBaseGroup {
  object Element { get; }
}

/// <summary>
/// Интерфейс, от которого реализуются все группы, содержащие один элемент
/// </summary>
public interface IGroupOfOne<TItem> : IGroupOfOne {
  new TItem Element { get; }
}
}

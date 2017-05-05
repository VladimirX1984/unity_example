namespace EG.Kernel {
/// <summary>
/// Интерфейс, от которого реализуются объекты с именами
/// </summary>
public interface INamedObject : IBaseObject {
  string Name { get; set; }
}
}

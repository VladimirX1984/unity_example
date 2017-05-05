using System;

namespace EG.Kernel {
/// <summary>
/// Интерфейс, от которого реализуются все объекты, имеющие обработчик событий изменения какого-то параметра объекта
/// </summary>
public interface IParamObject : IBaseObject {
  /// <summary>
  /// обработчик событий: 1-ый аргумент - имя измененного параметра, 2-ой аргумент - его старое значение, 3-ой аргумент - его новое значение
  /// </summary>
  Action<string, object, object> OnParamChanged { get; set; }
}
}

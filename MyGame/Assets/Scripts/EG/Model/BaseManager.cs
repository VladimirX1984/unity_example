using System;

using EG.Objects;

namespace EG.Model {
/// <summary>
/// Базовая реализация менеджера
/// </summary>
public class BaseManager : BaseIDObject<float>, IManager {
  public BaseManager(string name)
  : base(name) {

  }

  public BaseManager(string name, float id)
  : base(name, id) {

  }
}

[Serializable]
public class BaseSerializableManager : BaseIDSerializableObject<float> {
  public BaseSerializableManager(string name)
  : base(name) {

  }

  public BaseSerializableManager(string name, float id)
  : base(name, id) {

  }
}
}

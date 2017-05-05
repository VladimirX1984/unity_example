using System;

namespace EG.Objects {
[Serializable]
public class BaseIDSerializableObject<TIdType> : BaseIDObject<TIdType> {

  public BaseIDSerializableObject()
  : base() {

  }


  public BaseIDSerializableObject(string name)
  : base(name) {

  }

  public BaseIDSerializableObject(string name, TIdType id)
  : base(name, id) {

  }
}
}

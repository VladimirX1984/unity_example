using System.Collections.Generic;

namespace EG.InputTouch {

public interface IInput3D {

  IEnumerable<Touch3D> Touches { get; }

  bool NotifyTouchedObjects { get; set; }

  void Update();

}
}
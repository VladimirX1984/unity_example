using System;
using EG.Objects;

namespace EG.Model {
public class BaseMonoManager : BaseMonoObject, IManager {

  public BaseMonoManager(string name) : base() {
    _name = name;
  }

  #region MonoBehaviour Events

  // Use this for initialization
  protected override void Start() {
    base.Start();
    if (!String.IsNullOrEmpty(_name)) {
      name = _name;
    }
  }

  #endregion

  private string _name = String.Empty;
}
}

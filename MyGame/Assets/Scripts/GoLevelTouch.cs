using UnityEngine.UI;

using EG;
using EG.Objects;

public class GoLevelTouch : BaseMonoObject {

  public void Press() {
    DebugLogger.WriteInfo("GoLevelTouch.Press _levelNumber = {0}", _levelNumber);
    GameManager.Instance.StartLevel(_levelNumber);
  }

  public Button.ButtonClickedEvent Init(int levelNumber) {
    _levelNumber = levelNumber;
    Button.ButtonClickedEvent click = new Button.ButtonClickedEvent();
    click.AddListener(Press);
    return click;
  }

  private int _levelNumber;
}

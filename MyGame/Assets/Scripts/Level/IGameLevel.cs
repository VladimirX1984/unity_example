using UnityEngine.Events;

using EG.Kernel;

public interface IGameLevel : IBaseMonoObject {

  int Number { get; set; }

  void Begin();

  void Exit();

  event UnityAction OnLevelWin;
}

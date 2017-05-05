using UnityEngine;

using EG.Objects;

namespace EG.Players {
  public interface IPlayer : IGameObject {

    int Score { get; set; }

    bool IsAnyDir { get; set; }

    void Movement(Vector2 vec);
  }
}

using UnityEngine;
using System;

using EG.Kernel;

namespace EG.Objects {

  public delegate void ActionOnObject(IGameObject obj, int reason);

  public interface IGameObject : IBaseIDObject<float>, INamedObject, IParamObject,
    INotificationObject {

    int TypeId { get; }

    float Health { get; }

    float Speed { get; set; }

    float Acceleration { get; set; }

    int DiscreteDirection { get; }

    bool IsBonus { get; set; }

    bool IsControlable { get; set; }

    bool IsShield { get; set; }

    bool IsAlive { get; }

    float HitForce { get; set; }

    int Level { get; set; }

    // взято из Component
    GameObject gameObject { get; }

    // взято из Behaviour
    bool enabled { get; set; }

    void AddObserver(ActionOnObject diedObject);

    void RemoveObserver(ActionOnObject diedObject);

    Vector2 GetMoveDirection();

    void SetMoveDirection(float x, float y);

    void Move();

    void StopMove();

    void SetPosition(float x, float y);

    void SetPosition(Vector2 pos);

    float Hit(float damage);

    void Attack();

    void Attack(Vector2 vec);

    void Reset();

    void SetColliderTrigger(bool bValue);

    event Action<Vector2> OnMoved;
    event Action<Vector2> OnPosSetted;
  }
}

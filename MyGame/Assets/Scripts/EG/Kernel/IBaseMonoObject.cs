namespace EG.Kernel {
public interface IBaseMonoObject : IBaseIDObject<float>, IParamObject, INotificationObject {
}

public interface IBaseMonoObject<TIdType> : IBaseIDObject<TIdType>, IParamObject,
  INotificationObject {
}
}

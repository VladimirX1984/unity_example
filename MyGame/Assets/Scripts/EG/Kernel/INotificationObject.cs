using EG.Notifications;

namespace EG.Kernel {
/// <summary>
/// Интерфейс, от которого реализуются все объекты, которые могут отправлять уведомления асинхронной системе уведомлений
/// </summary>
public interface INotificationObject : IBaseObject, IBaseNotifier {
  bool IsNotificationSended { get; set; }

  void SendNotification(int what, params object[] notificationArgs);
}
}

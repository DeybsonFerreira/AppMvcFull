using AppMvcFull.Business.Models;
using System.Collections.Generic;

namespace AppMvcFull.Business.Interfaces
{
    public interface INotification
    {
        bool HasNotification();
        List<NotificationModel> GetNotifications();
        void Handle(NotificationModel model);
    }
}

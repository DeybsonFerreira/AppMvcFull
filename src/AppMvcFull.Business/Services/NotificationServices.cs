using AppMvcFull.Business.Interfaces;
using AppMvcFull.Business.Models;
using System.Collections.Generic;
using System.Linq;

namespace AppMvcFull.Business.Services
{
    public class NotificationServices : INotification
    {
        protected List<NotificationModel> _notifications;
        public NotificationServices()
        {
            _notifications = new List<NotificationModel>();
        }

        public List<NotificationModel> GetNotifications()
        {
            return _notifications;
        }

        public void Handle(NotificationModel model)
        {
            _notifications.Add(model);
        }

        public bool HasNotification()
        {
            return _notifications.Any();
        }
    }
}

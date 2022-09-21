using AppMvcFull.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppMvcFull.App.Extensions
{
    public class BaseController: Controller
    {
        private readonly INotification _notification;

        protected BaseController(INotification notification)
        {
            _notification = notification;
        }

        protected bool CheckValidation()
        {
            return !_notification.HasNotification();
        }
    }
}

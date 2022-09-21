using AppMvcFull.Business.Interfaces;
using AppMvcFull.Business.Models;
using FluentValidation;
using FluentValidation.Results;

namespace AppMvcFull.Business.Services
{
    public abstract class BaseServices
    {

        private readonly INotification _notification;

        protected BaseServices(INotification otification)
        {
            _notification = otification;
        }

        protected void Notify(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notify(error.ErrorMessage);
            }
        }

        protected void Notify(string mensagem)
        {
            _notification.Handle(new NotificationModel(mensagem));
        }

        protected bool ExecuteValidation<TV, TE>(TV validation, TE model) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validator = validation.Validate(model);

            if (validator.IsValid) return true;

            Notify(validator);

            return false;
        }
    }
}

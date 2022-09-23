using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AppMvcFull.App.Configuration
{
    public static class CustomMvcConfiguration
    {
        public static IServiceCollection AddCustomMvcConfiguration(this IServiceCollection services)
        {
            services.AddMvc(option =>
            {
                option.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => "O Campo deve ser numérico");
                option.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "O Campo deve ser numérico");
                AddValidateAntiForgeryToken(ref option);
            });
            return services;
        }

        /// <summary>
        /// Prevenção de ataques Cross-site Request Forgery (CSRF)
        /// </summary>
        /// <param name="option"></param>
        private static void AddValidateAntiForgeryToken(ref MvcOptions option)
        {
            option.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); //ValidateAntiForgeryToken em todos os controllers
        }
    }
}

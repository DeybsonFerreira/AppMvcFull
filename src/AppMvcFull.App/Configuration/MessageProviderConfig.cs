using Microsoft.Extensions.DependencyInjection;

namespace AppMvcFull.App.Configuration
{
    public static class MessageProviderConfig
    {
        public static IServiceCollection SetCustomLocalization(this IServiceCollection services)
        {
            services.AddMvc(option =>
            {
                option.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => "O Campo deve ser numérico");
                option.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "O Campo deve ser numérico");

            });
            return services;
        }
    }
}

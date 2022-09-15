using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;
using System.Globalization;

namespace AppMvcFull.App.Configuration
{
    public static class GlobalizationConfig
    {
        public static WebApplication SetCustomLocalization(this WebApplication app)
        {
            CultureInfo defaultCulture = new("pt-BR");
            CultureInfo enUsCulture = new("en-US");

            var localizationOptions = new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo>() { defaultCulture, enUsCulture },
                SupportedUICultures = new List<CultureInfo>() { defaultCulture, enUsCulture },
            };
            app.UseRequestLocalization(localizationOptions);
            return app;
        }
    }
}

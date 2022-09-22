using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace AppMvcFull.App.Extensions.TagHelpers
{
    [HtmlTargetElement("*", Attributes = "customHide-value")]
    [HtmlTargetElement("*", Attributes = "customHide-name")]
    public class CustomHideByAccessTagHelper : TagHelper
    {
        [HtmlAttributeName("customHide-value")]
        public string IdentityClaimValue { get; set; }
        [HtmlAttributeName("customHide-name")]
        public string IdentityClaimName { get; set; }

        private readonly IHttpContextAccessor _contextAccessor;

        public CustomHideByAccessTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            bool hasAccess = CustomAuthorization.ValidUserClaim(_contextAccessor.HttpContext, IdentityClaimName, IdentityClaimValue);

            if (hasAccess)
                return;//não cria

            output.SuppressOutput();
        }
    }
}

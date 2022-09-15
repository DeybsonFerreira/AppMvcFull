using AppMvcFull.Business.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using System;

namespace AppMvcFull.App.Extensions
{
    public static class RazorExtensions
    {
        public static string FormatDocument(this RazorPage page, SupplierType type, string documentNumber)
        {
            if (type == SupplierType.Natural)
            {
                return Convert.ToUInt64(documentNumber).ToString(@"000\.000\.000\-00");
            }
            else if (type == SupplierType.Legal)
            {
                return Convert.ToUInt64(documentNumber).ToString(@"00\.000\.000\/000\-00");
            }
            else
            {
                return documentNumber;
            }
        }
    }
}

using System.Web;
using System.Web.Mvc;

namespace Local_Theatre_Company_V1._0
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

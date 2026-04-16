using League_of_Legends_Tournament_Hosting.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace League_of_Legends_Tournament_Hosting.Controllers
{
    public abstract class AppControllerBase : Controller
    {
        protected void SetBreadcrumbs(params BreadcrumbItem[] items)
        {
            ViewData["Breadcrumbs"] = items.ToList();
        }
    }
}
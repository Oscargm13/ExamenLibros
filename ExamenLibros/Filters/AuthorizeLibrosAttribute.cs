using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ExamenLibros.Filters
{
    public class AuthorizeLibrosAttribute: AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            string controller =
                context.RouteData.Values["controller"].ToString();
            string action =
                context.RouteData.Values["action"].ToString();
            var id = context.RouteData.Values["id"];

            //Para comprobar si funciona, dibujamos en consola
            Debug.WriteLine("Controller: " + controller);
            Debug.WriteLine("Action: " + action);
            Debug.WriteLine("Id: " + id);
            ITempDataProvider provider =
                context.HttpContext.RequestServices
                .GetService<ITempDataProvider>();

            //Esta clase contiene en su interior el tempdata de nuestra app
            //Recuperamos el tempdata de nuestra app
            var TempData = provider.LoadTempData(context.HttpContext);

            //Guardamos la informacion en tempdata
            TempData["controller"] = controller;
            TempData["action"] = action;
            if (id != null)
            {
                TempData["id"] = id.ToString();
            }
            else
            {
                //Eliminamos la key para que no aparezca en 
                //Nuestra ruta 
                TempData.Remove("id");
            }
            //Volvemos a guardar los cambios de este tempdata en la app
            provider.SaveTempData(context.HttpContext, TempData);

            if (user.Identity.IsAuthenticated == false)
            {
                context.Result = this.GetRoute("Managed", "Login");
            }
        }

        private RedirectToRouteResult GetRoute(string controller, string action)
        {
            RouteValueDictionary ruta =
            new RouteValueDictionary(
            new { controller = controller, action = action }
            );
            RedirectToRouteResult result =
            new RedirectToRouteResult(ruta);
            return result;
        }
    }
}

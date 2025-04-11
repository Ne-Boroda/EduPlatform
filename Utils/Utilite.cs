using EduPlatform.Contexts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduPlatform.Utils
{
    public class Utilite
    {
        public static void SetViewBag(Controller controller)
        {
            controller.ViewBag.IsSign = false;
            controller.ViewBag.Role = "";

            if (controller.HttpContext != null && controller.HttpContext.User != null && controller.HttpContext.User.Identity != null)
            {
                bool isSign = controller.HttpContext.User.Identity.IsAuthenticated;
                // Флаг авторизации.
                controller.ViewBag.IsSign = isSign;

                if (isSign)
                {
                    var db = new ContextFactory().CreateDbContext(new string[] { });
                    var roles = db.Roles;
                    var userId = controller.HttpContext.User.Identity.GetUserId();
                    var roleId = db.UserRoles // Id роли.
                        .Where(r => r.UserId == userId)
                        .Select(uu => uu.RoleId).First();


                    var roleName = db.Roles.Where(r => r.Id == roleId).First().Name;

                    controller.ViewBag.Role = roleName;
                }
            }
        }
    }
}

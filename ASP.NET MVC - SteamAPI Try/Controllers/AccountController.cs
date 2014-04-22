using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PortableSteam;

namespace ASP.NET_MVC___SteamAPI_Try.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        private void SteamID()
        {
            SteamWebAPI.SetGlobalKey("CFDCF16D4EC9D68762FBE9C61B43892D");
            
            /*long steamID = await GetSteamIDByName("gabelogannewell");
            if (steamID != 0)
            {
                SteamProfile profile = await GetSteamProfileById(steamID);
            }*/
            var user = User.Identity.Name;
            long id = Convert.ToInt64(user);
            var identity = SteamIdentity.FromSteamID(id);
            var accountID = identity.SteamID;
            var r = SteamWebAPI.General().ISteamUser().ResolveVanityURL("munchies").GetResponse();
            ViewData["UserID"] = accountID;
        }

        public ActionResult Index()
        {
            AntiCache();
            SteamID();
            return View();
        }

        public ActionResult Signout()
        {
            AntiCache();
            /*HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoServerCaching();
            HttpContext.Current.Response.Cache.SetNoStore();*/
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Remove("AUTHCOOKIE");
            FormsAuthentication.SignOut();
            return View();
        }

        private void AntiCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.RelyingParty;
using ASP.NET_MVC___SteamAPI_Try.Models;
using System.Web.Security;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using Owin.Security.Providers;
using System.Text.RegularExpressions;



namespace ASP.NET_MVC___SteamAPI_Try.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        //STEAM_0:0:68926576
        //SteamWebAPI.SetGlobalKey("CFDCF16D4EC9D68762FBE9C61B43892D");
        //vmykel
        //wheniwasyourman
        
        public ActionResult Index()
        {
            OpenIdRelyingParty openid = new OpenIdRelyingParty();
            OpenIdLogin openlog = new OpenIdLogin();
            openid.SecuritySettings.AllowDualPurposeIdentifiers = true;
            IAuthenticationResponse response = openid.GetResponse();

            if (response != null)
            {
                string regex = Request.QueryString["openid.identity"];
                //string youareel= "http://steamcommunity.com/openid/id/76561198131281243";
                string[] str = regex.Split('/');
                string id = str[5];
                Session["user"] = id;
                FormsAuthentication.SetAuthCookie(id, false);
                if(!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index");
                }
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index","Account");
                }
            }

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Account");
            }
            
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(LoginModel log)
        {
            OpenIdRelyingParty openid = new OpenIdRelyingParty();
            try
            {
                IAuthenticationRequest request = openid.CreateRequest("http://steamcommunity.com/openid");
                OpenIdLogin openlog = new OpenIdLogin();
                openid.SecuritySettings.AllowDualPurposeIdentifiers = true;
                IAuthenticationResponse response = openid.GetResponse();
                return request.RedirectingResponse.AsActionResult();
            }
            catch(Exception e)
            {
                ModelState.AddModelError(e.Message, "Cannot Fetch Steam Credentials. Please try again Later");
            }

            return View(log);
        }

        public ActionResult Signout()
        {
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Remove("AUTHCOOKIE");
            FormsAuthentication.SignOut();
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

    }
}

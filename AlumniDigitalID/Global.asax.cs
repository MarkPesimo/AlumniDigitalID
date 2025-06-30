using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using ZMGModel.ViewModel.ALUMNI.User;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.User_model;

namespace AlumniDigitalID
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            try
            {
                if (authCookie != null)
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    LoginUser_model serializeModel = serializer.Deserialize<LoginUser_model>(authTicket.UserData);

                    if (serializeModel != null)
                    {
                        ContextLoginUser_model newUser = new ContextLoginUser_model(authTicket.Name);
                        newUser.UserId = serializeModel.UserId;
                        newUser.Username = serializeModel.Username;
                        newUser.UserType = serializeModel.UserType;
                        newUser.StudentName = serializeModel.StudentName;
                        newUser.SchoolId = serializeModel.SchoolId;
                        newUser.NavLogo = serializeModel.NavLogo;

                        HttpContext.Current.User = newUser;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}

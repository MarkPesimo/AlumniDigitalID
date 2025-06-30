
using Alumni.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static AlumniDigitalID.Models.misc_model;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.User_model;

namespace AlumniDigitalID.Controllers
{
    public class AccountController : Controller
    {
        private GlobalRepository _globalrepository { get; set; }
        private UserRepository _userrepository { get; set; }
        //private userrepositroy _
        private string _Settings_Index = "~/Views/Account/Settings.cshtml";

        public AccountController() 
        {
            if (_globalrepository == null) { _globalrepository = new GlobalRepository(); }
            if (_userrepository == null) { _userrepository = new UserRepository(); }
            //if (_accountrepository == null) { _accountrepository = new AccountRepository(); }
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Pin(string _guid)
        {
            Pin_Validity _pinstatus = _userrepository.IsValidGUID(_guid);
            if (_pinstatus.PinStatus == false) { return View("PinNotFound"); }

            Pin_model model = new Pin_model
            {
                GuidId = _guid,
                SchoolId = AlumniConstant.SchoolId,
                PinNumber = "",
                AlumniLogo = _pinstatus.AlumniLogo,
                AlumniName = _pinstatus.AlumniName
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Pin(Pin_model _model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    LoginResult _login_result = _userrepository.Pin(_model);
                    if (_login_result.Result == "Ok")
                    {
                        string _urlreferrer = Request.UrlReferrer.ToString();
                        int _index = _urlreferrer.IndexOf("ReturnUrl");
                        string _redirecttourl = "";
                        if (_index > 0)
                        {
                            _redirecttourl = _urlreferrer.Substring(_index + 10, (_urlreferrer.Length - (_index + 10)));
                        }

                        if (_redirecttourl != "")
                        {
                            _redirecttourl = _redirecttourl.Replace("%2F", "/");
                            _redirecttourl = _redirecttourl.Replace("%3F", "?");
                            _redirecttourl = _redirecttourl.Replace("%3D", "=");
                            //return Redirect(_redirecttourl);
                            return Json(new { Result = "Success",
                                UserId = _login_result.UserId,
                                URL = _redirecttourl });
                        }
                        else
                        {
                            return Json(new { Result = "Success",
                                UserId = _login_result.UserId,
                                URL = "/Alumni/Index" });
                        }
                    }
                    else
                    {
                        return Json(new { Result = "ERROR",
                            Message = _login_result.Result,
                            ElementName = "PinNumber" });
                    }
                }

                List<string> _errors = _globalrepository.GetModelErrors(ModelState);
                return Json(new { Result = "ERROR", Message = _errors[1], ElementName = _errors[0] });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            Login_model model = new Login_model
            {
                SchoolId = AlumniConstant.SchoolId
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult LogLogin(int _userid, string _longitude, string _latitude)
        {
            LoginLog_model _model = new LoginLog_model
            {
                UserId = _userid,
                Longitude = _longitude,
                Latitude = _latitude
            };

            bool _result = _userrepository.LogLogin(_model);
            return Json(new { Result = _result });
        }

        [HttpPost]
        public ActionResult Login(Login_model model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    LoginResult _login_result = _userrepository.Login(model);
                    if (_login_result.Result == "Ok")
                    {
                        string _urlreferrer = Request.UrlReferrer.ToString();
                        int _index = _urlreferrer.IndexOf("ReturnUrl");
                        string _redirecttourl = "";
                        if (_index > 0)
                        {
                            _redirecttourl = _urlreferrer.Substring(_index + 10, (_urlreferrer.Length - (_index + 10)));
                        }

                        if (_redirecttourl != "")
                        {
                            _redirecttourl = _redirecttourl.Replace("%2F", "/");
                            _redirecttourl = _redirecttourl.Replace("%3F", "?");
                            _redirecttourl = _redirecttourl.Replace("%3D", "=");
                            //return Redirect(_redirecttourl);
                            return Json(new { Result = "Success",
                                UserId = _login_result.UserId,
                                URL = _redirecttourl });
                        }
                        else
                        {
                            return Json(new { Result = "Success",
                                UserId = _login_result.UserId,
                                URL = "/Alumni/Index" });
                        }
                    }
                    else
                    {
                        return Json(new { Result = "ERROR",
                            Message = _login_result.Result,
                            ElementName = "Username" });
                    }
                }

                List<string> _errors = _globalrepository.GetModelErrors(ModelState);
                return Json(new { Result = "ERROR", Message = _errors[1], ElementName = _errors[0] });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
            Session.Abandon();

            //return Json(new { Result = "Success" }, JsonRequestBehavior.AllowGet);
            Login_model model = new Login_model();
            model.SchoolId = 1;
            return View("~/Views/Account/Login.cshtml", model);
        }

        //[Authorize]
        //[HttpGet]
        //public ActionResult Settings()
        //{
        //    return View(_Settings_Index);
        //}
    }
}
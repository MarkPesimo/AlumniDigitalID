using Alumni.Repository;
using AlumniDigitalID.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.Settings_model;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.User_model;

namespace AlumniDigitalID.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private GlobalRepository _globalrepository { get; set; }
        private UserRepository _userrepository { get; set; }
        private SettingsRepository _settingsrepository { get; set; }

        private int _loginuserid = 0;
        public string _usertype = "User";

        public SettingsController()
        {
            if (_globalrepository == null) { _globalrepository = new GlobalRepository(); }
            if (_userrepository == null) { _userrepository = new UserRepository(); }
            if (_settingsrepository == null) { _settingsrepository = new SettingsRepository(); }

            if (_loginuserid == 0)
            {
                LoginUser_model _model = _userrepository.GetLoginUser();
                if (_model != null)
                {
                    _loginuserid = _model.UserId;
                    _usertype = _model.UserType;
                }
            }
        }

        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChangeUsername()
        {
            try
            {
                Setting_model _model = _settingsrepository.Get(_loginuserid);
                _model.Mode = 1;
                return PartialView("~/Views/Settings/partial/_change_username_detail.cshtml", _model);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        public ActionResult MainMenus()
        {
            try
            {
                List<Menu_model> _model = _settingsrepository.GetMenus(_usertype, "Main");
                if (_usertype == "User") { return PartialView("~/Views/Settings/MainMenus.cshtml", _model); }
                else { return PartialView("~/Views/Settings/AdminMainMenus.cshtml", _model); }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult SideMenus()
        {
            try
            {
                List<Menu_model> _model = _settingsrepository.GetMenus(_usertype, "Side");
                return PartialView("~/Views/Settings/SideMenus.cshtml", _model); 
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[HttpGet]
        //public ActionResult ChangeUsername()
        //{
        //    try
        //    {
        //        Setting_model _model = _settingsrepository.Get(_loginuserid);
        //        _model.Mode = 1;
        //        _model.Username = "";
        //        return View( _model);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}

        [HttpGet]
        public ActionResult ChangePassword()
        {
            try
            {
                Setting_model _model = _settingsrepository.Get(_loginuserid);
                _model.Password = "";
                _model.Mode = 11;

                return PartialView("~/Views/Settings/partial/_change_password_detail.cshtml",_model);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        public ActionResult ChangePIN()
        {
            try
            {
                Setting_model _model = _settingsrepository.Get(_loginuserid);
                _model.PinNumber = "";
                _model.Mode = 111;
                return PartialView("~/Views/Settings/partial/_change_pin_detail.cshtml", _model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Update(Setting_model _model)
        {
            try
            {
                //if mode is updateusername, add validation here to check if new username is not yet existing


                if (ModelState.IsValid)
                {
                    if (_settingsrepository.CheckUsername(_model.Username, _model.UserId)) {                        
                        return Json(new { Result = "ERROR",
                            Message = "Username already exist. kindly try again.",
                            ElementName = "Username" });
                        }

                    int _id = _settingsrepository.Update(_model);

                    if (_model.Mode == 1)
                    {
                        FormsAuthentication.SignOut();
                        HttpContext.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
                        Session.Abandon();
                    }
                    return Json(new { Result = "Success", Id = _id });
                }

                List<string> _errors = _globalrepository.GetModelErrors(ModelState);
                return Json(new { Result = "ERROR", Message = _errors[1], ElementName = _errors[0] });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

    }
}
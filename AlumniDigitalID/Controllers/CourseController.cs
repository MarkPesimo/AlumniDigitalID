using Alumni.Repository;
using AlumniDigitalID.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.User_model;

namespace AlumniDigitalID.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private GlobalRepository _globalrepository { get; set; }
        private UserRepository _userrepository { get; set; }
        private CourseRepository _courserepository { get; set; }

        private string _infoview = "~/Views/Alumni/Index.cshtml";
        private int _loginuserid = 0;
        private int _schoolid = 0;
        private string _guid = "";

        public CourseController()
        {
            if (_globalrepository == null) { _globalrepository = new GlobalRepository(); }
            if (_courserepository == null) { _courserepository = new CourseRepository(); }
            if (_userrepository == null) { _userrepository = new UserRepository(); }

            if (_loginuserid == 0)
            {
                LoginUser_model _model = _userrepository.GetLoginUser();
                if (_model != null)
                {
                    _loginuserid = _model.UserId;
                    _schoolid = _model.SchoolId;
                    _guid = _model.Guid;
                }

            }
        }

        // GET: Course
        public ActionResult Index()
        {
            return View();
        }
    }
}
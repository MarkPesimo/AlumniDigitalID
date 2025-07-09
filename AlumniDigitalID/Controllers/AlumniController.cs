using Alumni.Repository;
using AlumniDigitalID.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.Perk_model;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.Student_model;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.User_model;

namespace AlumniDigitalID.Controllers
{
    [Authorize]
    public class AlumniController : Controller
    {
        private GlobalRepository _globalrepository { get; set; }
        private UserRepository _userrepository { get; set; }
        private AlumniRepository _alumnirepository { get; set; }
        private CourseRepository _courserepository { get; set; }

        private string _infoview = "~/Views/Alumni/Index.cshtml";
        private int _loginuserid = 0;
        private string _usertype = "";
        private int _schoolid = 0;
        private int _courseid = 0;
        private int _alumnigroupid = 0;
        private string _guid = "";

        private string _misc_about = "About";
        private string _misc_privacy = "Privacy";
        private string _misc_terms = "Terms";

        public AlumniController()
        {
            if (_globalrepository == null) { _globalrepository = new GlobalRepository(); }
            if (_alumnirepository == null) { _alumnirepository = new AlumniRepository(); }
            if (_userrepository == null) { _userrepository = new UserRepository(); }
            if (_courserepository == null) { _courserepository = new CourseRepository(); }

            if (_loginuserid == 0)
            {
                LoginUser_model _model = _userrepository.GetLoginUser();
                if (_model != null)
                {
                    _loginuserid = _model.UserId;
                    _alumnigroupid = _model.AlumniGroupId;
                    _usertype = _model.UserType;
                    _schoolid = _model.SchoolId;
                    _courseid = _model.CourseId;
                    _guid = _model.Guid;
                }

            }
        }

        [HttpGet]
        public ActionResult List()
        {
            if (_usertype == "User") {
                Profile_model _model = _alumnirepository.GetProfileByGuid(_guid);
                return View("~/Views/Alumni/Index.cshtml", _model);
            }

            return View();
        }

        // GET: Student
        public ActionResult Index()
        {
            try
            {
                Profile_model _model = _alumnirepository.GetProfileByGuid(_guid);
                if (_usertype == "User") { return View(_model); }
                else { return View("~/Views/Alumni/AdminIndex.cshtml", _model); }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    

        [HttpGet]
        public ActionResult Members()
        {
            try
            {
                if (_usertype == "User")
                {
                    Profile_model _model = _alumnirepository.GetProfileByGuid(_guid);
                    return View(_model);
                }
                else
                {
                    List<Members_model> _model = _alumnirepository.GetMembers(_alumnigroupid);
                    return View(_model);
                }
            }
            catch (Exception ex)
            {
                throw;
            }            
        }
 
        [HttpGet]
        public ActionResult _Activate(string _guid)
        {
            Members_model _model = _alumnirepository.GetMember(_guid);
            _model.Mode = 3;
            return PartialView("~/Views/Alumni/partial/_activate_member.cshtml", _model);
        }

        [HttpGet]
        public ActionResult _Deactivate(string _guid)
        {
            Members_model _model = _alumnirepository.GetMember(_guid);
            _model.Mode = 2;
            return PartialView("~/Views/Alumni/partial/_deactivate_member.cshtml", _model);
        }



        [HttpPost]
        public ActionResult ActivateDeactivate(Members_model _model)
        {
            try
            {
                if (ModelState.IsValid)
                {                    
                    int _id = _alumnirepository.ActivateDeactive(_model);
                    
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

 

        //=====================================================================================
        [HttpGet]
        public ActionResult Info(string guid)
        {
            try
            {
                Profile_model _model = _alumnirepository.GetProfileByGuid(guid);
                return View(_model);
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        [HttpGet]
        public ActionResult AlumniInfo()
        {
            try
            {
                Profile_model _model = _alumnirepository.GetProfileByGuid(_guid);



                return View(_infoview, _model);
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        [HttpGet]
        public ActionResult Edit(string _guid)
        {
            try
            {
                Profile_model _model = _alumnirepository.GetProfileByGuid(_guid);
                ViewBag._Gender = _globalrepository.GetGender().Select(t => new SelectListItem { Text = t.Description, Value = t.Value.ToString() }).ToList();
                ViewBag._CivilStatus = _globalrepository.GetCivilStatus().Select(t => new SelectListItem { Text = t.Description, Value = t.Value.ToString() }).ToList();
                ViewBag._Courses = _courserepository.GetSchoolCourses(_schoolid, _alumnigroupid).Select(t => new SelectListItem { Text = t.CourseName, Value = t.Id.ToString() }).ToList();

                
                return PartialView("~/Views/Alumni/partial/_edit.cshtml", _model);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public ActionResult Edit(Profile_model _model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int _id = _alumnirepository.UpdateProfile(_model);

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

        [HttpGet]
        public ActionResult Update()
        {
            try
            {
                Profile_model _model = _alumnirepository.GetProfileByGuid(_guid);
                ViewBag._Gender = _globalrepository.GetGender().Select(t => new SelectListItem { Text = t.Description, Value = t.Value.ToString() }).ToList();
                ViewBag._CivilStatus = _globalrepository.GetCivilStatus().Select(t => new SelectListItem { Text = t.Description, Value = t.Value.ToString() }).ToList();
                ViewBag._Courses = _courserepository.GetSchoolCourses(_schoolid, _alumnigroupid).Select(t => new SelectListItem { Text = t.CourseName, Value = t.Id.ToString() }).ToList();

                if (_usertype != "User") {
                    Profile_model _profile = _alumnirepository.GetProfileByGuid(_guid);
                    return View("~/Views/Alumni/AdminIndex.cshtml", _profile);
                }
                return View(_model);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public ActionResult Update(Profile_model _model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int _id = _alumnirepository.UpdateProfile(_model);

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

        [HttpPost]
        public ActionResult _Attachment(HttpPostedFileBase Attachment)
        {
            try
            {
                string _fileextension = _globalrepository.GetExtension(Attachment);
                var path = Path.Combine(Server.MapPath("~/AlumniImages/" + _guid.ToString() + ".JPEG"));

                if (System.IO.File.Exists(path)) { System.IO.File.Delete(path); }

                Attachment.SaveAs(path);

                return Json(new { Result = "Success", _guid=_guid });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult Terms()
        {
            Misc_model _model = _alumnirepository.GetMisc(_schoolid, _alumnigroupid, _misc_terms);
            return View(_model);
        }

               
        public ActionResult Privacy()
        {
            Misc_model _model = _alumnirepository.GetMisc(_schoolid, _alumnigroupid, _misc_privacy);
            return View(_model);
        }

        public ActionResult About()
        {
            Misc_model _model = _alumnirepository.GetMisc(_schoolid, _alumnigroupid, _misc_about);
            return View(_model);
        }


        //================= BEGIN MISC=================================
        [HttpGet]
        public ActionResult AboutIndex()
        {
            ViewBag.Title = "ABOUT US";

            Misc_model _model = _alumnirepository.GetMisc(_schoolid, _alumnigroupid, "About");
            _model.Type = "About Us";
            _model.UserId = _loginuserid;
            if (_usertype == "User") { return View("~/Views/Alumni/About.cshtml", _model); }

            return View("~/Views/Alumni/Misc.cshtml", _model);             
        }

        [HttpGet]
        public ActionResult PrivacyIndex()
        {
            ViewBag.Title = "PRIVACY POLICY";

            Misc_model _model = _alumnirepository.GetMisc(_schoolid, _alumnigroupid, "Privacy");
            _model.Type = "Privacy Policy";
            _model.UserId = _loginuserid;

            if (_usertype == "User") { return View("~/Views/Alumni/Privacy.cshtml", _model); }
            
            return View("~/Views/Alumni/Misc.cshtml", _model);
        }

        [HttpGet]
        public ActionResult TermsIndex()
        {
            ViewBag.Title = "TERM & CONDITIONS";

            Misc_model _model = _alumnirepository.GetMisc(_schoolid, _alumnigroupid, "Terms");
            _model.Type = "Terms & Condistions";
            _model.UserId = _loginuserid;

            if (_usertype == "User") { return View("~/Views/Alumni/Terms.cshtml", _model); }

            return View("~/Views/Alumni/Misc.cshtml", _model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Misc(int _id, string _type, string _description)
        {
            Misc_model _model = new Misc_model();
            try
            {
                _model.Id = _id;
                _model.Type = _type;
                _model.Description = _description;
                _model.UserId = _loginuserid;
            
                if (ModelState.IsValid)
                {
                    _id = _alumnirepository.UpdateMisc(_model);

                    return Json(new { Result = "Success", Type = _model.Type, Id = _id });
                }

                List<string> _errors = _globalrepository.GetModelErrors(ModelState);
                return Json(new { Result = "ERROR", Message = _errors[1], ElementName = _errors[0] });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }           
        }

        //================= END MISC=================================
    }
}
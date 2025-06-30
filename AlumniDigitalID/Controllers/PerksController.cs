using Alumni.Repository;
using AlumniDigitalID.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.Perk_model;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.User_model;

namespace AlumniDigitalID.Controllers
{
    [Authorize]
    public class PerksController : Controller
    {
        private GlobalRepository _globalrepository { get; set; }
        private UserRepository _userrepository { get; set; }
        private PerksRepository _perksrepository { get; set; }

        private int _loginuserid = 0;
        private int _alumnigroupid = 0;
        private int _schoolid = 0;
        private int _courseid = 0;
        private string _guid = "";
        private string _usertype = "";

        public PerksController()
        {
            if (_globalrepository == null) { _globalrepository = new GlobalRepository(); }
            if (_perksrepository == null) { _perksrepository = new PerksRepository(); }
            if (_userrepository == null) { _userrepository = new UserRepository(); }

            if (_loginuserid == 0)
            {
                LoginUser_model _model = _userrepository.GetLoginUser();
                if (_model != null)
                {
                    _loginuserid = _model.UserId;
                    _alumnigroupid = _model.AlumniGroupId;
                    _schoolid = _model.SchoolId;
                    _courseid = _model.CourseId;
                    _guid = _model.Guid;
                    _usertype = _model.UserType;
                }
            }
        }

        // GET: Perks
        public ActionResult PerksIndex()
        {
            List<PerkInfo_model> _model = _perksrepository.GetSchoolPerks(_schoolid, _alumnigroupid, _usertype);
            if (_usertype == "User") { return View("~/Views/Perks/Perks.cshtml", _model); }
            else { return View("~/Views/Perks/PerksIndex.cshtml", _model); }
            
        }

        public ActionResult Perks()
        {
            List<PerkInfo_model> _model = _perksrepository.GetSchoolPerks(_schoolid, _alumnigroupid, _usertype);
            return View("~/Views/Perks/Perks.cshtml", _model);
        }


        //public ActionResult CoursePerks()
        //{
        //    List<PerkInfo_model> _model = _perksrepository.GetSchoolPerks(_schoolid);
        //    return View("~/Views/Perks/PerksIndex.cshtml", _model);
        //}

        public ActionResult PerkByGuid(string _guid)
        {
            PerkInfo_model _model = _perksrepository.GetPerk(_guid);
            return View("~/Views/Perks/Perk.cshtml", _model);
        }

        public ActionResult Perk(int _id)
        {
            PerkInfo_model _model = _perksrepository.GetPerk(_id);
            return View("~/Views/Perks/Perk.cshtml", _model);
        }

        [HttpPost]
        public ActionResult Manage(PerkInfo_model _model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int _id = _perksrepository.ManagePerks(_model);

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
        public ActionResult _Add()
        {
            PerkInfo_model _model = new PerkInfo_model();
            _model.Id = 0;
            _model.SchoolId = _schoolid;
            _model.CourseId = _courseid;
            _model.AlumniGroupId = _alumnigroupid;
            _model.Title = "";
            _model.Description = "";
            _model.UserId = _loginuserid;
            _model.FileType = ".png";
            _model.DateCreated = DateTime.Now;
            _model.Guid = "-";
            _model.Mode = 0;

            return PartialView("~/Views/Perks/partial/_add_perks.cshtml", _model);
        }


        [HttpGet]
        public ActionResult _Edit(string _guid)
        {
            PerkInfo_model _model = _perksrepository.GetPerk(_guid);
            _model.Mode = 1;
            return PartialView("~/Views/Perks/partial/_edit_perks.cshtml", _model);
        }

        [HttpGet]
        public ActionResult _Activate(string _guid)
        {
            PerkInfo_model _model = _perksrepository.GetPerk(_guid);
            _model.Mode = 3;
            return PartialView("~/Views/Perks/partial/_activate_perks.cshtml", _model);
        }

        [HttpGet]
        public ActionResult _Deactivate(string _guid)
        {
            PerkInfo_model _model = _perksrepository.GetPerk(_guid);
            _model.Mode = 2;
            return PartialView("~/Views/Perks/partial/_deactivate_perks.cshtml", _model);
        }

        [HttpPost]
        public ActionResult _Attachment(int _id, HttpPostedFileBase Perks_Attachment)
        {
            try
            {
                string _fileextension = _globalrepository.GetExtension(Perks_Attachment);


                PerkInfo_model _model = new PerkInfo_model();
                _model.Id = _id;

                _model.SchoolId =0;
                _model.CourseId = 0;
                _model.Title = "";

                _model.Description = "";

                _model.UserId = _loginuserid;

                _model.FileType = _fileextension;
                _model.DateCreated = DateTime.Now;
                _model.Guid = "";
                _model.Mode = 11;
                
                _id = _perksrepository.ManagePerks(_model);
                //update table for the file extension 

                //check if candidate id folder already exist, if not create a folder
                //string _check_foloder = Path.Combine(Server.MapPath("~/LeaveAttachments/Filing/" + _id.ToString()), "");
                //if (Directory.Exists(_check_foloder) == false) { Directory.CreateDirectory(_check_foloder); }


                var path = Path.Combine(Server.MapPath("~/PerksAttachment/" + _id.ToString() + _fileextension));

                if (System.IO.File.Exists(path)) { System.IO.File.Delete(path); }

                Perks_Attachment.SaveAs(path);
                //attached procedure ends here------------------------------------------------------------------------------

                return Json(new { Result = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

    }
}
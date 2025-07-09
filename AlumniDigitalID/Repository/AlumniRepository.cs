using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.Perk_model;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.Student_model;

namespace Alumni.Repository
{
    public class AlumniRepository
    {
        private GlobalRepository _globalrepository { get; set; }

        public AlumniRepository()
        {
            if (_globalrepository == null) { _globalrepository = new GlobalRepository(); }
        }

        public Profile_model GetProfile(int _id)
        {
            try
            {
                Profile_model _obj = new Profile_model();
                string _endpoint = "Alumni/Get/" + _id.ToString();
                HttpResponseMessage _response = _globalrepository.GenerateGetRequest(_endpoint);
                if (_response.IsSuccessStatusCode)
                {
                    var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                    _obj = JsonConvert.DeserializeObject<Profile_model>(_value);
                }

                return _obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Profile_model GetProfileByGuid(string _guid)
        {
            try
            {
                Profile_model _obj = new Profile_model();
                string _endpoint = "Alumni/GetByGuid/" + _guid;
                HttpResponseMessage _response = _globalrepository.GenerateGetRequest(_endpoint);
                if (_response.IsSuccessStatusCode)
                {
                    var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                    _obj = JsonConvert.DeserializeObject<Profile_model>(_value);
                }

                return _obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UpdateProfile(Profile_model _model)
        {
            int _id = 0;
            string _endpoint = "Alumni/Update";


            if (_model.SectionName == null) { _model.SectionName = "-"; }
            if (_model.Middlename == null) { _model.Middlename = "-"; }

            var _content_prop = new Dictionary<string, string>
            {
                {"Id",                      _model.Id.ToString() },
                {"AlumniGroupId",           _model.AlumniGroupId.ToString() },
                {"Guid",                    _model.Guid.ToString() },
                {"Firstname",               _model.Firstname.ToString() },
                {"Lastname",                _model.Lastname.ToString()},
                {"Middlename",              _model.Middlename.ToString() },
                {"Birthday",                _model.Birthday.ToString() },
                {"Gender",                  _model.Gender.ToString() },
                {"CivilStatus",             _model.CivilStatus.ToString() },
                {"SectionName",             _model.SectionName.ToString() },
                {"AlumniNo",                _model.AlumniNo.ToString() },
                {"SchoolId",                _model.SchoolId.ToString() },
                {"SchoolName",              _model.SchoolName.ToString() },
                {"CourseId",                _model.CourseId.ToString() },
                {"CourseName",              _model.CourseName.ToString() },
                {"EmailAddress",            _model.EmailAddress.ToString() },
                {"MobileNo",                _model.MobileNo.ToString() },
                {"Address",                 _model.Address.ToString() },
                {"YearGraduated",           _model.YearGraduated.ToString() },
                {"UserType",                _model.UserType.ToString() },
                {"MemberType",              _model.MemberType.ToString() },
                {"MembershipExpiration",    _model.MembershipExpiration.ToString() },
                {"UserId",                  _model.UserId.ToString() },
                {"Nationality",             _model.Nationality.ToString() },
            };

            string _body_content = JsonConvert.SerializeObject(_content_prop);
            HttpContent _content = new StringContent(_body_content, Encoding.UTF8, "application/json");

            HttpResponseMessage _response = _globalrepository.GeneratePostRequest(_endpoint, _content);
            if (_response.IsSuccessStatusCode)
            {
                var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                _id = int.Parse(_value);
            }

            return _id;
        }


        public List<Members_model> GetMembers(int _alumnigroupid)
        {
            try
            {
                List<Members_model> _obj = new List<Members_model>();
                string _endpoint = "Alumni/Members/" + _alumnigroupid.ToString();
                HttpResponseMessage _response = _globalrepository.GenerateGetRequest(_endpoint);
                if (_response.IsSuccessStatusCode)
                {
                    var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                    _obj = JsonConvert.DeserializeObject<List<Members_model>>(_value);
                }

                return _obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Members_model GetMember(string _guid)
        {
            try
            {
                Members_model _obj = new Members_model();
                string _endpoint = "Alumni/Member/" + _guid.ToString();
                HttpResponseMessage _response = _globalrepository.GenerateGetRequest(_endpoint);
                if (_response.IsSuccessStatusCode)
                {
                    var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                    _obj = JsonConvert.DeserializeObject<Members_model>(_value);
                }

                return _obj;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public int ActivateDeactive(Members_model _model)
        {
            int _id = 0;
            string _endpoint = "Alumni/ActivateDeactivate";

            if (_model.Edit_prop == null) { _model.Edit_prop = "NA"; }
            if (_model.Activate_prop == null) { _model.Activate_prop = "NA"; }
            if (_model.Deactivate_prop == null) { _model.Deactivate_prop = "NA"; }

            var _content_prop = new Dictionary<string, string>
            {
                {"Id",                  _model.Id.ToString() },
                {"Guid",                _model.Guid.ToString() },
                {"MembersName",         _model.MembersName.ToString() },
                {"Course",              _model.Course.ToString()},
                {"Batch",               _model.Batch.ToString() },
                {"MemberType",          _model.MemberType.ToString() },
                {"Edit_prop",           _model.Edit_prop.ToString() },
                {"Activate_prop",       _model.Activate_prop.ToString() },
                {"Deactivate_prop",     _model.Deactivate_prop.ToString() },
                {"Mode",                _model.Mode.ToString() },
                {"UserId",              _model.UserId.ToString() },
 
            };

            string _body_content = JsonConvert.SerializeObject(_content_prop);
            HttpContent _content = new StringContent(_body_content, Encoding.UTF8, "application/json");

            HttpResponseMessage _response = _globalrepository.GeneratePostRequest(_endpoint, _content);
            if (_response.IsSuccessStatusCode)
            {
                var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                _id = int.Parse(_value);
            }

            return _id;
        }

        //================= BEGIN MISC=================================
        public Misc_model GetMisc(int _schoolid, int _alumnigroupid, string _type)
        {
            try
            {
                Misc_model _obj = new Misc_model();
                string _endpoint = "Alumni/GetMisc/" + _schoolid.ToString() +
                    "/" + _alumnigroupid.ToString() + 
                    "/" + _type;
                HttpResponseMessage _response = _globalrepository.GenerateGetRequest(_endpoint);
                if (_response.IsSuccessStatusCode)
                {
                    var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                    _obj = JsonConvert.DeserializeObject<Misc_model>(_value);
                }

                return _obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UpdateMisc(Misc_model _model)
        {
            int _id = 0;
            string _endpoint = "Alumni/UpdateMisc";
            

            var _content_prop = new Dictionary<string, string>
            {
                {"Id",                  _model.Id.ToString() },
                {"Description",         _model.Description.ToString() },
                {"UserId",              _model.UserId.ToString() }, 
            };

            string _body_content = JsonConvert.SerializeObject(_content_prop);
            HttpContent _content = new StringContent(_body_content, Encoding.UTF8, "application/json");

            HttpResponseMessage _response = _globalrepository.GeneratePostRequest(_endpoint, _content);
            if (_response.IsSuccessStatusCode)
            {
                var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                _id = int.Parse(_value);
            }

            return _id;
        }

        //================= END MISC=================================
    }
}

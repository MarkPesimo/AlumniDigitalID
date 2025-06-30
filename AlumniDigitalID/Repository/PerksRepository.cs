using Alumni.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.Perk_model;

namespace AlumniDigitalID.Repository
{
    public class PerksRepository
    {
        private GlobalRepository _globalrepository { get; set; }

        public PerksRepository()
        {
            if (_globalrepository == null) { _globalrepository = new GlobalRepository(); }
        }

        //================= BEGIN PERKS=================================
        public PerkInfo_model GetPerk(string _guid)
        {
            try
            {
                PerkInfo_model _obj = new PerkInfo_model();
                string _endpoint = "Perks/GetByGuid/" + _guid.ToString();
                HttpResponseMessage _response = _globalrepository.GenerateGetRequest(_endpoint);
                if (_response.IsSuccessStatusCode)
                {
                    var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                    _obj = JsonConvert.DeserializeObject<PerkInfo_model>(_value);
                }

                return _obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PerkInfo_model GetPerk(int _id)
        {
            try
            {
                PerkInfo_model _obj = new PerkInfo_model();
                string _endpoint = "Perks/Get/" + _id;
                HttpResponseMessage _response = _globalrepository.GenerateGetRequest(_endpoint);
                if (_response.IsSuccessStatusCode)
                {
                    var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                    _obj = JsonConvert.DeserializeObject<PerkInfo_model>(_value);
                }

                return _obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PerkInfo_model> GetSchoolPerks(int _schoolid, int _alumnigroupid, string _usertpe)
        {
            try
            {
                List<PerkInfo_model> _obj = new List<PerkInfo_model>();
                string _endpoint = "Perks/get/" + _schoolid + 
                    "/" + _alumnigroupid.ToString() +
                    "/" + _usertpe;
                HttpResponseMessage _response = _globalrepository.GenerateGetRequest(_endpoint);
                if (_response.IsSuccessStatusCode)
                {
                    var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                    _obj = JsonConvert.DeserializeObject<List<PerkInfo_model>>(_value);
                }

                return _obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ManagePerks(PerkInfo_model _model)
        {
            int _id = 0;
            string _endpoint = "Perks/Manage";

            if (_model.Guid == "") { _model.Guid = "-"; }

            var _content_prop = new Dictionary<string, string>
            {
                {"Id",                  _model.Id.ToString() },
                {"AlumniGroupId",       _model.AlumniGroupId.ToString() },
                {"SchoolId",            _model.SchoolId.ToString() },
                {"CourseId",            _model.CourseId.ToString() },
                {"Title",               _model.Title.ToString()},
                {"Description",         _model.Description.ToString() },
                {"UserId",              _model.UserId.ToString() },
                {"FileType",            _model.FileType.ToString() },
                {"DateCreated",         _model.DateCreated.ToString() },
                {"Guid",                _model.Guid.ToString() },
                {"Mode",                _model.Mode.ToString() },
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

        //================= END PERKS=================================

    }
}
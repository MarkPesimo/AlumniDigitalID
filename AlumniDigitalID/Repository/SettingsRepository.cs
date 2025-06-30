using Alumni.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.User_model;

namespace AlumniDigitalID.Repository
{
    public class SettingsRepository
    {
        private GlobalRepository _globalrepository { get; set; }

        public SettingsRepository()
        {
            if (_globalrepository == null) { _globalrepository = new GlobalRepository(); }
        }

        public Setting_model Get(int _userid)
        {
            try
            {
                Setting_model _obj = new Setting_model();
                string _endpoint = "AlumniUser/GetSettings/" + _userid.ToString();
                HttpResponseMessage _response = _globalrepository.GenerateGetRequest(_endpoint);
                if (_response.IsSuccessStatusCode)
                {
                    var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                    _obj = JsonConvert.DeserializeObject<Setting_model>(_value);
                }

                return _obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Setting_model _model)
        {
            int _id = 0;
            string _endpoint = "AlumniUser/UpdateSettings";

            string _hash_password = _model.Password != "" ? _globalrepository.PasswordHasher(_model.Password) : "";
            string _hash_pin = _model.PinNumber != "" ? _globalrepository.PasswordHasher(_model.PinNumber) : "";

            var _content_prop = new Dictionary<string, string>
            {
                {"UserId",                  _model.UserId.ToString() },
                {"Username",                _model.Username.ToString() },
                {"Password",                _hash_password.ToString() },
                {"PinNumber",               _hash_pin.ToString()},
                {"Mode",                    _model.Mode.ToString()},
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

        public int ChangeUsername(Setting_model _model)
        {
            int _id = 0;
            string _endpoint = "AlumniUser/ChangeUsername";
                        
            string _hash_password = _globalrepository.PasswordHasher(_model.Password);
            string _hash_pin = _globalrepository.PasswordHasher(_model.PinNumber);

            var _content_prop = new Dictionary<string, string>
            {
                {"UserId",                  _model.UserId.ToString() },
                {"Username",                _model.Username.ToString() },
                {"Password",                _hash_password.ToString() },
                {"PinNumber",               _hash_pin.ToString()},
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

        public bool CheckUsername(string _username, int _userid)
        {
            try
            {
                bool _isexist = false;
                string _endpoint = "AlumniUser/CheckUsername/" + _username +
                    "/" + _userid.ToString();
                HttpResponseMessage _response = _globalrepository.GenerateGetRequest(_endpoint);
                if (_response.IsSuccessStatusCode)
                {
                    var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                    _isexist = JsonConvert.DeserializeObject<bool>(_value);
                }

                return _isexist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Menu_model> GetMenus(string _usertype, string _menutype)
        {
            try
            {
                List< Menu_model> _obj = new List<Menu_model>();
                string _endpoint = "Alumni/GetMenus/" + _menutype +
                    "/" + _usertype;
                HttpResponseMessage _response = _globalrepository.GenerateGetRequest(_endpoint);
                if (_response.IsSuccessStatusCode)
                {
                    var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                    _obj = JsonConvert.DeserializeObject<List<Menu_model>>(_value);
                }

                return _obj;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
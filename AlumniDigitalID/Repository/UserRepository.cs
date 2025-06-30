using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

using System.Web;

using System.Web.Script.Serialization;
using System.Web.Security;
using static AlumniDigitalID.Models.misc_model;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.User_model;

namespace Alumni.Repository
{
    public class UserRepository
    {
        private GlobalRepository _globalrepository { get; set; }

        public UserRepository()
        {
            if (_globalrepository == null) { _globalrepository = new GlobalRepository(); }
        }

        public bool LogLogin(LoginLog_model _model)
        {
            var _content_prop = new Dictionary<string, string>
            {
                {"UserId",      _model.UserId.ToString() },
                {"Longitude",   _model.Longitude },
                {"Latitude",    _model.Latitude },
            };

            string _body_content = JsonConvert.SerializeObject(_content_prop);
            HttpContent _content = new StringContent(_body_content, Encoding.UTF8, "application/json");

            string _endpoint = "AlumniUser/LogLogin";
            HttpResponseMessage _response = _globalrepository.GeneratePostRequest(_endpoint, _content);

            if (_response.IsSuccessStatusCode)
            {
                var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                var _result = JsonConvert.DeserializeObject<bool>(_value);

                return _result;
            }

            return false;
        }

        public LoginResult Pin(Pin_model _model)
        {
            LoginResult _result = new LoginResult
            {
                UserId = 0,
                Result = ""
            };

            string _hash_password = _globalrepository.PasswordHasher(_model.PinNumber);
            var _content_prop = new Dictionary<string, string>
            {
                {"GuidId", _model.GuidId.ToString() },
                {"SchoolId", _model.SchoolId.ToString() },
                {"PinNumber", _hash_password },
            };

            string _body_content = JsonConvert.SerializeObject(_content_prop);
            HttpContent _content = new StringContent(_body_content, Encoding.UTF8, "application/json");

            string _endpoint = "AlumniUser/GetByPIN";
            HttpResponseMessage _response = _globalrepository.GeneratePostRequest(_endpoint, _content);

            if (_response.IsSuccessStatusCode)
            {
                var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                var _CustomPrincipalSerializeModel = JsonConvert.DeserializeObject<LoginUser_model>(_value);

                if (_CustomPrincipalSerializeModel.UserType == "User")
                {
                    if (_CustomPrincipalSerializeModel.Expiration != "-")
                    {
                        if (DateTime.Parse(_CustomPrincipalSerializeModel.Expiration) < DateTime.Now)
                        {
                            _result.Result = "Your Alumni Account Has Expired. To restore access, please reach out to your administrator for assistance.";
                            return _result;
                        }
                    }
                }
                

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string userData = serializer.Serialize(_CustomPrincipalSerializeModel);


                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,
                        _CustomPrincipalSerializeModel.Username,
                        DateTime.Now, DateTime.Now.AddDays(1),
                        true,
                        userData,
                        FormsAuthentication.FormsCookiePath);

                string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);

                
                
                _result.Result = "Ok";
                _result.UserId = _CustomPrincipalSerializeModel.UserId;
                
                
                return _result;
            }

            _result.Result = "Incorrect PIN number";
            return _result;
        }

        public LoginResult Login(Login_model model)
        {
            LoginResult _result = new LoginResult
            {
                UserId = 0,
                Result = ""
            };

            string _hash_password = _globalrepository.PasswordHasher(model.Password);
            var _content_prop = new Dictionary<string, string>
            {
                {"SchoolId", model.SchoolId.ToString() },
                {"Username", model.Username },
                {"Password", _hash_password },
            };


            string _body_content = JsonConvert.SerializeObject(_content_prop);
            HttpContent _content = new StringContent(_body_content, Encoding.UTF8, "application/json");

            string _endpoint = "AlumniUser/Login";
            HttpResponseMessage _response = _globalrepository.GeneratePostRequest(_endpoint, _content);

            if (_response.IsSuccessStatusCode)
            {
                var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                var _CustomPrincipalSerializeModel = JsonConvert.DeserializeObject<LoginUser_model>(_value);

                if (_CustomPrincipalSerializeModel.UserType == "User")
                {
                    if (_CustomPrincipalSerializeModel.Expiration != "-")
                    {
                        if (DateTime.Parse(_CustomPrincipalSerializeModel.Expiration) < DateTime.Now)
                        {
                            _result.Result = "Your Alumni Account Has Expired. To restore access, please reach out to your administrator for assistance.";
                            return _result;
                        }
                    }
                    
                }



                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string userData = serializer.Serialize(_CustomPrincipalSerializeModel);


                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,
                        model.Username,
                        DateTime.Now, DateTime.Now.AddDays(1),
                        true,
                        userData,
                        FormsAuthentication.FormsCookiePath);

                string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
 
                _result.Result = "Ok";
                _result.UserId = _CustomPrincipalSerializeModel.UserId;

                return _result;
            }

            _result.Result = "Incorrect Username or Password";
            return _result;
        }


        public LoginUser_model GetLoginUser()
        {
            var _username = System.Web.HttpContext.Current.User;
            LoginUser_model _obj = new LoginUser_model();
            string _endpoint = "AlumniUser/GetByUserName/" + _username.Identity.Name.ToString();
            HttpResponseMessage _response = _globalrepository.GenerateGetRequest(_endpoint);
            if (_response.IsSuccessStatusCode)
            {
                var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                _obj = JsonConvert.DeserializeObject<LoginUser_model>(_value);
            }

            return _obj;
        }

        public Pin_Validity IsValidGUID(string _guid)
        {
            Pin_Validity _obj = new Pin_Validity() ;
            string _endpoint = "AlumniUser/IsValidGUID/" + _guid;
            HttpResponseMessage _response = _globalrepository.GenerateGetRequest(_endpoint);
            if (_response.IsSuccessStatusCode)
            {
                var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                _obj = JsonConvert.DeserializeObject<Pin_Validity>(_value);
            }

            return _obj;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.User_model;

namespace Alumni.Logic.Repository
{
    public class UserRepository
    {
        private GlobalRepository _globalrepository { get; set; }

        public UserRepository()
        {

            if (_globalrepository == null) { _globalrepository = new GlobalRepository(); }
        }

        public string Login(Login_model model)
        {
            string _hash_password = _globalrepository.Md5HashPassword(model.Password);
            var _content_prop = new Dictionary<string, string>
                {
                    {"SchoolId", model.SchoolId.ToString() },
                    {"Username", model.Username },
                    {"Password", _hash_password },

                };


            string _body_content = JsonConvert.SerializeObject(_content_prop);
            HttpContent _content = new StringContent(_body_content, Encoding.UTF8, "application/json");

            string _endpoint = "Account/Login";
            HttpResponseMessage _response = _globalrepository.GeneratePostRequest(_endpoint, _content);

            if (_response.IsSuccessStatusCode)
            {
                var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                var _CustomPrincipalSerializeModel = JsonConvert.DeserializeObject<Login_model>(_value);


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

                return "Ok";
            }

            return "Incorrect Username or Password";
        }
    }
}

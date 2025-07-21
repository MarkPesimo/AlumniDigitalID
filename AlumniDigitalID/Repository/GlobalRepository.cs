using AlumniDigitalID;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZMGModel;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.Student_model;

namespace Alumni.Repository
{
    public class GlobalRepository
    {
        public HttpResponseMessage GenerateGetRequest(string _endpoint)
        {
            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(AlumniConstant.BaseAddress);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", SecretKeys.ALUMNI_TOKEN); 

                HttpResponseMessage _response = client.GetAsync(_endpoint).Result;

                return _response;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public HttpResponseMessage GeneratePostRequest(string _endpoint, HttpContent _content = null)
        {
            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(AlumniConstant.BaseAddress);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", SecretKeys.ALUMNI_TOKEN);

                HttpResponseMessage _response = new HttpResponseMessage();

                _response = client.PostAsync(_endpoint, _content).Result;

                return _response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public HttpResponseMessage GeneratePostRequest(string _endpoint)
        {
            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(AlumniConstant.BaseAddress);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", SecretKeys.ALUMNI_TOKEN);

                HttpResponseMessage _response = new HttpResponseMessage();

                _response = client.PostAsync(_endpoint, null).Result;

                return _response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string Md5HashPassword(string _value)
        {
            var md5Hasher = MD5.Create();

            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(_value));
            var sBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public string PasswordHasher(string _keyword)
        {
            UnicodeEncoding Ue = new UnicodeEncoding();

            byte[] ByteSourceText = Ue.GetBytes(_keyword);
            MD5CryptoServiceProvider Md5 = new MD5CryptoServiceProvider();
            byte[] ByteHash = Md5.ComputeHash(ByteSourceText);
            return Convert.ToBase64String(ByteHash);
        }

        public string GetExtension(HttpPostedFileBase _attachment)
        {
            string _ext = "";


            if (_attachment.ContentType == "image/jpeg") { _ext = ".jpg"; }
            else if (_attachment.ContentType == "image/png") { _ext = ".png"; }
            else if (_attachment.ContentType == "image/gif") { _ext = ".gif"; }
            else if (_attachment.ContentType == "application/pdf") { _ext = ".pdf"; }
            else if (_attachment.ContentType == "application/msword") { _ext = ".doc"; }
            else if (_attachment.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document") { _ext = ".docx"; }

            return _ext;
        }


        public List<String> GetModelErrors(System.Web.Mvc.ModelStateDictionary _modelstate)
        {
            List<string> _errors = new List<string>();

            int _cnt = 0;
            foreach (var item in _modelstate.Values)
            {
                var _elementname = _modelstate.Keys.ToList();
                if (item.Errors.Count != 0)
                {
                    _errors.Add(_elementname[_cnt]);
                    _errors.Add(item.Errors[0].ErrorMessage);
                    break;
                }
                _cnt++;
            }

            return _errors;
        }

        public List<Year_model> GetYears()
        {
            try
            {
                List<Year_model> _obj = new List<Year_model>();
                string _endpoint = "Alumni/GetYears";
                HttpResponseMessage _response = GenerateGetRequest(_endpoint);
                if (_response.IsSuccessStatusCode)
                {
                    var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                    _obj = JsonConvert.DeserializeObject<List<Year_model>>(_value);
                }

                return _obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Membertype_model> GetMemberType()
        {
            return new List<Membertype_model>() {
                new Membertype_model(){ Description="Lifetime Member",Value="Lifetime Member" },
                new Membertype_model(){ Description="Annual Member",Value="Annual Member" }
            };
        }

        public IEnumerable<Gender_model> GetGender()
        {
            return new List<Gender_model>() {
                new Gender_model(){ Description="Male",Value="Male" },
                new Gender_model(){ Description="Female",Value="Female" }
            };
        }


        public IEnumerable<CivilStatus_model> GetCivilStatus()
        {
            return new List<CivilStatus_model>() {
                 new CivilStatus_model(){
                    Description="Single",Value="Single"
                },
                new CivilStatus_model(){
                    Description="Married",Value="Married"
                },
                new CivilStatus_model(){
                    Description="Separated",Value="Separated"
                },
                new CivilStatus_model(){
                    Description="Widowed",Value="Widowed"
                },
                new CivilStatus_model(){
                    Description="Annuled/Divorced",Value="Annuled/Divorced"
                }
            };
        }
    }
}

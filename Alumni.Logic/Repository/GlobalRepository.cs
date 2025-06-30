using AlumniDigitalID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ZMGModel;

namespace Alumni.Logic.Repository
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

    }
}

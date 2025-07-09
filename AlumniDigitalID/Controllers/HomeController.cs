using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace AlumniDigitalID.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Success()
        {
            return View();
        }


        //public ActionResult GenerateQR()
        //{
        //    try
        //    {
        //        HttpClient client = new HttpClient();
        //        string _endpoint = "qr/custom";
        //        client.BaseAddress = new Uri("https://qrcode-monkey.p.rapidapi.com/");
        //        //client.DefaultRequestHeaders.Add("X-API-KEY", iSearchConstant.ParserKey_1);
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        var _content_prop = new Dictionary<string, string>
        //        {
        //            {"url",   _cv  },
        //        };

        //        string _body_content = JsonConvert.SerializeObject(_content_prop);
        //        HttpContent _content = new StringContent(_body_content, Encoding.UTF8, "application/json");

        //        HttpResponseMessage _response = new HttpResponseMessage();

        //        _response = client.PostAsync(_endpoint, _content).Result;
        //        var _value = "";
        //        if (_response.IsSuccessStatusCode)
        //        {
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        //[Authorize]
        //public ActionResult Perks()
        //{
        //    return View();
        //}

        //[Authorize]
        //public ActionResult Privacy()
        //{
        //    return View();
        //}



        //[Authorize]
        //public ActionResult About()
        //{
        //    return View();
        //}



    }
}
using Alumni.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using static ZMGModel.ViewModel.ALUMNI.Alumni_Model.Course_model;

namespace AlumniDigitalID.Repository
{
    public class CourseRepository
    {
        private GlobalRepository _globalrepository { get; set; }

        public CourseRepository()
        {
            if (_globalrepository == null) { _globalrepository = new GlobalRepository(); }
        }


        public List<CourseList_model> GetSchoolCourses(int _schoolid, int _alumnigroupid)
        {
            try
            {
                List<CourseList_model> _obj = new List<CourseList_model>();
                string _endpoint = "Course/GetSchoolCourses/" + _schoolid.ToString() +
                    "/" + _alumnigroupid.ToString();
                HttpResponseMessage _response = _globalrepository.GenerateGetRequest(_endpoint);
                if (_response.IsSuccessStatusCode)
                {
                    var _value = _response.Content.ReadAsStringAsync().Result.ToString();
                    _obj = JsonConvert.DeserializeObject<List<CourseList_model>>(_value);
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
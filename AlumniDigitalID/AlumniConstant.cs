using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AlumniDigitalID
{
    public class AlumniConstant
    {
        public static string BaseAddress = ConfigurationManager.AppSettings["_BaseAddress"].ToString();

        public static string SecretKey = ConfigurationManager.AppSettings["SecretKey"].ToString();
        public static int SchoolId = int.Parse( ConfigurationManager.AppSettings["SchoolId"].ToString());
        public static int CourseId = int.Parse(ConfigurationManager.AppSettings["CourseId"].ToString());
    }
}
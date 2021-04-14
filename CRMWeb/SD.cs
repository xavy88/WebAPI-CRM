using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb
{
    public class SD
    {
        public static string APIBaseUrl = "https://localhost:44321/";
        public static string DepartmentAPIPath = APIBaseUrl + "api/v1/departments/";
        public static string PositionAPIPath = APIBaseUrl + "api/v1/positions/";
        public static string AccountAPIPath = APIBaseUrl + "api/v1/users/";
        public static string EmployeeAPIPath = APIBaseUrl + "api/v1/employees/";
        public static string ClientAPIPath = APIBaseUrl + "api/v1/accounts/";
        public static string ContactAPIPath = APIBaseUrl + "api/v1/contacts/";
        public static string ServiceAPIPath = APIBaseUrl + "api/v1/services/";
    }
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace BOL_Business_Objects_Layer_
{
    public class Users
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string Registered { get; set; }
        public string LastActivity { get; set; }
        public string UserStatus { get; set; }
        public string IsLocked { get; set; }
        public string LoginCount { get; set; }
        public List<CompYear> compyears { get; set; }
    }
    public class CompYear
    {
        public string UserId { get; set; }
        public string CompCode { get; set; }
        public string CompName { get; set; }
    }
}

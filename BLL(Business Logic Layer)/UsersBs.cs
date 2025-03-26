using BOL_Business_Objects_Layer_;
using DAL_Data_Access_Layer_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_
{
    public class UsersBs
    {
        private UsersDb lobj;
        private bool _result;
        public bool Result
        {
            get { return this._result; }
            set { this._result = value; }
        }
        private string _message;
        public string Message
        {
            get { return this._message; }
            set { this._message = value; }
        }
        public UsersBs()
        {
            lobj = new UsersDb();
        }
        public List<Login> UserLogin(string username, string password)
        {
            return lobj.UserLogin(username, password);
        }
        public void Add(Users obj)
        {
            lobj.Add(obj);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public List<Users> AllList()
        {
            return lobj.AllList();
        }

        public void Update(Users objuser)
        {
            lobj.Update(objuser);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public void AddPermission(Menu objmenu)
        {
            lobj.AddPermission(objmenu);
            Result = lobj.Result;
            Message = lobj.Message;
        }
    }
}

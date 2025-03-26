using BOL_Business_Objects_Layer_;
using DAL_Data_Access_Layer_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_
{
    public class CountryBs
    {
        private CountryDb lobj;

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
        public CountryBs()
        {
            lobj = new CountryDb();
        }
        public void Add(Country obj)
        {
            lobj.Add(obj);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public List<Country> AllList(string compcode)
        {
            return lobj.AllList(compcode);
        }

        public void Update(Country objuser)
        {
            lobj.Update(objuser);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public void Delete(int CountryId, string compcode)
        {
            lobj.Delete(CountryId, compcode);
            Result = lobj.Result;
            Message = lobj.Message;
        }
    }
}

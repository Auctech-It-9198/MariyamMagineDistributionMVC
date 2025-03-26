using BOL_Business_Objects_Layer_;
using DAL_Data_Access_Layer_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_
{
    public class SubscriberBs
    {
        private SubscriberDb lobj;

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
        public SubscriberBs()
        {
            lobj = new SubscriberDb();
        }
        public void Add(Subscriber obj)
        {
            lobj.Add(obj);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public List<Subscriber> AllListPagewise(JqueryDatatableParam param, int cid, int sid, int cityid, string deactive, string forworder, string mtype, string gender, string ptype)
        {
            return lobj.AllListPagewise(param, cid, sid, cityid, deactive, forworder, mtype, gender, ptype);
        }

        public List<Subscriber> AllList(string mscode)
        {
            return lobj.AllList(mscode);
        }

        public void Update(Subscriber obj)
        {
            lobj.Update(obj);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public void Delete(string mscode)
        {
            lobj.Delete(mscode);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public List<Subscriber> GetForwarderChildListDetails(string mscode)
        {
            return lobj.GetForwarderChildListDetails(mscode);
        }
    }
}

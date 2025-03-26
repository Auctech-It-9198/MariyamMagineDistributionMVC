using BOL_Business_Objects_Layer_;
using DAL_Data_Access_Layer_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_
{
    public class SubscriptionsBs
    {
        private SubscriptionsDb lobj;

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
        public SubscriptionsBs()
        {
            lobj = new SubscriptionsDb();
        }
        public void Add(Subscriptions obj)
        {
            lobj.Add(obj);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public List<Subscriptions> AllListPagewise(JqueryDatatableParam param, int cid, int sid, int cityid, string deactive, string forworder, string mtype, string gender, string mzid, string tenure, string fromdate, string todate, string expmonth, string expyear)
        {
            return lobj.AllListPagewise(param, cid, sid, cityid, deactive, forworder, mtype, gender,mzid,tenure,fromdate,todate,expmonth,expyear);
        }

   

        public List<Subscriptions> getRecordsbysbid(string sbid, string mzid, string msid)
        {
            return lobj.getRecordsbysbid(sbid, mzid,msid);
        }

        public void Renew(Subscriptions obj)
        {
            lobj.Renew(obj);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public void Grace(Subscriptions obj)
        {
            lobj.Grace(obj);
            Result = lobj.Result;
            Message = lobj.Message;
        }
    }
}

using BOL_Business_Objects_Layer_;
using DAL_Data_Access_Layer_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_
{
    public class DeliveryBs
    {
        private DeliveryDb lobj;

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
        public DeliveryBs()
        {
            lobj = new DeliveryDb();
        }
        
        public List<Delivery> AllListPagewise(string search, string cid, string sid, string cityid, string areaid, string deactive, string mtype, string gender, string mzid, string mrid, string gperiods, string cstatus)
        {
            return lobj.AllListPagewise(search, cid, sid, cityid, areaid, deactive, mtype, gender, mzid, mrid, gperiods, cstatus);
        }

        public void Add(List<Delivery> delivery)
        {
            lobj.Add(delivery);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public List<Delivery> AllListPagewiselist(string search, string cid, string sid, string cityid, string areaid, string deactive, string mtype, string gender, string mzid, string mrid, string gperiods, string cstatus)
        {
            return lobj.AllListPagewiselist(search, cid, sid, cityid, areaid, deactive, mtype, gender, mzid, mrid, gperiods, cstatus);
        }

        public void Delete(int vno, string vtype)
        {
            lobj.Delete(vno,vtype);
            Result = lobj.Result;
            Message = lobj.Message;

        }

        public List<Delivery> DisplayListforprint(string vno, string vtype, string isfarwarder)
        {
            return lobj.DisplayListforprint(vno,vtype,isfarwarder);
        }

        public List<Delivery> AllListPagewiseforprintlables(string search, string cid, string sid, string cityid, string areaid, string deactive, string mtype, string gender, string mzid, string mrid, string gperiods, string cstatus)
        {
            return lobj.AllListPagewiseforprintlables(search, cid, sid, cityid, areaid, deactive, mtype, gender, mzid, mrid, gperiods, cstatus);
        }

        public void DispatchAdd(List<Delivery> delivery)
        {

            lobj.DispatchAdd(delivery);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public void AddPrint(List<Delivery> delivery)
        {

            lobj.AddPrint(delivery);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public List<Delivery> DisplayPrinlist(JqueryDatatableParam param, string mzid, string mrid)
        {
            return lobj.DisplayPrinlist(param, mzid,mrid);
        }

        public void DeletePrintRecord(int vno, int pvno)
        {
            lobj.DeletePrintRecord(vno, pvno);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public List<Delivery> DisplayListforDispatch(string vno)
        {
            return lobj.DisplayListforDispatch(vno);
        }

        public void Dispatchpostadd(List<Delivery> delivery)
        {
            lobj.Dispatchpostadd(delivery);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public List<Delivery> DisplayListPagewiseforsale(JqueryDatatableParam param , string vtype)
        {
            return  lobj.DisplayListPagewiseforsale(param , vtype);
        }

        public void DeleteSale(int vno, string vtype)
        {
            lobj.DeleteSale(vno,vtype);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public List<Delivery> GetSaleEntryRecordss(string vno, string vtype)
        {
            return lobj.GetSaleEntryRecordss(vno, vtype);
        }

        public void Update(List<Delivery> delivery)
        {
            lobj.Update(delivery);
            Result = lobj.Result;
            Message = lobj.Message;
        }
    }
}

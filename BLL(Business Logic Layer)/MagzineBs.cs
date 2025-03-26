using BOL_Business_Objects_Layer_;
using DAL_Data_Access_Layer_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_
{
    public class MagzineBs
    {
        private MagzineDb lobj;

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
        public MagzineBs()
        {
            lobj = new MagzineDb();
        }
        public void Add(Magzine obj)
        {
            lobj.Add(obj);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public List<Magzine> AllListPagewise(JqueryDatatableParam param)
        {
            return lobj.AllListPagewise(param);
        }

        public List<Magzine> AllList(string MagzineId)
        {
            return lobj.AllList(MagzineId);
        }

        public void Update(Magzine obj)
        {
            lobj.Update(obj);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public void Delete(int magzineId)
        {
            lobj.Delete(magzineId);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        //public void ReleaseMagzine(MagzineRelease objuser)
        //{
        //    throw new NotImplementedException();
        //}

        public List<MagzineRelease> AllReleaseListPagewise(JqueryDatatableParam param)
        {
            return lobj.AllReleaseListPagewise(param);
        }

        public List<MagzineRelease> GetMagzineReleaseDetails(string mRId)
        {
            return lobj.GetMagzineReleaseDetails(mRId);
        }

        public List<MagzineArticle> AllArticleListPagewise(JqueryDatatableParam param, string mrid)
        {
            return lobj.AllArticleListPagewise(param, mrid);
        }

        public void DeleteArticle(int arid)
        {
            lobj.DeleteArticle(arid);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public object GetMagzineArticleDetails(string arid)
        {
            return lobj.GetMagzineArticleDetails(arid);
        }

        public void DeleteReleaseMagzine(int mRId)
        {
            lobj.DeleteReleaseMagzine(mRId);
            Result = lobj.Result;
            Message = lobj.Message;
        }

        public void DeletePriceaster(int magzineId, int priceid)
        {
            lobj.DeletePriceaster(magzineId,priceid);
            Result = lobj.Result;
            Message = lobj.Message;
        }
    }
}

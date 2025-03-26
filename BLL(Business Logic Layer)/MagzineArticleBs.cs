using BOL_Business_Objects_Layer_;
using DAL_Data_Access_Layer_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_
{
    public class MagzineArticleBs
    {
        private MagzineArticleDb lobj;

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
        public MagzineArticleBs()
        {
            lobj = new MagzineArticleDb();
        }
        public void Add(MagzineArticle obj)
        {
            lobj.Add(obj);
            Result = lobj.Result;
            Message = lobj.Message;
        }

       

        public void UpdateMagzineArticle(MagzineArticle obj)
        {
            lobj.UpdateMagzineArticle(obj);
            Result = lobj.Result;
            Message = lobj.Message;
        }
    }
}

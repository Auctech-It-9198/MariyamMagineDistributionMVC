using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace BOL_Business_Objects_Layer_
{
    public class MagzineArticle
    {
        public string arid { get; set; }
        public string mid { get; set; }
        public string mrid { get; set; }
        public string atitlecode { get; set; }
        public string artitle { get; set; }

        public string descrp { get; set; }
        public string arurl { get; set; }
        public string releasedate { get; set; }
        public string ispublish { get; set; }

        public string CoverImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }


        public string releasedcode { get; set; }
        public string releadetittle { get; set; }

        public string publishdate { get; set; }


    }


    public class MagzineArticleReleased
    {
        public string mrid { get; set; }
        public string title { get; set; }

    }


}

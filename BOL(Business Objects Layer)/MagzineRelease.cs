using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace BOL_Business_Objects_Layer_
{
    public class MagzineRelease
    {
        public string MagzineReleaseId { get; set; }
        public string MagzineId { get; set; }
        public string MagzineName { get; set; }
        public string MagzineNumber { get; set; }
        public string MagzineReleaseTitle { get; set; }
        public string DisplayTitle { get; set; }
        public string MagzineReleaseCode { get; set; }
        public string Description { get; set; }
        public string PdfUrl { get; set; }
        public string VideoUrl { get; set; }
        public string CoverImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ReleaseDate { get; set; }
        public string ReleaseMonth { get; set; }
        public string ReleaseYear { get; set; }
        public string Publish { get; set; }
        public string Stock { get; set; }
    }
}

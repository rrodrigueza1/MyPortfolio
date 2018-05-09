using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPortfolio.Models
{
    public class ImageModel
    {
        public int ImageId { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public byte[] Picture { get; set; }
        public int ProjectId { get; set; }
        public HttpPostedFileBase UploadImages { get; set; }
    }
}
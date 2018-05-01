using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Berry.Models
{
    public class FileResultViewModel
    {
        public string FullPath { get; set; }
        public string FileName { get; set; }
        public string Format { get; set; }

        private string mime;

        public string Mime
        {
            get
            {
                switch (this.Format)
                {
                    case ".pdf":
                        this.mime = "application/pdf";
                        break;
                    case ".xls":
                        this.mime = "application/vnd.ms-excel";
                        break;
                }

                return this.mime;
            }
        }
    }
}
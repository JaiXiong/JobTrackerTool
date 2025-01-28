using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobData.Common
{
    public struct DownloadOptions
    {
        public DownloadType Include { get; set; }
        public DownloadType Csv { get; set; }
        public DownloadType Pdf { get; set; }
    }
}

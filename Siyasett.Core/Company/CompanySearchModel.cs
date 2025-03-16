using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Company
{
    public class CompanySearchModel
    {
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyManager { get; set; }
        public string CompanyWebSite { get; set; }
        public string CompanyDate { get; set; }
        public byte OrderBy { get; set; }
        public byte OrderByDesc { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Company
{
    public class CompanyCreateModel:BaseModel
    {
      

        public string? ShortName { get; set; }
        public string? LeaderName { get; set; }
        public int? LeaderPeopleId { get; set; }
        public bool Active { get; set; } = true;
        public string? Photo { get; set; }
        public int[]? SectorId { get; set; }
        public string? Sector { get; set; }
        public string? WebAdress { get; set; }
        public string? UploadPhoto { get; set; }
        public string? Dof { get; set; }



    }
}

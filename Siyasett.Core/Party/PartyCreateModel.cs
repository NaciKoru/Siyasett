using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Party
{
    public class PartyCreateModel:BaseModel
    {
       

        [Required(ErrorMessage = "Party adı kısaltması zorunludur.")]
        public string ShortName { get; set; }
        public string? LeaderName { get; set; }
        public string? Dof { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsCanJoinElection { get; set; } = true;
        public string? UploadPhoto { get; set; }
        public string? Photo { get; set; }
        public int? LeaderPeopleId { get; set; }
        public List<JoinedElection>? JoinedElections { get; set; }
        public int? MemberCount { get; set; }
        public int? Parliamentarian { get; set; }
        public string? WebSite { get; set; }
        public bool IsClosed { get; set; }
        public string? NameEn { get; set; }

    }

    public class JoinedElection:BaseModel
    {

    }
}

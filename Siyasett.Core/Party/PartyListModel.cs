using Siyasett.Core.SocialMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Party
{
    public class PartyListModel:BaseModel
    {
        public string PartyName { get; set; }
        public string PartyNameShort { get; set; }

        public string LeaderName { get; set; }
        public int? LeaderPeopleId { get; set; }
        public bool Active { get; set; } = true;
        public string Dof { get; set; }
        public int? MemberCount { get; set; }
        public string WebSite { get; set; }
        public int? Parliamenteries { get; set; }
        public bool ParticipateElection { get; set; }
        public string? Logo { get; set; }
        public double? RatioAvg { get; set; }
        public string? NameEn { get; set; }
        public int? SecimDetayId { get; set; }
        public List<ComboBaseModel> Ideoloies { get; set; }

        public List<SocialMediaModel> SocialMedias { get; set; }

    }
}

using Siyasett.Core.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Colums
{
    public class ColumsModel:BaseModel
    {
        public int DilId { get; set; }
        public int TypeId { get; set; }
        public int[] CategoryIds { get; set; }
        public int CompanyId { get; set; }
        public int PeopleId { get; set; }
        public DateTime Date { get; set; }
        public string? OrjLink { get; set; }
        public int ReadCount { get; set; }
        public string Header { get; set; }
        public string? HeaderEn { get; set; }
        public string? Context { get; set; }
        public string? ContextEn { get; set; }
        public string? MetinOzet { get; set; }
        public string? MetinOzetEn { get; set; }
        public string? Keywords { get; set; }
        public string? YazarAdSoyad { get; set; }
        public string? TurAdi { get; set; }
        public string? SirketAdi { get; set; }
        public string? DilAdi { get; set; }
        public string? AuthorPhoto { get; set; }
        public List<int>? Kategoriler { get; set; }

    }
}

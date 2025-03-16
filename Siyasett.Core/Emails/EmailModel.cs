using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Emails
{
    public class EmailModel
    {
        public int Id { get; set; }
        public string? EmailAddress { get; set; }
        
        public int CommunicationTypeId { get; set; }
        public string? CommunicationTypeNameTr { get; set; }
        public string? CommunicationTypeNameEn { get; set; }

        public int ParentId { get; set; }

    }
}

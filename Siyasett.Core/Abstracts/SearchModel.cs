using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyasett.Core.Abstracts
{
    public abstract class SearchModel
    {
        public byte OrderBy { get; set; }

        public byte OrderByDesc { get; set; }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Siyasett.Web.Models
{
    public class MailSettings
    {

            public string Mail { get; set; }
            public string DisplayName { get; set; }
            public string Host { get; set; }
            public int Port { get; set; }
            
            public string Password { get; set; }
     

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scour.Code {
    class LdapAuthorization : Authorization {
        public override string Token { get; set; }
    }
}

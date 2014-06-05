using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scour.Code {
    abstract class Domain {
        public abstract IAuthority Authority { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scour.Code {
    abstract class Authorization {

        public abstract string Token {
            get;
            set;
        }
    }
}

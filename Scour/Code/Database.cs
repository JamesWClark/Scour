using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scour.Code {
    abstract class Database {
        public abstract string ConnectionString { get; set; }
        public abstract string Url { get; set; }
    }
}

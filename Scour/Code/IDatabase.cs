using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scour.Code {
    interface IDatabase {
        void Connect();
        void Update();
        void Insert();
        void Delete();
    }
}

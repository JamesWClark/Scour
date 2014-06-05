using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scour.Code {
    class Scout {
        public Computer PreviousRecord;
        public Computer CurrentRecord;
        public Authorization Authorization;

        public Computer GetPreviousRecord(Computer computer) {
            throw new NotImplementedException();
        }
        public Computer GetCurrentRecord() {
            throw new NotImplementedException();
        }
        public bool CompareRecords(Computer a, Computer b) {
            throw new NotImplementedException();
        }
        public bool SaveRecord(Computer computer) {
            throw new NotImplementedException();
        }
    }
}

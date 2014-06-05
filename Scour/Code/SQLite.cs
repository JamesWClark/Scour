using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scour.Code {
    class SQLite : Database, IDatabase {
        public override string ConnectionString {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public override string Url {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public void Connect() {
            throw new NotImplementedException();
        }
        public void Delete() {
            throw new NotImplementedException();
        }
        public void Insert() {
            throw new NotImplementedException();
        }
        public void Update() {
            throw new NotImplementedException();
        }
    }
}

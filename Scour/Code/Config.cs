using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scour.Code {
    class Config {
        List<Admin> Admins;
        List<Domain> Domains;
        public Config() {
            //@TODO: get list from config file, refer to http://dracoater.blogspot.com/2009/11/add-list-to-appconfig.html for example

            //read xml values
            LdapDomain d = new LdapDomain();
        }
    }
}

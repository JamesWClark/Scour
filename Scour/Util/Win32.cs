using System;
using System.Collections;
using System.Configuration;
using System.Management;

namespace Scour.Util {
    class Win32 {
        private static ManagementScope scope;
        private static ManagementObjectSearcher searcher;
        private static ManagementObjectCollection information;
        private static ConnectionOptions options;

        public Win32(string computerName, string admin, string password, string domain) {
            options = new ConnectionOptions();
            options.Username = admin;
            options.Password = password;
            options.Authority = "ntlmdomain:" + domain.ToUpper();
            scope = new ManagementScope(@"\\" + computerName + "." + domain + @"\root\cimv2", options);
        }

        public ManagementObjectCollection GetQueryResult(string query) {
            var q = new ObjectQuery(query);
            searcher = new ManagementObjectSearcher(scope, q);
            information = searcher.Get();
            return information;
        }
    }
}

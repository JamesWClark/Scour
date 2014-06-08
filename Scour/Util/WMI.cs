using System;
using System.Collections;
using System.Configuration;
using System.Management;

namespace Scour.Util {
    class WMI {
        private ManagementScope _managementScope;
        private ConnectionOptions _connectionOptions;

        public ManagementScope ManagementScope { get { return _managementScope; } }

        /// <summary>
        /// Constructor for local machine, no credentials necessary, requires elevation
        /// </summary>
        /// <param name="computerName"></param>
        public WMI(string computerName) {
            this.CreateManagementScope(computerName);
        }
        /// <summary>
        /// Constructor for remote machine, connection options required
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        public WMI(string computerName, string username, string password, string domain) {
            this.CreateManagementScope(computerName, username, password, domain);
        }
        /// <summary>
        /// Get a query result for any query string
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ManagementObjectCollection GetQueryResult(string query) {
            var q = new ObjectQuery(query);
            var searcher = new ManagementObjectSearcher(_managementScope, q);
            var information = searcher.Get();
            return information;
        }
        /// <summary>
        /// Create a local management scope
        /// </summary>
        /// <param name="computerName"></param>
        /// <returns></returns>
        private ManagementScope CreateManagementScope(string computerName) {
            this._managementScope = new ManagementScope(@"\\" + computerName + @"\root\cimv2");
            return _managementScope;
        }
        /// <summary>
        /// Create a credentialed management scope for remote connections
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        private ManagementScope CreateManagementScope(string computerName, string username, string password, string domain) {
            this._connectionOptions = new ConnectionOptions() {
                Username = username,
                Password = password,
                Authority = "ntlmdomain:" + domain.ToUpper()
            };
            this._managementScope = new ManagementScope(@"\\" + computerName + "." + domain + @"\root\cimv2", _connectionOptions);
            return _managementScope;
        }
    }
}

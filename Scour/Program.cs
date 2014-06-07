using System;
using System.Collections;
using System.Configuration;
using System.DirectoryServices;
using System.Management;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading;
//using System.Data.SQLite;
using Scour.Code;

namespace Scour {
    class Program {
        static void Main(string[] args) {

            const string computerFilter = "(objectClass=computer)";
            var domainUrl = ConfigurationManager.AppSettings["domain.ldap.url"];
            var subUrl = ConfigurationManager.AppSettings["sub.domain.ldap.url"];
            var domain = new Util.LDAP(domainUrl);
            var sub = new Util.LDAP(subUrl);
            var domainWin32 = new Thread(new ParameterizedThreadStart(GoToAndCollectWin32));
            var subWin32 = new Thread(new ParameterizedThreadStart(GoToAndCollectWin32));
            ArrayList domainComputers = domain.SearchByFilter(computerFilter);
            ArrayList subComputers = sub.SearchByFilter(computerFilter);

            domainWin32.Start(domainComputers);
            subWin32.Start(subComputers);
        }
        /// <summary>
        /// Visits a list of Active Directory computers and collects their Win32 properties
        /// </summary>
        /// <param name="obj">Casts input to ArrayList type.</param>
        static void GoToAndCollectWin32(object obj) {
            try {
                var computerNames = (ArrayList)obj;
                foreach (string computerName in computerNames) {
                    GetWin32PropertiesFromComputer(computerName);
                }
            } catch (InvalidCastException ex) {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Get a single computer's Win32 properties
        /// </summary>
        /// <param name="computerName"></param>
        /// <returns></returns>
        static string GetWin32PropertiesFromComputer(string computerName) {
            var builder = new StringBuilder();
            try {
                var computer = new Computer();
                var username = ConfigurationManager.AppSettings["rockhurst.int.admin"];
                var password = ConfigurationManager.AppSettings["rockhurst.int.admin.password"];
                var domain = ConfigurationManager.AppSettings[""];
                var win32 = new Util.Win32(computerName, username, password, domain);

                builder.Append(GetWmiQueryResult(win32.GetBaseBoard()));
                builder.Append(GetWmiQueryResult(win32.GetDiskDrive()));
                builder.Append(GetWmiQueryResult(win32.GetProcessor()));
                builder.Append(GetWmiQueryResult(win32.GetMotherboardDevice()));
                builder.Append(GetWmiQueryResult(win32.GetPhysicalMemory()));
                builder.Append(GetWmiQueryResult(win32.GetVideoController()));
                builder.Append(GetWmiQueryResult(win32.GetOperatingSystem()));
                builder.Append(GetWmiQueryResult(win32.GetWindowsProductActivation()));
            } catch (Exception ex) {
                string error = "Caught an error: " + ex.Message;
                try {
                    error += ":::" + win32.Query.QueryString;
                } catch (Exception) {
                    //do nothing?
                }
                Console.WriteLine(error);
            }
            return builder.ToString();
        }
        static string GetWmiQueryResult(ManagementObjectCollection information) {
            StringBuilder builder = new StringBuilder();
            foreach (ManagementObject obj in information) {
                foreach (PropertyData data in obj.Properties) {
                    //Console.WriteLine("{0} = {1}", data.Name, data.Value);
                }
                builder.Append(obj.Properties.ToJson());
            }
            return builder.ToString();
        }

        /**  */
        static ArrayList GetComputerNames(string ldapString) {
            ArrayList names = new ArrayList();
            DirectoryEntry entry = new DirectoryEntry(ConfigurationManager.AppSettings[ldapString]);
            DirectorySearcher mySearcher = new DirectorySearcher(entry);
            mySearcher.Filter = ("(objectClass=computer)");

            foreach (SearchResult resEnt in mySearcher.FindAll()) {
                string name = (resEnt.GetDirectoryEntry().Name.ToString().Remove(0, 3));//remove(0,3)removes the CN= portion of the string
                names.Add(name);
            }
            return names;
        }
    }
}

/* works:
SQLiteDatabase db = new SQLiteDatabase(@"C:\LocalStorage\Scour\Scour\bin\Debug\test");
string sql = "INSERT INTO Users (FirstName,LastName) VALUES ('Mark','Bayhylle');";
db.ExecuteNonQuery(sql);
* */

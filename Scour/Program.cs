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

namespace Scour {
    class Program {
        static void Main(string[] args) {

            ArrayList rockhurstComputers = GetComputerNames(ConfigurationManager.AppSettings["rockhurst.int.ldap"]);
            ArrayList studentComputers = GetComputerNames(ConfigurationManager.AppSettings["student.rockhurst.int.ldap"]);

            //start 2 threads
            Thread rockhurst = new Thread(new ParameterizedThreadStart(goToAndCollect));
            Thread student = new Thread(new ParameterizedThreadStart(goToAndCollect));
            rockhurst.Start(rockhurstComputers);
            student.Start(studentComputers);
        }
        static void goToAndCollect(object obj) {
            string hostKey = "mongodb.host";
            string nameKey = "mongodb.database.name";
            string collectionKey = "mongodb.database.collection.computers";

            var connectionString = ConfigurationManager.AppSettings[hostKey];
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(ConfigurationManager.AppSettings[nameKey]);
            var collection = database.GetCollection(ConfigurationManager.AppSettings[collectionKey]);

            ArrayList computerNames = (ArrayList)obj;
            foreach (string name in computerNames) {
                Console.WriteLine(getComputerWin32(name, collection));
                Console.WriteLine("\n\n===================================");
            }
        }
        static string getComputerWin32(string computerName, MongoCollection collection) {

            //prob use these as properties of Computer class
            string baseBoard;
            string diskDrive;
            string processor;
            string motherboardDevice;
            string physicalMemory;
            string videoController;
            string operatingSystem;
            string activation;

            Util.Win32 win32 = null;

            StringBuilder builder = new StringBuilder();
            try {

                string admin = ConfigurationManager.AppSettings["rockhurst.int.admin"];
                string password = ConfigurationManager.AppSettings["rockhurst.int.admin.password"];
                string domain = "rockhurst.int";
                win32 = new Util.Win32(computerName, admin, password, domain);

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

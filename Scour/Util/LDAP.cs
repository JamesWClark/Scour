using System;
using System.Collections;
using System.Configuration;
using System.DirectoryServices;

namespace Scour.Util {
    class LDAP {

        private string ldapUrl;

        public LDAP(string ldapUrl) {
            this.ldapUrl = ldapUrl;
        }

        /// <summary>
        /// Searches active directory by string filter.
        /// </summary>
        /// <example>ldap.SearchByFilter("(objectClass=computer)");</example>
        /// <param name="filter"></param>
        /// <returns></returns>
        public ArrayList SearchByFilter(string filter) {
            ArrayList names = new ArrayList();
            DirectoryEntry entry = new DirectoryEntry(ldapUrl);
            DirectorySearcher mySearcher = new DirectorySearcher(entry);
            mySearcher.Filter = (filter);
            foreach (SearchResult resEnt in mySearcher.FindAll()) {
                string name = resEnt.GetDirectoryEntry().Name;//remove(0,3)removes the CN= portion of the string
                names.Add(name);
            }
            return names;
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

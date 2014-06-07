﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Management;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading;
using System.Windows.Forms;
//using System.Data.SQLite;
using Scour.Code;
using Scour.Code.Components;

//there's a good example of WMI code here:
//http://code.msdn.microsoft.com/windowsdesktop/Using-WMI-with-C-4e5a9ee1

namespace Scour {
    class Program {

        const string computerFilter = "(objectClass=computer)";

        static void Main(string[] args) {


        }

        static void CollectLocalMachine() {
            var computerName = SystemInformation.ComputerName;
            var serverString = @"\\" + computerName + @"\root\cimv2";
            var scope = new ManagementScope(serverString);
            GetWin32PropertiesFromComputer(scope);
        }
        static void CollectRemoteMachines() {

            
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
                    Computer c = GetWin32PropertiesFromComputer(computerName);
                    Console.WriteLine(c);
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
        static Computer GetWin32PropertiesFromComputer(string computerName) {
            var queries = new Dictionary<string, string>()
            {
                {"Baseboard","SELECT Manufacturer,Model,Product,SerialNumber FROM Win32_BaseBoard"},
                {"BIOS","SELECT Manufacturer,SerialNumber,SMBIOSBIOSVersion FROM Win32_BIOS"},
                {"DiskDrive","SELECT Model,Size,Manufacturer FROM Win32_DiskDrive"},
                {"MotherboardDevice", "SELECT PrimaryBusType,SecondaryBusType FROM Win32_MotherboardDevice"},
                //{"OperatingSystem","SELECT * FROM Win32_OperatingSystem"},
                {"PhysicalMemory","SELECT Capacity,DataWidth,FormFactor,MemoryType,Speed FROM Win32_PhysicalMemory"},
                {"Processor","SELECT Manufacturer,Name,Description,MaxClockSpeed,L2CacheSize,AddressWidth,DataWidth,NumberOfCores,NumberOfLogicalProcessors,ProcessorId FROM Win32_Processor"},
                {"VideoController","SELECT AdapterCompatibility,AdapterRAM,Name,VideoModeDescription,VideoProcessor,VideoMemoryType FROM Win32_VideoController"}
            };

            var computer = new Computer();
            var username = ConfigurationManager.AppSettings["domain.admin.username"];
            var password = ConfigurationManager.AppSettings["domain.admin.password"];
            var domain = ConfigurationManager.AppSettings["domain.ldap.url"].Split(new string[] { "LDAP://" }, StringSplitOptions.None)[1]; //get only the domain.com part
            var win32 = new Util.Win32(computerName, username, password, domain);

            computer.Baseboard = new Baseboard(win32.GetQueryResult(queries["Baseboard"]));
            computer.BIOS = new BIOS(win32.GetQueryResult(queries["BIOS"]));
            computer.DiskDrive = new DiskDrive(win32.GetQueryResult(queries["DiskDrive"]));
            computer.MotherboardDevice = new MotherboardDevice(win32.GetQueryResult(queries["MotherboardDevice"]));
            computer.PhysicalMemory = new PhysicalMemory(win32.GetQueryResult(queries["PhysicalMemory"]));
            computer.Processor = new Processor(win32.GetQueryResult(queries["Processor"]));
            computer.VideoController = new VideoController(win32.GetQueryResult(queries["VideoController"]));

            return computer;
        }
        static ArrayList GetComputerNames(string ldapString) {
            var names = new ArrayList();
            var entry = new DirectoryEntry(ConfigurationManager.AppSettings[ldapString]);
            var mySearcher = new DirectorySearcher(entry);
            mySearcher.Filter = (computerFilter);

            foreach (SearchResult resEnt in mySearcher.FindAll()) {
                var name = (resEnt.GetDirectoryEntry().Name.Remove(0, 3));//remove(0,3)removes the CN= portion of the string
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

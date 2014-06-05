using System;
using System.Collections;
using System.Configuration;
using System.Management;

namespace Scour.Util {
    class Win32 {
        private static ManagementScope scope;
        private static ObjectQuery query;
        private static ManagementObjectSearcher searcher;
        private static ManagementObjectCollection information;
        private static ConnectionOptions options;

        public Win32(string computerName, string admin, string password, string domain)
        {
            options = new ConnectionOptions();
            options.Username = admin;
            options.Password = password;
            options.Authority = "ntlmdomain:" + domain.ToUpper();
            scope = new ManagementScope(@"\\" + computerName + "." + domain + @"\root\cimv2", options);
        }

        public ManagementObjectCollection GetBaseBoard()
        {
            query = new ObjectQuery("SELECT Manufacturer FROM Win32_BaseBoard");
            searcher = new ManagementObjectSearcher(scope, query);
            information = searcher.Get();

            //IBM Lenovo stores its serial number in the BIOS, not the motherboard. Still need to test other model PCs.
            //this checks for Lenovo first
            string manufacturer = String.Empty;
            foreach (ManagementObject obj in information)
            {
                foreach (PropertyData data in obj.Properties)
                {
                    manufacturer = data.Value as string;
                }
            }
            //If it's a Lenovo PC, use the Win32_BIOS table instead
            if (manufacturer == "LENOVO" || manufacturer == "IBM")
            {
                query = new ObjectQuery("SELECT Manufacturer,SerialNumber,SMBIOSBIOSVersion FROM Win32_BIOS");
                searcher = new ManagementObjectSearcher(scope, query);
                information = searcher.Get();
            }
            else
            {
                query = new ObjectQuery("SELECT Manufacturer,Model,Product,SerialNumber FROM Win32_BaseBoard");
                searcher = new ManagementObjectSearcher(scope, query);
                information = searcher.Get();
            }
            return information;
        }
        public ManagementObjectCollection GetDiskDrive()
        {
            query = new ObjectQuery("SELECT Model,Size,Manufacturer FROM Win32_DiskDrive");
            searcher = new ManagementObjectSearcher(scope, query);
            information = searcher.Get();

            return information;
        }
        public ManagementObjectCollection GetProcessor()
        {
            query = new ObjectQuery("SELECT Manufacturer,Name,Description,MaxClockSpeed,L2CacheSize,AddressWidth,DataWidth,NumberOfCores,NumberOfLogicalProcessors,ProcessorId FROM Win32_Processor");
            searcher = new ManagementObjectSearcher(scope, query);
            information = searcher.Get();

            return information;
        }
        public ManagementObjectCollection GetMotherboardDevice()
        {
            query = new ObjectQuery("SELECT PrimaryBusType,SecondaryBusType FROM Win32_MotherboardDevice");
            searcher = new ManagementObjectSearcher(scope, query);
            information = searcher.Get();

            return information;
        }
        public ManagementObjectCollection GetPhysicalMemory()
        {
            query = new ObjectQuery("SELECT Capacity,DataWidth,FormFactor,MemoryType,Speed FROM Win32_PhysicalMemory");
            searcher = new ManagementObjectSearcher(scope, query);
            information = searcher.Get();

            return information;
        }
        public ManagementObjectCollection GetVideoController()
        {
            query = new ObjectQuery("SELECT AdapterCompatibility,AdapterRAM,Name,VideoModeDescription,VideoProcessor,VideoMemoryType FROM Win32_VideoController");
            searcher = new ManagementObjectSearcher(scope, query);
            information = searcher.Get();

            return information;
        }
        /*************** OPERATING SYSTEM CLASSES **********************/
        /**/
        public ManagementObjectCollection GetOperatingSystem()
        {
            query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            searcher = new ManagementObjectSearcher(scope, query);
            information = searcher.Get();

            return information;
        }

        /**********Windows Product Activation*****************************/
        /*
         *  This class doesn't work in Vista and 7 (presumably also in Windows 2008, though I did not check). 
         *  It has been replaced with the Software Licensing Classes, which you can find documented at:
         *  http://msdn.microsoft.com/en-us/library/cc534598.aspx
         */
        public ManagementObjectCollection GetWindowsProductActivation()
        {
            query = new ObjectQuery("SELECT * FROM Win32_WindowsProductActivation");
            searcher = new ManagementObjectSearcher(scope, query);
            information = searcher.Get();

            return information;
        }

        /***************ACCESSORS*******************/
        public ObjectQuery Query
        {
            get
            {
                return query;
            }
        }
    }
}

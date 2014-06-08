using System.Management;
using Scour.Util;

namespace Scour.Code.Components {
    class BIOS {
        public string Manufacturer { get; set; }
        public string SerialNumber { get; set; }
        public string SMBIOSBIOSVersion { get; set; }

        public BIOS(ManagementObjectCollection queryResult) {
            foreach (var obj in queryResult) {
                foreach (var data in obj.Properties) {
                    switch (data.Name) {
                        case "Manufacturer":
                            this.Manufacturer = data.Value.ToString();
                            break;
                        case "SerialNumber":
                            this.SerialNumber = data.Value.ToString();
                            break;
                        case "SMBIOSBIOSVersion":
                            this.SMBIOSBIOSVersion = data.Value.ToString();
                            break;
                    }
                }
            }
        }
    }
}

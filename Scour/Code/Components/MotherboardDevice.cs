using System.Management;
using Scour.Util;

namespace Scour.Code.Components {
    class MotherboardDevice {
        public string Manufacturer { get; set; }

        public MotherboardDevice(ManagementObjectCollection queryResult) {
            foreach (var obj in queryResult) {
                foreach (var data in obj.Properties) {
                    switch (data.Name) {
                        case "Manufacturer":
                            this.Manufacturer = data.Value.ToString();
                            break;
                    }
                }
            }
        }
    }
}

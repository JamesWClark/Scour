using System.Management;
using Scour.Util;

namespace Scour.Code.Components {
    class DiskDrive {
        public string Model { get; set; }
        public string Size { get; set; }
        public string Manufacturer { get; set; }

        public DiskDrive(ManagementObjectCollection queryResult) {
            foreach (var obj in queryResult) {
                foreach (var data in obj.Properties) {
                    switch (data.Name) {
                        case "Model":
                            this.Model = data.Value.ToString();
                            break;
                        case "Size":
                            this.Size = data.Value.ToString();
                            break;
                        case "Manufacturer":
                            this.Manufacturer = data.Value.ToString();
                            break;
                    }
                }
            }
        }
    }
}

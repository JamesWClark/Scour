using System;
using System.Management;
using Scour.Util;

namespace Scour.Code.Components {
    class Baseboard {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Product { get; set; }
        public string SerialNumber { get; set; }

        public Baseboard(ManagementObjectCollection queryResult) {
            foreach (var obj in queryResult) {
                foreach (var data in obj.Properties) {
                    switch (data.Name) {
                        case "Manufacturer":
                            this.Manufacturer = data.Value.ToString();
                            break;
                        case "Model":
                            try {
                                this.Model = data.Value.ToString();
                            } catch (NullReferenceException) {
                                this.Model = String.Empty;
                            }
                            break;
                        case "Product":
                            this.Product = data.Value.ToString();
                            break;
                        case "SerialNumber":
                            this.SerialNumber = data.Value.ToString();
                            break;
                    }
                }
            }
        }

    }
}

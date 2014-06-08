using System.Management;
using Scour.Util;

namespace Scour.Code.Components {
    class Processor {
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MaxClockSpeed { get; set; }
        public string L2CacheSize { get; set; }
        public string AddressWidth { get; set; }
        public string DataWidth { get; set; }
        public string NumberOfCores { get; set; }
        public string NumberOfLogicalProcessors { get; set; }
        public string ProcessorId { get; set; }

        public Processor(ManagementObjectCollection queryResult) {
            foreach (var obj in queryResult) {
                foreach (var data in obj.Properties) {
                    switch (data.Name) {
                        case "Manufacturer":
                            this.Manufacturer = data.Value.ToString();
                            break;
                        case "Name":
                            this.Name = data.Value.ToString();
                            break;
                        case "Description":
                            this.Description = data.Value.ToString();
                            break;
                        case "MaxClockSpeed":
                            this.MaxClockSpeed = data.Value.ToString();
                            break;
                        case "L2CacheSize":
                            this.L2CacheSize = data.Value.ToString();
                            break;
                        case "AddressWidth":
                            this.AddressWidth = data.Value.ToString();
                            break;
                        case "DataWidth":
                            this.DataWidth = data.Value.ToString();
                            break;
                        case "NumberOfCores":
                            this.NumberOfCores = data.Value.ToString();
                            break;
                        case "NumberOfLogicalProcessors":
                            this.NumberOfLogicalProcessors = data.Value.ToString();
                            break;
                        case "ProcessorId":
                            this.ProcessorId = data.Value.ToString();
                            break;
                    }
                }
            }
        }
    }
}

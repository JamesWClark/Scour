using System.Management;
using Scour.Util;

namespace Scour.Code.Components {
    class PhysicalMemory {
        public string Capacity { get; set; }
        public string DataWidth { get; set; }
        public string FormFactory { get; set; }
        public string MemoryType { get; set; }
        public string Speed { get; set; }

        public PhysicalMemory(ManagementObjectCollection queryResult) {
            foreach (var obj in queryResult) {
                foreach (var data in obj.Properties) {
                    switch (data.Name) {
                        case "Capacity":
                            this.Capacity = data.Value.ToString();
                            break;
                        case "DataWidth":
                            this.DataWidth = data.Value.ToString();
                            break;
                        case "FormFactory":
                            this.FormFactory = data.Value.ToString();
                            break;
                        case "MemoryType":
                            this.MemoryType = data.Value.ToString();
                            break;
                        case "Speed":
                            this.Speed = data.Value.ToString();
                            break;
                    }
                }
            }
        }
    }
}

using System;
using System.Management;
using Scour.Util;

namespace Scour.Code.Components {
    class VideoController {
        public string AdapterCompatibility { get; set; }
        public string AdapterRam { get; set; }
        public string Name { get; set; }
        public string VideoModeDescription { get; set; }
        public string VideoProcessor { get; set; }
        public string VideoMemoryType { get; set; }

        public VideoController(ManagementObjectCollection queryResult) {
            foreach (var obj in queryResult) {
                foreach (var data in obj.Properties) {
                    switch (data.Name) {
                        case "AdapterCompatibility":
                            this.AdapterCompatibility = data.Value.ToString();
                            break;
                        case "AdapterRam":
                            this.AdapterRam = data.Value.ToString();
                            break;
                        case "Name":
                            this.Name = data.Value.ToString();
                            break;
                        case "VideoModeDescription":
                            this.VideoModeDescription = data.Value.ToString();
                            break;
                        case "VideoProcessor":
                            try {
                                this.VideoProcessor = data.Value.ToString();
                            } catch (NullReferenceException) {
                                this.VideoProcessor = String.Empty;
                            }
                            break;
                        case "VideoMemoryType":
                            this.VideoMemoryType = data.Value.ToString();
                            break;
                    }
                }
            }
        }
    }
}

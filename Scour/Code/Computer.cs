using MongoDB.Bson;
using Scour.Code.Components;


namespace Scour.Code {
    class Computer {
        public Baseboard Baseboard { get; set; }
        public BIOS BIOS { get; set; }
        public DiskDrive DiskDrive { get; set; }
        public MotherboardDevice MotherboardDevice { get; set; }
        public PhysicalMemory PhysicalMemory { get; set; }
        public Processor Processor { get; set; }
        public VideoController VideoController { get; set; }

        public override string ToString() {
            return this.ToJson();
        }
    }
}

using CapFrameX.Contracts.Sensor;
using System;

namespace CapFrameX.Sensor
{
    public class CustomSensorEntry : ICustomSensorEntry
    {
        public string Name { get; set; }
        public string Formula { get; set; }
        public string Unit { get; set; }
        public bool IsActive { get; set; }
        public string Identifier => $"Custom/{Name}";
    }
}

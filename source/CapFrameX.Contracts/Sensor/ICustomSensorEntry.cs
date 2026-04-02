namespace CapFrameX.Contracts.Sensor
{
    public interface ICustomSensorEntry
    {
        string Name { get; set; }
        string Formula { get; set; }
        string Unit { get; set; }
        bool IsActive { get; set; }
        string Identifier { get; }
    }
}

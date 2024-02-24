using DataModels.Ultrasound;

namespace Core.Display.Interfaces
{
    public interface IProbeOutput
    {
        ScanData sendScanData();
    }
}
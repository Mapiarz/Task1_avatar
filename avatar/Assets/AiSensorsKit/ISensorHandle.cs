using System;

namespace Aisens
{
    public interface ISensorHandle
    {
        bool CanGetDatagram { get; }
        bool IsConnectionOpen { get; }
        ISensorInfo SensorInfo { get; }

        event Action<ISensorHandle> ConnectionClosed;

        ISensorDatagram GetDatagram();
        void Connect();
        void Disconnect();
    }
}
using System;

namespace Aisens
{
    [Flags]
    public enum SensorProtocol
    {
        Udp     = 1 << 0,
        Ble     = 1 << 1
    }

    public static class EnumExtensions
    {
        public static bool IsAny( this SensorProtocol protocol, SensorProtocol other )
        {
            return ( protocol & other ) != 0;
        }

        public static bool IsAll( this SensorProtocol protocol, SensorProtocol other )
        {
            return ( protocol & other ) == other;
        }
    }
}
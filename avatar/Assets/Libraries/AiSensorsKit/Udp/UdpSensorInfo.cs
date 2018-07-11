using System.Net;

namespace Aisens.Udp
{
    public class UdpSensorInfo : ISensorInfo
    {
        public IPEndPoint SensorEndpoint { get; set; }

        public UdpSensorInfo( IPEndPoint sensorEndpoint )
        {
            SensorEndpoint = sensorEndpoint;
        }

        public bool Equals( ISensorInfo other )
        {
            // Optimization for a common success case.
            if ( ReferenceEquals( this, other ) )
            {
                return true;
            }
            // If parameter is null, return false.
            if ( ReferenceEquals( null, other ) )
            {
                return false;
            }
            // If run-time types are not exactly the same, return false.
            if ( GetType() != other.GetType() )
            {
                return false;
            }

            var otherSensorInfo = other as UdpSensorInfo;  // It is now safe to cast as the types are the same

            // Sensor infos are the same if the port and IP address are the same
            return SensorEndpoint.Port == otherSensorInfo.SensorEndpoint.Port &&
                   SensorEndpoint.Address.Equals( otherSensorInfo.SensorEndpoint.Address );
        }

        public override bool Equals( object obj )
        {
            return Equals( obj as UdpSensorInfo );
        }

        public override int GetHashCode()
        {
            return SensorEndpoint.GetHashCode();
        }

        public static bool operator ==( UdpSensorInfo lhs, UdpSensorInfo rhs )
        {
            // Check for null on left side.
            if ( ReferenceEquals( lhs, null ) )
            {
                // If there's null on left side, return true if null is also on the right side, otherwise false
                return ReferenceEquals( rhs, null );
            }

            // Equals handles case of null on right side.
            return lhs.Equals( rhs );
        }

        public static bool operator !=( UdpSensorInfo lhs, UdpSensorInfo rhs )
        {
            return !( lhs == rhs );
        }
    }
}
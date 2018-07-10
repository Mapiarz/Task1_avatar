using System;

namespace Aisens.Udp
{
    /// <summary>
    /// Represents a raw UDP Sensor datagram
    /// </summary>
    /// <remarks>
    /// Datagram length is 62 bytes
    /// First two bytes are hexadecimal letters 'Ai'
    /// Next two bytes are some kind of header information
    /// Bytes 4-60 are sensor data
    /// Last two bytes are CRC16 checksum
    /// </remarks>
    public class UdpDatagram
    {
        public static readonly byte FrameLength = 62;

        public bool IsValid { get; private set; }

        byte[] Data { get; set; }

        public UdpDatagram( byte[] data )
        {
            // Note: Later on, the header should be parsed here to know what kind of data this is and so on
            IsValid = IsDataValid( data );

            Data = data;
        }

        public ISensorDatagram CreateSensorDatagram()
        {
            if ( !IsValid )
            {
                throw new InvalidOperationException( "The datagram is not valid" );
            }

            // Extract Quaternion components w, x, y, z from raw byte data
            // The quaternion components are floats (single precision - each float is 4 bytes)
            // The quaternion components start at byte 44, in order: w, x, z, y
            // Thus, the quaternion components are in bytes 44-60 of the datagram
            var quaternionComponents = new float[4];
            Buffer.BlockCopy( Data, 4 + 40, quaternionComponents, 0, 4 * sizeof( float ) );

            // Note: because sensors use a different internal representation of quaternion from Unity, thus components x, y, z have to be reversed
            var w = quaternionComponents[0];
            var x = -quaternionComponents[1];
            var z = -quaternionComponents[2];
            var y = -quaternionComponents[3];            

            var quaternion = new UnityEngine.Quaternion( x, y, z, w );

            return new SensorDatagram( quaternion );
        }

        bool IsDataValid( byte[] dataFrame )
        {
            // First, check if frame length is correct
            if ( dataFrame.Length != FrameLength )
            {
                return false;
            }

            // Check if first two bytes are hexadecimal letters A and i
            var isAI = ( dataFrame[0] == 0x41 ) && ( dataFrame[1] == 0x69 );
            if ( !isAI )
            {
                return false;
            }

            // Last two bytes are CRC 16 checksum
            var dataCrc = BitConverter.ToUInt16( dataFrame, 60 );
            var calculatedCrc = CalculateCRC16( dataFrame, 60 );

            return dataCrc == calculatedCrc;
        }

        /// <summary>
        /// A simple implementation of CRC 16 that conforms to the algorithm used in the sensor
        /// </summary>
        ushort CalculateCRC16( byte[] data, int length )
        {
            byte x = 0;
            ushort crc = 0xFFFF;

            for ( int i = 0; i < length; i++ )
            {
                x = (byte)( ( crc >> 8 ) ^ data[i] );
                x ^= (byte)( x >> 4 );
                crc = (ushort)( ( crc << 8 ) ^ ( x << 12 ) ^ ( x << 5 ) ^ x );
            }

            return crc;
        }
    }
}
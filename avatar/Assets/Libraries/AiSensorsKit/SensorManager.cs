using Aisens.Udp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aisens
{
    // TODO: Sensor handle needs to implement IDisposable
    public class SensorManager
    {
        // Note: Need to store active sensor handles so that users of this class will know how many active sensors there are
        public IList<ISensorHandle> ActiveSensorConnections { get; private set; } = new List<ISensorHandle>();

        public event Action<ISensorHandle> ConnectionClosed;

        // Note: Need to store a hashset of sensor infos so we can detect duplicate connection attempts - ISensorInfo implements IEquatable, 
        // ISensorHandle does not.
        HashSet<ISensorInfo> sensorConnections = new HashSet<ISensorInfo>();
        List<ISensorDetector> sensorDetectors = new List<ISensorDetector>();

        public SensorManager( SensorProtocol protocols )
        {
            if ( protocols == 0 )
            {
                throw new InvalidOperationException( "No protocol chosen" );
            }
            if ( protocols.IsAny( SensorProtocol.Udp ) )
            {
                sensorDetectors.Add( new UdpSensorDetector() );
            }
            if ( protocols.IsAny( SensorProtocol.Ble ) )
            {
                throw new NotImplementedException( "BLE sensors are not yet implemented" );
            }
        }

        /// <summary>
        /// Discovers available sensors and returns a list of <see cref="ISensorInfo"/> which can be used to connect to the sensors
        /// </summary>
        /// <param name="timeout">How long should the discovery at most take, in miliseconds</param>
        /// <returns>A list of <see cref="ISensorInfo"/> indicating available sensors</returns>
        public async Task<IList<ISensorInfo>> DiscoverSensorsAsync( int timeout )
        {
            var discoveryTasks = new List<Task<IList<ISensorInfo>>>();

            foreach ( var sensorDetector in sensorDetectors )
            {
                discoveryTasks.Add( sensorDetector.DiscoverSensorsAsync( timeout ) );
            }

            await Task.WhenAll( discoveryTasks );

            var result = new List<ISensorInfo>();

            foreach ( var task in discoveryTasks )
            {
                if ( task.IsFaulted )
                {
                    throw task.Exception;
                }

                result.AddRange( task.Result );
            }

            return result;
        }

        /// <summary>
        /// Connects with all specified sensors and returns a list of <see cref="ISensorHandle"/>
        /// </summary>
        /// <param name="sensorInfos">A list of sensors to connect to</param>
        /// <returns>A list of new sensor handles</returns>
        /// <remarks>Returned sensor handles are not guaranteed to have open connection. It is important to always check whether sensor 
        /// connection is open before using it.</remarks>
        public IList<ISensorHandle> ConnectToSensors( IList<ISensorInfo> sensorInfos )
        {
            // TODO: Make this method Atomic - either all handles are created and added or none.

            if ( sensorInfos == null )
            {
                throw new ArgumentNullException( nameof( sensorInfos ) );
            }

            var result = new List<ISensorHandle>();

            foreach ( var sensorInfo in sensorInfos )
            {
                result.Add( ConnectToSensor( sensorInfo ) );
            }

            return result;
        }

        public ISensorHandle ConnectToSensor( ISensorInfo sensorInfo )
        {
            if ( sensorInfo == null )
            {
                throw new ArgumentNullException( nameof( sensorInfo ) );
            }
            if ( sensorConnections.Contains( sensorInfo ) )
            {
                throw new InvalidOperationException( $"Sensor {sensorInfo} is already connected" );
            }

            ISensorHandle handle;

            // Note switch statements on types are available in C# 7 or later
            if ( sensorInfo is UdpSensorInfo )
            {
                handle = new UdpSensorHandle( sensorInfo );
            }
            else  // e.g. BLE or other
            {
                throw new InvalidOperationException( "Unsupported sensor info type" );
            }

            ActiveSensorConnections.Add( handle );
            sensorConnections.Add( sensorInfo );

            // It is possible that ConnectionClosed will be invoked immediately after calling Connect so we need to sign up to the event first
            handle.ConnectionClosed += OnHandleConnectionClosed;

            try
            {
                handle.Connect();
            }
            catch ( Exception ex )
            {
                // If exception has been thrown during connection, we 'disconnect' the sensor which also unregisters from the ConnectionClosed event
                DisconnectSensor( handle );

                throw ex;
            }

            return handle;
        }

        public void DisconnectSensor( ISensorHandle handle )
        {
            if ( handle == null )
            {
                throw new ArgumentNullException( nameof( handle ) );
            }
            if ( !ActiveSensorConnections.Contains( handle ) )
            {
                throw new InvalidOperationException( "The specified sensor handle is not managed by this SensorManager" );
            }

            handle.ConnectionClosed -= OnHandleConnectionClosed;

            ActiveSensorConnections.Remove( handle );
            sensorConnections.Remove( handle.SensorInfo );

            handle.Disconnect();  // This call must be idempotent
        }

        public void DisconnectAllSensors()
        {
            // Disconnecting modifies ActiveSensorConnections list so we need a copy
            var activeSensors = new List<ISensorHandle>( ActiveSensorConnections );

            foreach ( var handle in activeSensors )
            {
                DisconnectSensor( handle );
            }
        }

        void OnHandleConnectionClosed( ISensorHandle handle )
        {
            if ( handle == null )
            {
                throw new ArgumentNullException( nameof( handle ) );
            }

            DisconnectSensor( handle );
            ConnectionClosed?.Invoke( handle );
        }
    }
}
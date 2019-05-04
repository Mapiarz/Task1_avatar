using System;

namespace Aisens.Udp
{
    public class UdpSensorHandle : ISensorHandle, IDisposable
    {
        public bool CanGetDatagram
        {
            get
            {
                if ( Server == null )
                {
                    return false;
                }
                else
                {
                    return Server.HasDataAvailable;
                }
            }
        }

        public bool IsConnectionOpen { get; private set; }

        public ISensorInfo SensorInfo { get; private set; }

        public event Action<ISensorHandle> ConnectionClosed;

        UdpSensorServer Server { get; set; }

        public UdpSensorHandle( ISensorInfo sensorInfo )
        {
            if ( sensorInfo == null )
            {
                throw new ArgumentNullException( nameof( sensorInfo ) );
            }
            if ( !( sensorInfo is UdpSensorInfo ) )
            {
                throw new ArgumentException( $"{nameof( sensorInfo )} must be of type UdpSensorInfo", nameof( sensorInfo ) );
            }

            SensorInfo = sensorInfo;

            Server = new UdpSensorServer();
            Server.ServerFaulted += OnServerFaulted;
        }

        public void Connect()
        {
            // Calling StartServer might immediately invoke ServerFaulted, that's why we need to indicate the connection is open first and then
            // start the server - if it throws, we disconnect (and thus indicate the connection is closed), if it doesn't throw but immediately calls
            // ServerFaulted - That will disconnect
            IsConnectionOpen = true;

            try
            {
                Server.StartServer( SensorInfo );
            }
            catch ( Exception ex )
            {
                Disconnect();

                throw ex;
            }
        }

        public void Disconnect()
        {
            if ( isDisposed )
            {
                return;
            }

            Dispose( true );

            IsConnectionOpen = false;
        }

        public ISensorDatagram GetDatagram()
        {
            if ( !CanGetDatagram )
            {
                throw new InvalidOperationException( $"{nameof( CanGetDatagram )} is false" );
            }

            return Server.GetDatagram();
        }

        void OnServerFaulted()
        {
            Disconnect();

            ConnectionClosed?.Invoke( this );
        }

        #region IDisposable Support

        private bool isDisposed = false; // To detect redundant calls

        protected virtual void Dispose( bool disposing )
        {
            if ( !isDisposed )
            {
                if ( disposing )
                {
                    if ( Server.Status != UdpSensorServerStatus.Closed )
                    {
                        Server.CloseServer();
                    }
                }

                Server.ServerFaulted -= OnServerFaulted;
                Server = null;

                isDisposed = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            Dispose( true );
        }

        #endregion
    }
}
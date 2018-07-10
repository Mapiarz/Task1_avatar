using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Aisens.Udp
{
    public class UdpSensorServer : IDisposable
    {
        public UdpSensorServerStatus Status { get; private set; }
        public bool HasDataAvailable { get { return Buffer?.Size > 0; } }

        public event Action ServerFaulted;

        UdpClient Client { get; set; }
        CircularBuffer<UdpDatagram> Buffer { get; set; }
        CancellationTokenSource Cts { get; set; } = new CancellationTokenSource();

        public UdpSensorServer()
        {
            Buffer = new CircularBuffer<UdpDatagram>( 1000 );
            Status = UdpSensorServerStatus.Unstarted;
        }

        public void StartServer( ISensorInfo sensorInfo )
        {
            if ( Status != UdpSensorServerStatus.Unstarted )
            {
                throw new InvalidOperationException( "This server has already been started" );
            }
            if ( isDisposed )
            {
                throw new InvalidOperationException( "This server has been disposed" );
            }
            if ( sensorInfo == null )
            {
                throw new ArgumentNullException( nameof( sensorInfo ) );
            }
            if ( !( sensorInfo is UdpSensorInfo ) )
            {
                throw new ArgumentException( $"{nameof( sensorInfo )} must be of type UdpSensorInfo", nameof( sensorInfo ) );
            }

            var udpSensorInfo = sensorInfo as UdpSensorInfo;
            Client = new UdpClient( udpSensorInfo.SensorEndpoint );

            Status = UdpSensorServerStatus.Started;

            Task.Run( () => ReadSensorDataAsync( Cts.Token ) )
                .ContinueWith( ( result ) => UnityEngine.Debug.LogWarning( result.Exception ), TaskContinuationOptions.OnlyOnFaulted );
        }

        public void CloseServer()
        {
            if ( Status == UdpSensorServerStatus.Closed )
            {
                throw new InvalidOperationException( "Server is already closed" );
            }

            Dispose( true );

            Status = UdpSensorServerStatus.Closed;
        }

        public ISensorDatagram GetDatagram()
        {
            if ( Status == UdpSensorServerStatus.Closed )
            {
                throw new InvalidOperationException( "Server is already closed" );
            }
            if ( !HasDataAvailable )
            {
                throw new InvalidOperationException( "There is no data available" );
            }

            var datagram = Buffer.Front();

            return datagram.CreateSensorDatagram();
        }

        async Task ReadSensorDataAsync( CancellationToken cancellationToken )
        {
            while ( true )
            {
                UdpReceiveResult data;

                try
                {
                    // TODO: Implement timeout using Task.Delay and Task.WhenAny
                    // TODO: Also a timeout is needed when udp data is coming through but when none of it is considered valid
                    cancellationToken.ThrowIfCancellationRequested();
                    data = await Client.ReceiveAsync();
                    cancellationToken.ThrowIfCancellationRequested();
                }
                catch ( ObjectDisposedException )
                {
                    // Note: object disposed exception will be thrown when server is closed/disposed. Just ignore it and return
                    return;
                }
                catch ( TaskCanceledException )
                {
                    // Note: if we cancel the task, just close the read loop
                    return;
                }
                catch ( Exception ex )
                {
                    Status = UdpSensorServerStatus.Faulted;
                    ServerFaulted?.Invoke();
                    throw ex;
                }

                var datagram = new UdpDatagram( data.Buffer );
                if ( datagram.IsValid )
                {
                    Buffer.PushFront( datagram );
                    if ( Status == UdpSensorServerStatus.Started )
                    {
                        Status = UdpSensorServerStatus.ReceivingData;
                    }
                    // UnityEngine.Debug.Log( $"Got data: {data.Buffer.Length};" );
                }
                else
                {
                    // UnityEngine.Debug.LogWarning( $"Got invalid data: {data.Buffer.Length}" );
                }
            }
        }

        #region IDisposable Support

        bool isDisposed = false; // To detect redundant calls

        protected virtual void Dispose( bool disposing )
        {
            if ( !isDisposed )
            {
                if ( disposing )
                {
                    Cts?.Cancel();
                    Client?.Close();
                    Client?.Dispose();
                }

                Client = null;
                Buffer = null;

                isDisposed = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose( true );
        }

        #endregion
    }
}
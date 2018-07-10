using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Aisens.Udp
{
    public class UdpSensorDetector : ISensorDetector
    {
        /// <summary>
        /// Discover UDP sensors
        /// </summary>
        /// <param name="timeout">Discovery duration, in miliseconds</param>
        /// <returns>A list of sensors discovered</returns>
        public async Task<IList<ISensorInfo>> DiscoverSensorsAsync( int timeout )
        {
            // TODO: Progress, Cancellation
            // TODO: Progress should return information about new sensors in the moment they become available

            ConcurrentBag<ISensorInfo> validSensorInfos = new ConcurrentBag<ISensorInfo>();

            // We know that sensors broadcast transit their data via udp on ports 10000 to 10010
            var startPort = 10000;
            var endPort = 10010;

            var discoveryTasks = new List<Task>();

            for ( int i = startPort; i <= endPort; i++ )
            {
                var sensorInfo = new UdpSensorInfo( new IPEndPoint( IPAddress.Any, i ) );
                discoveryTasks.Add( DiscoverSensorAsync( sensorInfo, validSensorInfos, timeout ) );
            }

            await Task.WhenAll( discoveryTasks ).ConfigureAwait( false );

            return validSensorInfos.ToList();
        }

        async Task DiscoverSensorAsync( ISensorInfo sensorInfo, ConcurrentBag<ISensorInfo> resultsBag, int timeout )
        {
            var server = new UdpSensorServer();
            server.StartServer( sensorInfo );

            var scanTask = ScanForIncomingData( server );
            var timeoutTask = Task.Delay( timeout );

            await Task.WhenAny( scanTask, timeoutTask ).ConfigureAwait( false );  // Whichever task finishes first

            var isValidSensor = scanTask.Status == TaskStatus.RanToCompletion && server.HasDataAvailable;

            if ( isValidSensor )
            {
                resultsBag.Add( sensorInfo );
            }

            // Close the server and change status to Closed, ScanForIncomingData task will run to completion
            server.CloseServer();
        }

        async Task ScanForIncomingData( UdpSensorServer server )
        {
            // This task will run to completion when there is data available or the server is closed
            while ( !server.HasDataAvailable && server.Status != UdpSensorServerStatus.Closed )
            {
                await Task.Delay( 100 );
            }
        }
    }
}
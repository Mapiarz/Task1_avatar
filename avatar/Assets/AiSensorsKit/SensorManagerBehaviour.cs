using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Aisens
{
    public class SensorManagerBehaviour : MonoBehaviour
    {
        SensorManager sensorManager;

        void Awake()
        {
            sensorManager = new SensorManager( SensorProtocol.Udp );  // So far, only UDP is implemented;
        }

        public IEnumerator DiscoverSensors( int timeout, Action<IList<ISensorInfo>> onFinish )
        {
            var discoveryTask = sensorManager.DiscoverSensorsAsync( timeout );

            yield return new WaitUntil( () => discoveryTask.IsCompleted || discoveryTask.IsFaulted );

            if ( discoveryTask.Status == TaskStatus.RanToCompletion )
            {
                onFinish?.Invoke( discoveryTask.Result );
            }
            else
            {
                onFinish?.Invoke( null );
            }
        }

        public IList<ISensorHandle> ConnectToSensors( IList<ISensorInfo> sensorInfos )
        {
            return sensorManager.ConnectToSensors( sensorInfos );
        }

        public ISensorHandle ConnectToSensor( ISensorInfo sensorInfo )
        {
            return sensorManager.ConnectToSensor( sensorInfo );
        }

        public void DisconnectSensor( ISensorHandle handle )
        {
            sensorManager.DisconnectSensor( handle );
        }

        public void DisconnectAllSensors()
        {
            sensorManager.DisconnectAllSensors();
        }

        void OnDestroy()
        {
            sensorManager?.DisconnectAllSensors();
        }
    }
}
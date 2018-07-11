using Aisens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorTester : MonoBehaviour
{
    [SerializeField] SensorManagerBehaviour sensorManager;
    [SerializeField] GameObject cubePrefab;
    [SerializeField] Vector2 cubeInsets;

    Coroutine discoveryCoroutine;

    public void CreateServer()
    {
        if ( discoveryCoroutine != null )
        {
            return;  // Note: Sensor discovery coroutine doesn't support cancellation so we have to return when multiple attempts are made
        }

        DestroyExistingCubes();

        discoveryCoroutine = StartCoroutine( DiscoverSensors() );
    }

    IEnumerator DiscoverSensors()
    {
        yield return sensorManager.DiscoverSensors( 5000, ( result ) =>
        {
            if ( result != null )
            {
                Debug.Log( $"Discovered {result.Count} sensors" );
                SpawnSensorCubes( result );
            }
            else
            {
                Debug.LogWarning( "Sensor discovery failed" );
            }
        } );

        discoveryCoroutine = null;
    }

    void SpawnSensorCubes( IList<ISensorInfo> infos )
    {
        var handles = sensorManager.ConnectToSensors( infos );

        var cubes = SpawnCubes( cubePrefab, handles.Count );

        for ( int i = 0; i < handles.Count; i++ )
        {
            StartCoroutine( RotateCube( handles[i], cubes[i].transform ) );
        }
    }

    List<GameObject> SpawnCubes( GameObject prefab, int count )
    {
        var cubesFieldSize = cubeInsets.y - cubeInsets.x;
        var cubeSpacing = cubesFieldSize / ( count + 1 );

        var result = new List<GameObject>();

        for ( int i = 0; i < count; i++ )
        {
            var xPosition = cubeInsets.x + ( cubeSpacing * ( i + 1 ) );
            var cube = SpawnCube( prefab, xPosition );
            result.Add( cube );
        }

        return result;
    }

    GameObject SpawnCube( GameObject prefab, float xPosition )
    {
        var cube = Instantiate( prefab, transform );

        var localPos = cube.transform.localPosition;
        localPos.x = xPosition;
        cube.transform.localPosition = localPos;

        return cube;
    }

    void DestroyExistingCubes()
    {
        StopAllCoroutines();  // Stop all rotation coroutines

        foreach ( Transform childTransform in transform )
        {
            Destroy( childTransform.gameObject );
        }

        sensorManager.DisconnectAllSensors();
    }

    IEnumerator RotateCube( ISensorHandle handle, Transform transform )
    {
        yield return new WaitUntil( () => handle.CanGetDatagram );

        while ( handle.IsConnectionOpen )
        {
            var rotation = handle.GetDatagram().Rotation;
            // Debug.Log( $"Got rotation: {rotation}; (x: {rotation.eulerAngles.x}; y: {rotation.eulerAngles.y}; z: {rotation.eulerAngles.z}" );

            transform.rotation = rotation;

            yield return null;
        }
    }
}

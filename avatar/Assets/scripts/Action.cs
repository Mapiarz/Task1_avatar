using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
void ReadData()
{
    try
    {
        while (WorkCondition)
        {
            Byte[] receiveBytes = DataGeneration.Gather(int bodyPart);
            string returnData = Encoding.ASCII.GetString(receiveBytes);

            //ESP
            Frame_lenght = 62;
            //string returnData = Encoding.ASCII.GetString(receiveBytes);
            Buffer.BlockCopy(receiveBytes, 4, sensor_udp, 0, Frame_lenght - 4 - 2);

            time = sensor_udp[0]; //czas z pliku

            //dane sensoryczne
            for (int i = 0; i < 3; i++)
            {
                acc[i] = sensor_udp[i + 1];
                mag[i] = sensor_udp[i + 4];
                gyr[i] = sensor_udp[i + 7];
            }

            q.w = sensor_udp[10];
            q.x = -sensor_udp[11];
            q.y = -sensor_udp[13];
            q.z = -sensor_udp[12];
            //dane z udp dla 5 czujników, ale tylko i wyłącznie kawaterniony
            int nr = 3 * 4;
            /*
                            q.w = data[nr + 0];
                            q.x = data[nr + 1];
                            q.y = data[nr + 2];
                            q.z = data[nr + 3];
              */
        }
    }
    catch
    {


    }
}
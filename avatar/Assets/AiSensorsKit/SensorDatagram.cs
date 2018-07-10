using UnityEngine;

namespace Aisens
{
    public class SensorDatagram : ISensorDatagram
    {
        public Quaternion Rotation { get; private set; }

        public SensorDatagram( Quaternion quaternion )
        {
            Rotation = quaternion;
        }
    }
}
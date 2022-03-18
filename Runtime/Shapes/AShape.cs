using UnityEngine;
using Utilities;

namespace ShapeFormulas
{
    public abstract class AShape<Coordinates>
    {
        public abstract Vector3 GetPoint(Coordinates _normalizedCoordinates);
        public virtual void GetPoint(
            Coordinates _normalizedCoordinates, out Vector3 _position, out Vector3 _forward
            ) {
            _position = GetPoint(_normalizedCoordinates);
            _forward = Quaternion.Euler(0, -90, 0) * _position;
        }


        public abstract Range<Coordinates> CoordinateRange
        {
            get;
        }

        public abstract float Size
        {
            get;
        }
    }
}
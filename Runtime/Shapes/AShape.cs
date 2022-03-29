using UnityEngine;
using Utilities;

namespace ShapeFormulas
{
    public abstract class AShape<Coordinates>
    {
        public abstract Vector3 GetPoint(Coordinates _normalizedCoordinates);

        public abstract void GetPoint(
            Coordinates _normalizedCoordinates, out Vector3 _position, out Vector3 _forward
        );


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
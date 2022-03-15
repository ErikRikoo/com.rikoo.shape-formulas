using UnityEngine;

namespace Utilities
{
    public static class Formulas
    {
        public static class Circle
        {
            public static Vector3 GetPoint(float _normalizedCoordinates, float _radius = 1f)
            {
                float rightAngle = _normalizedCoordinates * Mathf.PI * 2;
                return new Vector3(
                    Mathf.Cos(rightAngle) * _radius, 0f,
                    Mathf.Sin(rightAngle) * _radius
                );
            }
        }
    }
}
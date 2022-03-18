using System;
using UnityEngine;
using Utilities;

namespace ShapeFormulas.Implementation.OneDimension
{
    [Serializable]
    public class CircleShape : AShape1D
    {
        [Min(0)]
        [SerializeField] private float m_Radius;
        
        public override Vector3 GetPoint(float _normalizedCoordinates)
        {
            return Formulas.Circle.GetPoint(_normalizedCoordinates, m_Radius);
        }

        public override Range<float> CoordinateRange => new Range<float>
        {
            Min = 0,
            Max = 360f,
        };

        public override float Size => 2 * Mathf.PI * m_Radius;
    }
}
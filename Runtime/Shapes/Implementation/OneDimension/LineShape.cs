using System;
using UnityEngine;
using Utilities;

namespace ShapeFormulas.Implementation.OneDimension
{
    [Serializable]
    public class LineShape : AShape1D
    {
        [SerializeField] private Vector3 m_Direction = Vector3.forward;
        [SerializeField] private float m_Length = 1;
        
        
        public override Vector3 GetPoint(float _normalizedCoordinates)
        {
            return Vector3.LerpUnclamped(Vector3.zero, m_Direction * m_Length, _normalizedCoordinates);
        }

        public override void GetPoint(float _normalizedCoordinates, out Vector3 _position, out Vector3 _forward)
        {
            _position = GetPoint(_normalizedCoordinates);
            _forward = m_Direction;
        }

        public override Range<float> CoordinateRange => new Range<float>
        {
            Min = 0,
            Max = m_Length
        };

        public override float Size => m_Length;
    }
}
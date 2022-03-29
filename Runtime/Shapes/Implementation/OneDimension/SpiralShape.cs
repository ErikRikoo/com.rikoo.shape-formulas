using System;
using UnityEngine;
using Utilities;

namespace ShapeFormulas.Implementation.OneDimension
{
    [Serializable]
    public class SpiralShape : AShape1D
    {
        [SerializeField] private float m_Radius;
        [SerializeField] private float m_TurnHeight;
        [Min(0.01f)]
        [SerializeField] private float m_TurnCount = 1;

        public override Vector3 GetPoint(float _normalizedCoordinates)
        {
            return new Vector3(m_Radius * Mathf.Cos(_normalizedCoordinates * 2 * Mathf.PI * m_TurnCount),
                m_TurnHeight * _normalizedCoordinates * m_TurnCount,
                m_Radius * Mathf.Sin(_normalizedCoordinates * 2 * Mathf.PI * m_TurnCount));
        }

        public override void GetPoint(float _normalizedCoordinates, out Vector3 _position, out Vector3 _forward)
        {
            _position = GetPoint(_normalizedCoordinates);
            _forward = new Vector3(
                -2 * Mathf.PI * m_TurnCount * _position.z,
                m_TurnHeight * m_TurnCount,
                2 * Mathf.PI * m_TurnCount * _position.x
                );
        }

        public override Range<float> CoordinateRange => new Range<float>()
        {
            Min = 0,
            Max = 2 * Mathf.PI * m_TurnCount
        };

        public override float Size => new Vector2(m_Radius, m_TurnHeight).magnitude * m_TurnCount;
    }
}
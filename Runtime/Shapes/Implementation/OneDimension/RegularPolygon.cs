using System;
using UnityEngine;
using Utilities;

namespace ShapeFormulas.Implementation.OneDimension
{
    public enum Orientation
    {
        FlatSide,
        PointySide
    }
    
    [Serializable]
    public class RegularPolygon : AShape1D
    {
        [HideInInspector]
        [SerializeField] private Vector3[] m_Vertices;

        [Min(0)]
        [SerializeField] private float m_Radius = 1;
        
        [Min(3)]
        [SerializeField] private int m_VertexCount = 3;

        [SerializeField] private Orientation m_Orientation;
        
        
        
        public void RecomputeVertices()
        {
            m_Vertices = new Vector3[m_VertexCount];
            float angle = 1f / m_VertexCount;
            float pointAngle = m_Orientation switch
            {
                Orientation.FlatSide => angle * 0.5f,
                Orientation.PointySide => 0
            };
            for (int i = 0; i < m_VertexCount; ++i)
            {
                m_Vertices[i] = Formulas.Circle.GetPoint(pointAngle);
                pointAngle += angle;
            }
        }
        
        public override Vector3 GetPoint(float _normalizedCoordinates)
        {
            float pointCoordinate = _normalizedCoordinates * m_VertexCount;
            int floored = Mathf.FloorToInt(pointCoordinate) % m_VertexCount;
            Vector3 startPoint = m_Vertices[floored];
            Vector3 endPoint = m_Vertices[(floored + 1) % m_VertexCount];
            float interp = pointCoordinate - floored;

            return Vector3.LerpUnclamped(startPoint, endPoint, interp) * m_Radius;
        }

        public override Range<float> CoordinateRange => new Range<float>
        {
            Min = 0,
            Max = 360f,
        };

        public override float Size => m_Radius * Mathf.Sqrt(2 * (1 - Mathf.Cos(2 * Mathf.PI / m_VertexCount)));
        public override void UpdateState()
        {
            RecomputeVertices();
        }

        public int VertexCount => m_VertexCount;
    }
}
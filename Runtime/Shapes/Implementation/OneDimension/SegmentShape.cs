using System;
using DotLiquid.Tags;
using EditorUtilities.Editor.Attributes.AbstractReference;
using UnityEngine;
using Utilities;

namespace ShapeFormulas.Implementation.OneDimension
{
    [AbstractNaming("Segment")]
    [Serializable]
    public class SegmentShape : AShape1D
    {
        [SerializeField] private Vector3 m_Start;
        [SerializeField] private Vector3 m_End;
        
        
        public override Vector3 GetPoint(float _normalizedCoordinates)
        {
            return Vector3.LerpUnclamped(m_Start, m_End, _normalizedCoordinates);
        }

        public override void GetPoint(float _normalizedCoordinates, out Vector3 _position, out Vector3 _forward)
        {
            _position = GetPoint(_normalizedCoordinates);
            _forward = Forward;
        }

        public override Range<float> CoordinateRange => new Range<float>
        {
            Min = 0,
            Max = Size
        };

        public override float Size => Vector3.Distance(m_Start, m_End);

        public Vector3 Forward => (m_End - m_Start).normalized;

#if UNITY_EDITOR
        [HideInInspector]
        [SerializeField] private AdvancedSettings m_AdvancedSettings;
        
        [Serializable]
        public class AdvancedSettings
        {
            [SerializeField] private bool m_StartSelected;
            [SerializeField] private bool m_EndSelected;
            [SerializeField] private int m_CurrentlyUsedTool = -1;
            [SerializeField] private float m_FloatBuffer;
            [SerializeField] private int m_IntBuffer1 = 0;
            [SerializeField] private int m_IntBuffer2 = 0;
        }
#endif        
    }


}
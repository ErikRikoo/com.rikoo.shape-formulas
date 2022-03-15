using System;
using ShapeFormulas.Implementation.OneDimension;
using UnityEditor;
using UnityEngine;

namespace ShapeFormulas.ShapesDrawer.Implementation
{
    public class CircleShapeDrawer : AShapeDrawer
    {
        private static Vector3[] _direction = {
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1)
        };
        
        public override Type HandledType => typeof(CircleShape);
        
        public override void OnSceneGUI(Transform _worldTransform, SerializedProperty _property)
        {
            Handles.color = Color.red;
            var radiusProperty = _property.FindPropertyRelative("m_Radius");
            Handles.DrawWireDisc(_worldTransform.position, _worldTransform.up, radiusProperty.floatValue);
            
            foreach (var dir in _direction)
            {
                var rotatedDir = _worldTransform.rotation * dir;
                Vector3 _worldPoint = _worldTransform.position + rotatedDir * radiusProperty.floatValue;
                EditorGUI.BeginChangeCheck();
                Vector3 _newPosition = Handles.Slider(_worldPoint, rotatedDir, 0.2f, Handles.CubeHandleCap, 0.01f);
                if (EditorGUI.EndChangeCheck())
                {
                    float change = Vector3.Dot(rotatedDir, _newPosition - _worldTransform.position);
                    radiusProperty.floatValue = Mathf.Max(change, 0);
                }
            }
        }
    }
}
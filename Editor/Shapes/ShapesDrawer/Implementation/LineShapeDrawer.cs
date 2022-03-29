using System;
using System.IO;
using ShapeFormulas.Implementation.OneDimension;
using UnityEditor;
using UnityEngine;

namespace ShapeFormulas.ShapesDrawer.Implementation
{
    public class LineShapeDrawer : AShapeDrawer
    {
        public override Type HandledType => typeof(LineShape);

        public override void OnSceneGUI(Transform _worldTransform, SerializedProperty _property)
        {
            SerializedProperty directionProperty = _property.FindPropertyRelative("m_Direction");
            SerializedProperty lengthProperty = _property.FindPropertyRelative("m_Length");

            var direction = directionProperty.vector3Value;
            Vector3 _endWorldPosition =
                _worldTransform.TransformPoint(direction * lengthProperty.floatValue);
            Handles.DrawLine(_worldTransform.position, _endWorldPosition);
            Handles.SphereHandleCap(0, _worldTransform.position, Quaternion.identity, 0.2f, EventType.Repaint);
            
            EditorGUI.BeginChangeCheck();
            Vector3 newPosition = Handles.Slider(_endWorldPosition, direction, 0.5f, Handles.CubeHandleCap, 0.01f);
            if (EditorGUI.EndChangeCheck())
            {
                var worldNewPoint = _worldTransform.InverseTransformPoint(newPosition);
                lengthProperty.floatValue = Vector3.Dot(worldNewPoint, direction.normalized);
                _property.serializedObject.ApplyModifiedProperties();
            }
            
            EditorGUI.BeginChangeCheck();
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion newRotation = Handles.RotationHandle(rotation, _worldTransform.position);
            if (EditorGUI.EndChangeCheck())
            {
                directionProperty.vector3Value = newRotation * Vector3.forward;
            }
        }
    }
}
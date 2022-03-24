using System;
using ShapeFormulas.Implementation.OneDimension;
using UnityEditor;
using UnityEngine;

namespace ShapeFormulas.ShapesDrawer.Implementation
{
    
    public class SegmentShapeDrawer : AShapeDrawer
    {
        public override Type HandledType => typeof(SegmentShape);

        public override void OnSceneGUI(Transform _worldTransform, SerializedProperty _property)
        {
            DisplayPositionAsHandle(_worldTransform, _property.FindPropertyRelative("m_Start"));
            DisplayPositionAsHandle(_worldTransform, _property.FindPropertyRelative("m_End"));
        }

        private void DisplayPositionAsHandle(Transform _worldTransform, SerializedProperty _property)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 _worldPosition = _worldTransform.TransformPoint(_property.vector3Value);
            Vector3 _newPosition = Handles.PositionHandle(_worldPosition, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                _property.vector3Value = _worldTransform.InverseTransformPoint(_newPosition);
                _property.serializedObject.ApplyModifiedProperties();
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}
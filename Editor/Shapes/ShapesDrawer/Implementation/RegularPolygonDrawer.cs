using System;
using System.Linq;
using EditorUtilities.Editor.Extensions;
using ShapeFormulas.Implementation.OneDimension;
using UnityEditor;
using UnityEngine;

namespace ShapeFormulas.ShapesDrawer.Implementation
{
    public class RegularPolygonDrawer : AShapeDrawer
    {
        public override Type HandledType => typeof(RegularPolygon);

        public override void OnSceneGUI(Transform _worldTransform, SerializedProperty _property)
        {
            var pointsProp = _property.FindPropertyRelative("m_Vertices");
            var radiusProp = _property.FindPropertyRelative("m_Radius");

            var points = pointsProp.GetArrayElement().Select(
                prop => _worldTransform.TransformPoint(prop.vector3Value * radiusProp.floatValue)).ToList();
            points.Add(points[0]);
            
            Handles.color = Color.red;
            Handles.DrawPolyLine(points.ToArray());

            for(int i = 0; i < points.Count - 1; ++i)
            {
                Vector3 objectHandlePosition = points[i];
                Vector3 direction = (objectHandlePosition - _worldTransform.position).normalized;
                EditorGUI.BeginChangeCheck();
                Vector3 _newPosition = Handles.Slider(objectHandlePosition, direction, 0.2f, Handles.CubeHandleCap, 0.01f);
                if (EditorGUI.EndChangeCheck())
                {
                    
                    float change = Vector3.Dot(direction, _newPosition - _worldTransform.position);
                    radiusProp.floatValue = Mathf.Max(change, 0);
                }
            }
        }
    }
}
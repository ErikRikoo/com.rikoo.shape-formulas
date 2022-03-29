using System;
using EditorUtilities.Editor.Attributes.AbstractReference;
using EditorUtilities.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace ShapeFormulas.ShapesDrawer.Implementation.EditorTools.Implementation
{
    [AbstractNaming("Mouse Raycast")]
    public class MouseRaycast : ASegmentTool
    {

        public override void OnSceneGUI(Transform _worldTransform, SerializedProperty _property)
        {
            if (Event.current.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(0); // Consume the event
            }
            
            if (Event.current.rawType == EventType.MouseDown && Event.current.button == 0)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    var properties = Props(_property);

                    var worldPoint = hit.point + hit.normal * properties.FloatBuffer.floatValue;

                    var hitPoint = _worldTransform.InverseTransformPoint(worldPoint);
                    if (properties.StartSelected.boolValue)
                    {
                        properties.StartProp.vector3Value = hitPoint;
                    }
                    
                    if (properties.EndSelected.boolValue)
                    {
                        properties.EndProp.vector3Value = hitPoint;
                    }
                }
            }
        }

        private float Padding => 4;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var properties = Props(property);
            bool shouldWarn = ShouldWarn(properties);

            return EditorGUIUtility.singleLineHeight * (
                (shouldWarn?  2: 0)
                + 1
                ) + (shouldWarn?Padding:0);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var properties = Props(property);
            Rect floatFieldPosition = new Rect(position);
            floatFieldPosition.height = EditorGUIUtility.singleLineHeight;
            properties.FloatBuffer.floatValue = 
                EditorGUI.FloatField(floatFieldPosition, "Distance To Surface", properties.FloatBuffer.floatValue);
            
            if (ShouldWarn(properties))
            {
                position.AddLine();
                position.AddLine(Padding);
                EditorGUI.HelpBox(position, "Both points are selected and thus will be collapsed", MessageType.Warning);
            }
        }

        public bool ShouldWarn(SegmentShapeDrawer.Properties _props)
        {
            return _props.StartSelected.boolValue && _props.EndSelected.boolValue;
        }

        public SegmentShapeDrawer.Properties Props(SerializedProperty _root)
        {
            var ret = new SegmentShapeDrawer.Properties();
            ret.Initialize(_root);
            return ret;
        }
    }
}
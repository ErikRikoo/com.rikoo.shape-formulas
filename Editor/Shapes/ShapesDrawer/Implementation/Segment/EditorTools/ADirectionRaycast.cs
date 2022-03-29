using EditorUtilities.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace ShapeFormulas.ShapesDrawer.Implementation.EditorTools.Implementation
{
    public abstract class ADirectionRaycast : ASegmentTool
    {

        protected abstract Vector3 StartRaycastDirection(SegmentShapeDrawer.Properties _properties);

        protected abstract Vector3 EndRaycastDirection(SegmentShapeDrawer.Properties _properties);
        
        private float OkButtonSize => EditorGUIUtility.singleLineHeight * 2;
        private float Padding => 4;

        private Transform WorldTransform(SerializedProperty _property)
        {
            return (_property.serializedObject.targetObject as MonoBehaviour).transform;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var properties = new SegmentShapeDrawer.Properties().Initialize(property);


            Rect settingsPosition = new Rect(position);
            settingsPosition.ShrinkRight(OkButtonSize + Padding);
            
            Rect floatFieldPosition = new Rect(settingsPosition);
            floatFieldPosition.height = EditorGUIUtility.singleLineHeight;
            properties.FloatBuffer.floatValue = 
                EditorGUI.FloatField(floatFieldPosition, "Distance To Surface", properties.FloatBuffer.floatValue);
            settingsPosition.AddLine();
            DrawSettings(settingsPosition, properties);
            
            Rect buttonPosition = new Rect(position);
            buttonPosition.xMin = buttonPosition.xMax - OkButtonSize;
            if (GUI.Button(buttonPosition, "OK"))
            {
                Transform transform = WorldTransform(property);
                Ray startRay = new Ray(transform.TransformPoint(properties.StartProp.vector3Value), StartRaycastDirection(properties));
                if (properties.StartSelected.boolValue && Physics.Raycast(startRay, out var hit))
                {
                    var worldPoint = hit.point + hit.normal * properties.FloatBuffer.floatValue;

                    var hitPoint = WorldTransform(property).InverseTransformPoint(worldPoint);
                    properties.StartProp.vector3Value = hitPoint;
                }
                    
                Ray endRay = new Ray(transform.TransformPoint(properties.EndProp.vector3Value), EndRaycastDirection(properties));
                if (properties.EndSelected.boolValue && Physics.Raycast(endRay, out hit))
                {
                    var worldPoint = hit.point + hit.normal * properties.FloatBuffer.floatValue;
                    var hitPoint = WorldTransform(property).InverseTransformPoint(worldPoint);
                    properties.EndProp.vector3Value = hitPoint;
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight + GetSettingsHeight(property);
        }

        protected abstract void DrawSettings(Rect _position, SegmentShapeDrawer.Properties _properties);
        protected abstract float GetSettingsHeight(SerializedProperty _property);

        protected static Vector3[] Directions = {
            Vector3.forward,
            Vector3.right,
            Vector3.up,
            Vector3.back,
            Vector3.left,
            Vector3.down,
        };

        private GUIContent[] DirectionsNames =
        {
            new GUIContent("Forward","(0, 0, 1)"), 
            new GUIContent("Right", "(1, 0, 0)"), 
            new GUIContent("Up", "(0,1,0)"), 
            new GUIContent("Back", "(0,0,-1)"), 
            new GUIContent("Left", "(-1, 0, 0)"), 
            new GUIContent("Down", "(0, -1, 0)")
        };
        
        protected int DrawDirectionPicker(Rect _position, int _current, string _label)
        {
            return EditorGUI.Popup(_position, new GUIContent(_label), _current, DirectionsNames);
        }
        
    }
}
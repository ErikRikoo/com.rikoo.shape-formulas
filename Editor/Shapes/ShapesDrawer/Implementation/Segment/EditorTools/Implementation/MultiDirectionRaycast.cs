using EditorUtilities.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace ShapeFormulas.ShapesDrawer.Implementation.EditorTools.Implementation
{
    public class MultiDirectionRaycast : ADirectionRaycast
    {
        protected override Vector3 StartRaycastDirection(SegmentShapeDrawer.Properties _properties)
        {
            int index = _properties.IntBuffer1.intValue;
            return Directions[index];
        }

        protected override Vector3 EndRaycastDirection(SegmentShapeDrawer.Properties _properties)
        {
            int index = _properties.IntBuffer2.intValue;
            return Directions[index];
        }

        protected override void DrawSettings(Rect _position, SegmentShapeDrawer.Properties _properties)
        {
            _position.AddLine(BeginningPadding);
            _position.height = EditorGUIUtility.singleLineHeight;
            if (_properties.StartSelected.boolValue)
            {
                _properties.IntBuffer1.intValue = DrawDirectionPicker(_position, _properties.IntBuffer1.intValue, "Start Direction");
                _position.AddLine(EditorGUIUtility.singleLineHeight + InnerPadding);
                _position.height = EditorGUIUtility.singleLineHeight;
            }

            if (_properties.EndSelected.boolValue)
            {
                _properties.IntBuffer2.intValue = DrawDirectionPicker(_position, _properties.IntBuffer2.intValue, "End Direction");
            }
        }

        protected override float GetSettingsHeight(SerializedProperty _property)
        {
            var properties = new SegmentShapeDrawer.Properties().Initialize(_property);
            var selectedCount = (properties.StartSelected.boolValue ? 1 : 0) +
                                (properties.EndSelected.boolValue ? 1 : 0);
            return EditorGUIUtility.singleLineHeight * Mathf.Max(selectedCount, 1) + BeginningPadding +
                   (selectedCount == 2 ? InnerPadding : 0);
        }
        
        public float BeginningPadding => 4;
        public float InnerPadding => 2;
        
    }
}
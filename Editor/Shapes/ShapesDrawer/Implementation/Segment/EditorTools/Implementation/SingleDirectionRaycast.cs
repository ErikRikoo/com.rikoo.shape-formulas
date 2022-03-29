using EditorUtilities.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace ShapeFormulas.ShapesDrawer.Implementation.EditorTools.Implementation
{
    public class SingleDirectionRaycast : ADirectionRaycast
    {
        protected override Vector3 StartRaycastDirection(SegmentShapeDrawer.Properties _properties)
        {
            int index = _properties.IntBuffer1.intValue;
            return Directions[index];
        }

        protected override Vector3 EndRaycastDirection(SegmentShapeDrawer.Properties _properties)
        {
            return StartRaycastDirection(_properties);
        }

        protected override void DrawSettings(Rect _position, SegmentShapeDrawer.Properties _properties)
        {
            _position.AddLine(BeginningPadding);
            _properties.IntBuffer1.intValue = DrawDirectionPicker(_position, _properties.IntBuffer1.intValue, "Direction");
        }

        protected override float GetSettingsHeight(SerializedProperty _property)
            => EditorGUIUtility.singleLineHeight + BeginningPadding;

        public float BeginningPadding => 4;
    }
}
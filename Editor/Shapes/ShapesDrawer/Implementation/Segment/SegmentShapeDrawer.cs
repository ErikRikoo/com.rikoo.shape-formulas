using System;
using System.Xml.Serialization;
using EditorUtilities.Editor.Extensions;
using EditorUtilities.Editor.Utilities;
using ShapeFormulas.Implementation.OneDimension;
using ShapeFormulas.ShapesDrawer.Implementation.EditorTools;
using UnityEditor;
using UnityEngine;
using Utilities.DrawerFactory;

namespace ShapeFormulas.ShapesDrawer.Implementation
{
    
    public class SegmentShapeDrawer : AShapeDrawer
    {
        public override Type HandledType => typeof(SegmentShape);

        public override void OnSceneGUI(Transform _worldTransform, SerializedProperty _property)
        {
            Properties properties = new Properties();
            properties.Initialize(_property);

            Vector3 start = _worldTransform.TransformPoint(properties.StartProp.vector3Value);
            Vector3 end = _worldTransform.TransformPoint(properties.EndProp.vector3Value);
            Handles.DrawLine(start, end);
            Handles.SphereHandleCap(0, start, Quaternion.identity, 0.4f, EventType.Repaint);
            Handles.SphereHandleCap(0, end, Quaternion.identity, 0.4f, EventType.Repaint);
            
            if (properties.StartSelected.boolValue)
            {
                DisplayPositionAsHandle(_worldTransform, properties.StartProp);
            }
            
            if (properties.EndSelected.boolValue)
            {
                DisplayPositionAsHandle(_worldTransform, properties.EndProp);
            }
            
            properties.GetCurrentTool(this)?.OnSceneGUI(_worldTransform, _property);
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

        private MainDrawerFactory<ASegmentTool> m_Tools;

        private MainDrawerFactory<ASegmentTool> Tools =>
            m_Tools ??= new MainDrawerFactory<ASegmentTool>();

        private static readonly float s_PointTogglePadding = 10;
        private static readonly int s_ToolsPerRow = 3;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
            Rect originalPosition = new Rect(position);
            Properties properties = new Properties();
            properties.Initialize(property);
            
            DrawPoints(originalPosition, properties);
            
            Rect rect = new Rect(position);
            rect.yMin = rect.y + rect.height - GetExtraDrawerSize(property);
            DrawTools(rect, properties);
            
            rect.AddLine(EditorGUIUtility.singleLineHeight * RowCount + PaddingAfterToolSelection);
            properties.GetCurrentTool(this)?.OnGUI(rect, property, label);
        }

        private void DrawTools(Rect _position, Properties _properties)
        {
            Rect selection = new Rect(_position);
            selection.ShrinkRight(EditorGUIUtility.singleLineHeight);
            selection.height = EditorGUIUtility.singleLineHeight * RowCount;
            
            _properties.CurrentToolIndex = GUI.SelectionGrid(selection, _properties.CurrentToolIndex, Tools.Names, s_ToolsPerRow);
            Rect buttonPos = new Rect(_position);
            buttonPos.xMin = buttonPos.xMax - EditorGUIUtility.singleLineHeight;
            buttonPos.height = EditorGUIUtility.singleLineHeight;
            if (GUI.Button(buttonPos, "x"))
            {
                _properties.CurrentToolIndex = -1;
            }
        }

        private void DrawPoints(Rect _position, Properties _properties)
        {
            float toggleSize = GUI.skin.toggle.CalcSize(GUIContent.none).x;
            Rect lineProp = new Rect(_position);
            lineProp.height = EditorGUIUtility.singleLineHeight;
            Rect toggleRect = new Rect(lineProp);
            toggleRect.xMin = toggleRect.xMax - toggleSize;
            toggleRect.IgnorePaddingLeft();
            lineProp.ShrinkRight(toggleSize + s_PointTogglePadding);
            
            EditorGUI.PropertyField(lineProp, _properties.StartProp);
            _properties.StartSelected.boolValue = EditorGUI.Toggle(toggleRect, _properties.StartSelected.boolValue);
            lineProp.NextLine();
            toggleRect.NextLine();
            
            EditorGUI.PropertyField(lineProp, _properties.EndProp);
            _properties.EndSelected.boolValue = EditorGUI.Toggle(toggleRect, _properties.EndSelected.boolValue);
        }

        private float PaddingAfterToolSelection => 6;
        
        private float GetExtraDrawerSize(SerializedProperty _root)
        {
            var props = new Properties().Initialize(_root);
            var tool = props.GetCurrentTool(this);
            var toolHeight = tool?.GetPropertyHeight(_root, GUIContent.none) ?? 0;
            
            return EditorGUIUtility.singleLineHeight * RowCount + toolHeight + PaddingAfterToolSelection;
        }

        private int RowCount => Mathf.CeilToInt(Tools.Count * 1f/ s_ToolsPerRow) ;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) +
                   GetExtraDrawerSize(property);
        }

        public class Properties
        {
            public SerializedProperty AdvancedSettings;
            public SerializedProperty StartProp;
            public SerializedProperty EndProp;
            public SerializedProperty StartSelected;
            public SerializedProperty EndSelected;
            public SerializedProperty CurrentlyUsedTool;
            public SerializedProperty FloatBuffer;
            public SerializedProperty IntBuffer1;
            public SerializedProperty IntBuffer2;

            public Properties Initialize(SerializedProperty _main)
            {
                AdvancedSettings = GetAdvancedSettingsProperty(_main);
                StartProp = GetStart(_main);
                EndProp = GetEnd(_main);
                StartSelected = AdvancedSettings.FindPropertyRelative("m_StartSelected");
                EndSelected = AdvancedSettings.FindPropertyRelative("m_EndSelected");
                CurrentlyUsedTool = AdvancedSettings.FindPropertyRelative("m_CurrentlyUsedTool");
                FloatBuffer = AdvancedSettings.FindPropertyRelative("m_FloatBuffer");
                IntBuffer1 = AdvancedSettings.FindPropertyRelative("m_IntBuffer1");
                IntBuffer2 = AdvancedSettings.FindPropertyRelative("m_IntBuffer2");
                
                return this;
            }
            
            public static SerializedProperty GetStart(SerializedProperty _property)
                => _property.FindPropertyRelative("m_Start");

        
            public static SerializedProperty GetEnd(SerializedProperty _property)
                => _property.FindPropertyRelative("m_End");
            
            public static SerializedProperty GetAdvancedSettingsProperty(SerializedProperty _property)
                => _property.FindPropertyRelative("m_AdvancedSettings");

            public int CurrentToolIndex
            {
                get => CurrentlyUsedTool.intValue;
                set => CurrentlyUsedTool.intValue = value;
            }



            public ASegmentTool GetCurrentTool(SegmentShapeDrawer _mainDrawer)
            {
                return _mainDrawer.Tools.GetInstance(CurrentToolIndex);
            }
        }
    }
}
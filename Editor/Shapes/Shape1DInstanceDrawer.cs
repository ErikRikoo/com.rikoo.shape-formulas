using System;
using EditorUtilities.Editor.Extensions;
using ShapeFormulas;
using ShapeFormulas.ShapesDrawer;
using UnityEditor;
using UnityEngine;
using Utilities.DrawerFactory;

namespace Shapes
{
    [CustomEditor(typeof(Shape1DInstance))]
    public class Shape1DInstanceDrawer : Editor
    {
        public Shape1DInstance Target => target as Shape1DInstance;

        private ShapeDrawerHolder m_ShapeDrawerHolder = new ShapeDrawerHolder();
        
        private void OnEnable()
        {
            m_ShapeDrawerHolder.OnEnable(serializedObject, "m_Shape", Target);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        private void OnSceneGUI()
        {
            m_ShapeDrawerHolder.OnSceneGUI(Target.transform);
        }
    }
}
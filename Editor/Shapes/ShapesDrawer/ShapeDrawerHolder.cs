using System;
using UnityEditor;
using UnityEngine;
using Utilities.DrawerFactory;

namespace ShapeFormulas.ShapesDrawer
{
    public class ShapeDrawerHolder
    {
        private ObjectDrawerFactory<AShapeDrawer> m_Drawer = new ObjectDrawerFactory<AShapeDrawer>();
        private SerializedProperty m_ShapeProperty;
        private SerializedObject m_SerializedObject;
        private IShape1DHolder m_ShapeHolder;

        public void OnEnable(SerializedObject _serializedObject, string _propertyName, IShape1DHolder _holder)
        {
            m_SerializedObject = _serializedObject;
            m_ShapeProperty = _serializedObject.FindProperty(_propertyName);
            m_ShapeHolder = _holder;
        }
        
        public void OnSceneGUI(Transform _worldTransform)
        {
            if (m_ShapeHolder.Shape == null)
            {
                return;
            }
            
            Type shapeType = m_ShapeHolder.Shape.GetType();
            if (m_Drawer.TryGetDrawer(shapeType, out AShapeDrawer _drawer))
            {
                _drawer.OnSceneGUI(_worldTransform, m_ShapeProperty);
                m_SerializedObject.ApplyModifiedProperties();
            }
        }
    }
}
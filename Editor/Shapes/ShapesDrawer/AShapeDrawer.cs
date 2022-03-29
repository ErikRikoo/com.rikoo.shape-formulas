using System;
using UnityEditor;
using UnityEngine;
using Utilities.DrawerFactory;

namespace ShapeFormulas.ShapesDrawer
{
    public abstract class AShapeDrawer : AObjectDrawer
    {
        public virtual void OnSceneGUI(Transform _worldTransform, SerializedProperty _property) {}
    }
}
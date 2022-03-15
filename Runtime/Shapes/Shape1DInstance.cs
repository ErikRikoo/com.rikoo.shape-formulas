using EditorUtilities.Editor.Attributes.AbstractReference;
using UnityEngine;

namespace ShapeFormulas
{
    public class Shape1DInstance : MonoBehaviour, IShape1DHolder
    {
        [AbstractReference]
        [SerializeReference] private AShape1D m_Shape;

        public AShape1D Shape => m_Shape;
    }
}
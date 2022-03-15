using UnityEngine;

namespace ShapeFormulas
{
    public abstract class AShape1D : AShape<float>
    {
        public AShape1D()
        {
            UpdateState();
        }
        
        public virtual void UpdateState() {}
    }
}
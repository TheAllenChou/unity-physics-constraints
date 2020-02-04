/******************************************************************************/
/*
  Project - Physics Constraints
            https://github.com/TheAllenChou/unity-physics-constraints
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

using UnityEngine;

namespace PhysicsConstraints
{
  public class SphereCollider : PhysicsCollider
  {
    [Min(0.0f)]
    public float radius = 0.5f;

    public SphereCollider()
      : base(ShapeType.Sphere)
    { }
  }
}


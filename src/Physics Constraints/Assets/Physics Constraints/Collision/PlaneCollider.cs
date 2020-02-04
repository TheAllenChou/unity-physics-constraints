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
  public class PlaneCollider : PhysicsCollider
  {
    public Vector3 LocalNormal = Vector3.up;
    public Vector2 Normal { get { return transform.TransformVector(LocalNormal); } }

    public PlaneCollider()
      : base(ShapeType.Plane)
    { }
  }
}


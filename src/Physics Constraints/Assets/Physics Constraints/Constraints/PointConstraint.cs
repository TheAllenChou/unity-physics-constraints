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
  [RequireComponent(typeof(PhysicsBody))]
  public class PointConstraint : PointConstraintBase
  {
    public Transform Target;
    public Vector3 LocalAnchor;

    protected override Vector3 GetTarget()
    {
      return 
        (Target != null)
          ? Target.position 
          : transform.position;
    }

    protected override Vector3 GetLocalAnchor()
    {
      return LocalAnchor;
    }
  }
}

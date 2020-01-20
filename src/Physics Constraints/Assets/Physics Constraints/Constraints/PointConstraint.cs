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
  [RequireComponent(typeof(Body))]
  public class PointConstraint : PointConstraintBase
  {
    public Transform Anchor;

    protected override Vector3 GetAnchor()
    {
      return 
        (Anchor != null)
          ? Anchor.position 
          : transform.position;
    }
  }
}

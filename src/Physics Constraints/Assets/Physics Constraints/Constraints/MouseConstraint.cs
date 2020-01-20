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
  public class MouseConstraint : PointConstraintBase
  {
    public Transform Anchor;

    protected override Vector3 GetAnchor()
    {
      // TODO
      return transform.position;
    }
  }
}

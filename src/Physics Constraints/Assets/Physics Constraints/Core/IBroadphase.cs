/******************************************************************************/
/*
  Project - Physics Constraints
            https://github.com/TheAllenChou/unity-physics-constraints
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

using System.Collections.Generic;

namespace PhysicsConstraints
{
  public struct ColliderPair
  {
    public PhysicsCollider ColliderA;
    public PhysicsCollider ColliderB;

    public ColliderPair(PhysicsCollider colliderA, PhysicsCollider colliderB)
    {
      ColliderA = colliderA;
      ColliderB = colliderB;
    }
  }

  public interface IBroadphase
  {
    ICollection<ColliderPair> GenerateColliderPairs(ICollection<PhysicsCollider> colliders);
  }
}

/******************************************************************************/
/*
  Project - Physics Constraints
            https://github.com/TheAllenChou/unity-physics-constraints
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

using System;
using System.Collections.Generic;

namespace PhysicsConstraints
{
  public class NSquared : IBroadphase
  {
    private ICollection<ColliderPair> m_colliderPairs;

    // y u no clone IEnumerable?? >:(
    private ICollection<PhysicsCollider> m_outerLoopBuffer;

    public NSquared()
    {
      m_colliderPairs = new List<ColliderPair>();
      m_outerLoopBuffer = new List<PhysicsCollider>();
    }

    public ICollection<ColliderPair> GenerateColliderPairs(ICollection<PhysicsCollider> colliders)
    {
      m_colliderPairs.Clear();
      var itColliderA = colliders.GetEnumerator();
      while (itColliderA.MoveNext())
      {
        var colliderA = itColliderA.Current;
        foreach (var colliderB in m_outerLoopBuffer)
        {
          m_colliderPairs.Add(new ColliderPair(colliderA, colliderB));
        }
        m_outerLoopBuffer.Add(colliderA);
      }
      m_outerLoopBuffer.Clear();

      return m_colliderPairs;
    }
  }
}

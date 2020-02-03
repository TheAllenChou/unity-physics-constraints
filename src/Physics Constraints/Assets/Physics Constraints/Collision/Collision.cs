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

using CjLib;

namespace PhysicsConstraints
{
  public class Collision
  {
    public static bool SphereVsSphere
    (
      PhysicsBody bodyA, 
      Vector3 centerA, 
      float radiusA, 
      PhysicsBody bodyB, 
      Vector3 centerB, 
      float radiusB, 
      ref Contact contactOut
    )
    {
      Vector3 vec = centerB - centerA;
      float distSqr = vec.sqrMagnitude;
      float radiusSum = radiusA + radiusB;
      float radiusSumSqr = radiusSum * radiusSum;

      if (distSqr > radiusSumSqr)
        return false;

      Vector3 dir = VectorUtil.NormalizeSafe(vec, Vector3.zero);
      contactOut = Pool<Contact>.Get();
      contactOut.BodyA = bodyA;
      contactOut.BodyB = bodyB;
      contactOut.PositionA = centerA + radiusA * dir;
      contactOut.PositionB = centerB - radiusB * dir;
      contactOut.Normal = dir;
      contactOut.Penetration = radiusSum - Mathf.Sqrt(distSqr);

      return true;
    }
  }
}

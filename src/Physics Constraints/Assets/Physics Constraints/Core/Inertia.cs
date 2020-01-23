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
  public struct Inertia
  {
    // https://en.wikipedia.org/wiki/List_of_moments_of_inertia

    public static Matrix3x3 SolidSphere(float mass, float radius)
    {
      float i = (2.0f / 5.0f) * mass * radius * radius;
      return 
        new Matrix3x3
        (
          i, 0.0f, 0.0f, 
          0.0f, i, 0.0f, 
          0.0f, 0.0f, i
        );
    }

    public static Matrix3x3 SolidBox(float mass, Vector3 dimensions)
    {
      float oneTwelfth = 1.0f / 12.0f;
      float xx = dimensions.x * dimensions.x;
      float yy = dimensions.y * dimensions.y;
      float zz = dimensions.z * dimensions.z;
      return 
        new Matrix3x3
        (
          oneTwelfth * mass * (yy + zz), 0.0f, 0.0f, 
          0.0f, oneTwelfth * mass * (xx + zz), 0.0f, 
          0.0f, 0.0f, oneTwelfth * mass * (xx + yy)
        );
    }
  }
}

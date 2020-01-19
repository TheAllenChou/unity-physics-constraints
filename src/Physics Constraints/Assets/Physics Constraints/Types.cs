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

namespace PhysicsConstriants
{
  public struct InertiaTensor
  {
    public Vector3 I0; // row 0
    public Vector3 I1; // row 1
    public Vector3 I2; // row 2

    public static Vector3 operator *(InertiaTensor i, Vector3 v)
    {
      return 
        new Vector3
        (
          Vector3.Dot(i.I0, v), 
          Vector3.Dot(i.I1, v), 
          Vector3.Dot(i.I2, v)
        );
    }
  }
}

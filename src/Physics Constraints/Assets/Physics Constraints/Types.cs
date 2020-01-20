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

    public InertiaTensor(float i00, float i01, float i02, float i10, float i11, float i12, float i20, float i21, float i22)
    {
      I0 = new Vector3(i00, i01, i02);
      I1 = new Vector3(i10, i11, i12);
      I2 = new Vector3(i20, i21, i22);
    }

    public InertiaTensor(Vector3 i0, Vector3 i1, Vector3 i2)
    {
      I0 = i0;
      I1 = i1;
      I2 = i2;
    }

    private static readonly InertiaTensor kIdentity = 
      new InertiaTensor
      (
        1.0f, 0.0f, 0.0f, 
        0.0f, 1.0f, 0.0f, 
        0.0f, 0.0f, 1.0f
      );
    public static InertiaTensor Identity { get { return kIdentity; } }

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

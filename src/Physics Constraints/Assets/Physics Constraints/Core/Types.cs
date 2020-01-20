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

using UnityEngine;

using CjLib;

namespace PhysicsConstraints
{
  [Serializable]
  public class ConstraintParams
  {
    public enum ParameterMode
    {
      Hard, 
      SoftDampingRatio, 
      SoftHalfLife, 
      SoftExponential, 
    }

    public ParameterMode Mode = ParameterMode.SoftDampingRatio;

    [ConditionalField("Mode", ParameterMode.SoftDampingRatio, ParameterMode.SoftHalfLife, Min = 0.0001f, Max = 10.0f)]
    public float FrequencyHz = 5.0f;
    [ConditionalField("Mode", ParameterMode.SoftDampingRatio, Min = 0.0f, Max = 10.0f)]
    public float DampingRatio = 0.5f;
    [ConditionalField("Mode", ParameterMode.SoftHalfLife, ParameterMode.SoftExponential, Min = 0.0001f, Max = 10.0f)]
    public float HalfLife = 0.02f;

    public void GenerateVelocityConstraintParams(float mass, out float dampingCoefficient, out float springConstant)
    {
      float dampingRatio = DampingRatio;
      float angularFrequency = FrequencyHz * ConstraintUtil.TwoPi;
      switch (Mode)
      {
      case ParameterMode.SoftHalfLife:
        dampingRatio = 0.6931472f / (angularFrequency * HalfLife);
        break;
      case ParameterMode.SoftExponential:
        angularFrequency = 0.6931472f / HalfLife;
        dampingRatio = 1.0f;
        break;
      }

      dampingCoefficient = 2.0f * mass * dampingRatio * angularFrequency;
      springConstant = mass * angularFrequency *angularFrequency;
    }
  }

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

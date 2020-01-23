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
    public float I00;
    public float I01;
    public float I02;
    public float I10;
    public float I11;
    public float I12;
    public float I20;
    public float I21;
    public float I22;

    public Vector3 Row0
    {
      get { return new Vector3(I00, I01, I02); }
      set { I00 = value.x; I01 = value.y; I02 = value.z; }
    }
    public Vector3 Row1
    {
      get { return new Vector3(I10, I11, I12); }
      set { I10 = value.x; I11 = value.y; I12 = value.z; }
    }
    public Vector3 Row2
    {
      get { return new Vector3(I20, I21, I22); }
      set { I20 = value.x; I21 = value.y; I22 = value.z; }
    }
    public Vector3 Col0
    {
      get { return new Vector3(I00, I10, I20); }
      set { I00 = value.x; I10 = value.y; I20 = value.z; }
    }
    public Vector3 Col1
    {
      get { return new Vector3(I01, I11, I21); }
      set { I01 = value.x; I11 = value.y; I21 = value.z; }
    }
    public Vector3 Col2
    {
      get { return new Vector3(I02, I12, I22); }
      set { I02 = value.x; I12 = value.y; I22 = value.z; }
    }

    public static InertiaTensor FromRows(Vector3 row0, Vector3 row1, Vector3 row2)
    {
      return 
        new InertiaTensor
        (
          row0.x, row0.y, row0.z, 
          row1.x, row1.y, row1.z, 
          row2.x, row2.y, row2.z
        );
    }

    public static InertiaTensor FromCols(Vector3 col0, Vector3 col1, Vector3 col2)
    {
      return 
        new InertiaTensor
        (
          col0.x, col1.x, col2.x, 
          col0.y, col1.y, col2.y, 
          col0.z, col1.z, col2.z
        );
    }

    public InertiaTensor(float i00, float i01, float i02, float i10, float i11, float i12, float i20, float i21, float i22)
    {
      I00 = i00; I01 = i01; I02 = i02;
      I10 = i10; I11 = i11; I12 = i12;
      I20 = i20; I21 = i21; I22 = i22;
    }

    private static readonly InertiaTensor kIdentity = 
      new InertiaTensor
      (
        1.0f, 0.0f, 0.0f, 
        0.0f, 1.0f, 0.0f, 
        0.0f, 0.0f, 1.0f
      );
    public static InertiaTensor Identity { get { return kIdentity; } }

    public static Vector3 Mul(InertiaTensor i, Vector3 v)
    {
      return 
        new Vector3
        (
          Vector3.Dot(i.Row0, v), 
          Vector3.Dot(i.Row1, v), 
          Vector3.Dot(i.Row2, v)
        );
    }

    public static Vector3 Mul(Vector3 v, InertiaTensor i)
    {
      return 
        new Vector3
        (
          Vector3.Dot(v, i.Col0), 
          Vector3.Dot(v, i.Col1), 
          Vector3.Dot(v, i.Col2)
        );
    }

    public static float Mul(Vector3 a, InertiaTensor i, Vector3 b)
    {
      return Vector3.Dot(Mul(a, i), b);
    }
  }
}

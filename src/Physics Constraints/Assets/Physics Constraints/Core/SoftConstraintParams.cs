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
      springConstant = mass * angularFrequency * angularFrequency;
    }
  }
}

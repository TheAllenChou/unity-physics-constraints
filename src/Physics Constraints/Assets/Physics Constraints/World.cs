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
using UnityEngine;

namespace PhysicsConstriants
{
  public class World : MonoBehaviour
  {
    private static readonly int Iterations = 100;

    private static HashSet<Constraint> s_constraints;
    public static void Register(Constraint c)
    {
      ValidateWorld();
      s_constraints.Add(c);
    }
    public static void Unregister(Constraint c)
    {
      ValidateWorld();
      s_constraints.Remove(c);
    }

    private static HashSet<Body> s_bodies;
    public static void Register(Body b)
    {
      ValidateWorld();
      s_bodies.Add(b);
    }
    public static void Unregister(Body b)
    {
      ValidateWorld();
      s_bodies.Remove(b);
    }

    private static GameObject s_world;
    private static void ValidateWorld()
    {
      if (s_world != null)
        return;

      s_constraints = new HashSet<Constraint>();
      s_bodies = new HashSet<Body>();

      s_world = new GameObject("World (Physics Constraints)");
      s_world.AddComponent<World>();
    }

    private void FixedUpdate()
    {
      float dt = Mathf.Max(ConstraintUtil.Epsilon, Time.fixedDeltaTime);
      Step(Time.fixedDeltaTime);
    }

    public static void Step(float dt)
    {
      foreach (var constraint in s_constraints)
      {
        constraint.InitVelocityConstraint(dt);
      }

      for (int i = 0; i < Iterations; ++i)
      {
        foreach (var constraint in s_constraints)
        {
          constraint.SolveVelocityConstraint(dt);
        }
      }

      foreach (var body in s_bodies)
      {
        body.Integrate(dt);
      }
    }
  }
}

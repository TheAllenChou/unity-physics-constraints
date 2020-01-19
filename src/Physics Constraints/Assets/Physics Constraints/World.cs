/******************************************************************************/
/*
  Project - Physics Constraints
            https://github.com/TheAllenChou/unity-physics-constraints
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsConstriants
{
  public class World
  {
    private HashSet<Constraint> m_constraints = new HashSet<Constraint>();
    public void Register(Constraint c) {  m_constraints.Add(c); }
    public void Unregister(Constraint c) {  m_constraints.Remove(c); }

    public void Step(float dt)
    {
      foreach (var c in m_constraints)
      {
        c.Solve(dt);
      }
    }
  }
}

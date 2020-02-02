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
  [RequireComponent(typeof(Body))]
  public class Contact
  {
    public Body BodyA;
    public Body BodyB;
    public Vector3 PositionA;
    public Vector3 PositionB;
    public Vector3 Normal;
    public float Penetration;

    public void InitVelocityConstraint(float dt)
    {

    }

    public void SolveVelocityConstraint(float dt)
    {

    }
  }
}

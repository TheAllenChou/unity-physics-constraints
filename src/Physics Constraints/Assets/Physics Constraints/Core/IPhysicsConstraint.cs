/******************************************************************************/
/*
  Project - Physics Constraints
            https://github.com/TheAllenChou/unity-physics-constraints
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

namespace PhysicsConstraints
{
  public interface IPhysicsConstraint
  {
    void InitVelocityConstraint(float dt);
    void SolveVelocityConstraint(float dt);
  }
}


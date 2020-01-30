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

using PhysicsConstraints;

public class SystemicPointConstraintWithRotationMain : MonoBehaviour
{
  private void Start()
  {
    World.Gravity = 9.8f * Vector3.down;
  }
}

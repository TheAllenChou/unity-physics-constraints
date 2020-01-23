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

public class SimplePointConstraintWithRotationMain : MonoBehaviour
{
  public float Beta = 0.02f;

  public GameObject Box;
  public GameObject Target;

  private Vector3 v = Vector3.zero;

  private void Update()
  {
    if (Box == null)
      return;

    if (Target == null)
      return;

    float dt = Time.deltaTime;

    // TODO
  }
}

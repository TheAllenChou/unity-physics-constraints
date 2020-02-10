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

public class InlinePointConstraintMain : MonoBehaviour
{
  public float Beta = 0.02f;

  public GameObject Object;
  public GameObject Target;

  private Vector3 v = Vector3.zero;

  private void Update()
  {
    if (Object == null)
      return;

    if (Target == null)
      return;

    float dt = Time.deltaTime;
    Vector3 c = Object.transform.position - Target.transform.position;

    v += (-Beta / dt) * c;
    v *= 0.9f; // temp magic cheat
    Object.transform.position += v * dt;
  }
}

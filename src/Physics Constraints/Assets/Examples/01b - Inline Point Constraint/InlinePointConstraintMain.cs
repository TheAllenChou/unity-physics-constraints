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

public class SimplePointConstraintMain : MonoBehaviour
{
  public float Beta = 0.02f;

  public GameObject Ball;
  public GameObject P;

  private Vector3 v = Vector3.zero;

  private void Update()
  {
    if (Ball == null)
      return;

    if (P == null)
      return;

    float dt = Time.deltaTime;
    Vector3 c = Ball.transform.position - P.transform.position;

    v += (-Beta / dt) * c;
    v *= 0.9f; // temp cheat
    Ball.transform.position += v * dt;
  }
}

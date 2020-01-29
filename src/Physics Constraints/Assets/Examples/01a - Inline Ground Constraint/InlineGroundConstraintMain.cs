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

public class InlineGroundConstraintMain : MonoBehaviour
{
  public float Gravity = -9.8f;
  public float Beta = 0.2f;

  public GameObject Ball;

  private float vy = 0.0f;

  private void FixedUpdate()
  {
    if (Ball == null)
      return;

    float dt = Time.deltaTime;
    float y = Ball.transform.position.y;

    vy += Gravity * dt;

    if (y <= 0.0f)
    {
      vy = -(Beta / dt) * y;
    }

    y += vy * dt;

    Vector3 pos = Ball.transform.position;
    pos.y = y;
    Ball.transform.position = pos;
  }
}

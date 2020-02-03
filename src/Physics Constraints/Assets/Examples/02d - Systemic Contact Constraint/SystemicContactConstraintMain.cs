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
  public class SystemicContactConstraintMain : MonoBehaviour
  {
    public GameObject[] Objects;

    private void Update()
    {
      if (Objects == null)
        return;

      // N-squared "broadphase"
      for (int i = 0; i < Objects.Length; ++i)
      {
        if (Objects[i] == null)
          continue;

        var bodyA = Objects[i].GetComponent<Body>();
        if (bodyA == null)
          continue;

        for (int j = i + 1; j < Objects.Length; ++j)
        {
          if (Objects[j] == null)
            continue;

          var bodyB = Objects[j].GetComponent<Body>();
          if (bodyB == null)
            continue;

          // collision detection
          Contact contact = null;
          float radius = 0.5f;
          bool sphereCollided =
            Collision.SphereVsSphere
            (
              bodyA, bodyA.transform.position, radius,
              bodyB, bodyB.transform.position, radius,
              ref contact
            );
          
          if (sphereCollided)
          {
            World.AddContact(contact);
          }
        }
      }
    }
  }
}

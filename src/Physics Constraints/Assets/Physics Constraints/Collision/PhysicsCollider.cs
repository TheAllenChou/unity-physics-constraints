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
  [RequireComponent(typeof(PhysicsBody))]
  public abstract class PhysicsCollider : MonoBehaviour
  {
    public enum ShapeType
    {
      Sphere,
      Plane,
    }

    private ShapeType m_type;
    public ShapeType Type { get { return m_type; } }

    protected PhysicsCollider(ShapeType type)
    {
      m_type = type;
    }

    private void OnEnable()
    {
      World.Register(this);
    }

    private void OnDisable()
    {
      World.Unregister(this);
    }
  }
}


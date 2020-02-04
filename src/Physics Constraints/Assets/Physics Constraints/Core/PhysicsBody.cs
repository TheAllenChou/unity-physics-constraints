/******************************************************************************/
/*
  Project - Physics Constraints
            https://github.com/TheAllenChou/unity-physics-constraints
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

using CjLib;
using UnityEngine;

namespace PhysicsConstraints
{
  public class PhysicsBody : MonoBehaviour
  {
    // mass
    private float m_mass;
    private float m_inverseMass;
    public float Mass
    {
      get { return m_mass; }
      set
      {
        m_mass = value;
        m_inverseMass = (m_mass == float.MaxValue) ? 0.0f : 1.0f / m_mass;
      }
    }
    public float InverseMass
    {
      get { return m_inverseMass; }
      set
      {
        m_inverseMass = value;
        m_mass = (m_inverseMass == 0.0f) ? float.MaxValue : 1.0f / m_inverseMass;
      }
    }

    // inertia tensor
    public Matrix3x3 m_inertia;
    public Matrix3x3 m_inverseInertia;
    public Matrix3x3 Inertia
    {
      get { return m_inertia; }
      set
      {
        m_inertia = value;
        m_inverseInertia = value; // TODO
      }
    }
    public Matrix3x3 InverseInertia
    {
      get { return m_inverseInertia; }
    }

    // center of mass
    [HideInInspector]
    public Vector3 m_centerOfMassLs;
    [HideInInspector]
    public Vector3 CenterOfMassLs
    {
      get { return m_centerOfMassLs; }
      set { m_centerOfMassLs = value; }
    }
    public Vector3 CenterOfMassWs
    {
      get { return transform.TransformPoint(m_centerOfMassLs); }
    }

    // velocity
    public Vector3 LinearVelocity;
    public Vector3 AngularVelocity;
    [Range(0.0f, 1.0f)]
    public float LinearDrag = 0.0f;
    [Range(0.0f, 1.0f)]
    public float AngularDrag = 0.0f;

    // gravity
    [Range(0.0f, 1.0f)]
    public float GravityScale = 1.0f;

    // contact
    [Range(0.0f, 1.0f)]
    public float ContactBeta = 0.5f;
    [Range(0.0f, 1.0f)]
    public float Restitution = 0.7f;
    [Min(0.0f)]
    public float Friction = 1.0f;

    // transform
    public bool LockPosition = false;
    public bool LockRotation = false;

    public PhysicsBody()
    {
      Mass = 1.0f;
      Inertia = Matrix3x3.Identity;
      CenterOfMassLs = Vector3.zero;
    }

    private void OnEnable()
    {
      World.Register(this);
    }

    private void OnDisable()
    {
      World.Unregister(this);
    }

    public void ApplyImpulse(Vector3 impulse, Vector3 atWs)
    {
      if (LockPosition)
      {
        LinearVelocity = Vector3.zero;
      }
      else
      {
        LinearVelocity += InverseMass * impulse;
      }

      if (LockRotation)
      {
        AngularVelocity = Vector3.zero;
      }
      else
      {
        Vector3 atLs = transform.InverseTransformPoint(atWs);
        Vector3 r = atLs - CenterOfMassLs;
        Vector3 angularImpulse = Vector3.Cross(r, impulse);
        AngularVelocity += InverseInertia * angularImpulse;
      }
    }

    public void Integrate(float dt)
    {
      if (!LockPosition)
      {
        transform.position += LinearVelocity * dt;;
      }

      if (!LockRotation)
      {
        Quaternion q = QuaternionUtil.AxisAngle(VectorUtil.NormalizeSafe(AngularVelocity, Vector3.forward), AngularVelocity.magnitude * dt);
        transform.rotation = q * transform.rotation;
      }
    }
  }
}


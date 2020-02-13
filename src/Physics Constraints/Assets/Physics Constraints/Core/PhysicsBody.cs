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
using System.Diagnostics;
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

    // moment of inertia
    private Matrix3x3 m_inertiaLs;
    private Matrix3x3 m_inverseInertiaLs;
    private Matrix3x3 m_inverseInertiaWs;
    public Matrix3x3 InertiaLs
    {
      get { return m_inertiaLs; }
      set
      {
        m_inertiaLs = value;
        m_inverseInertiaLs = m_inertiaLs.Inverted;
      }
    }
    public Matrix3x3 InverseInertiaLs { get { return m_inverseInertiaLs; } }
    public Matrix3x3 InverseInertiaWs { get { return m_inverseInertiaWs; } }
    public void UpdateInertiaWs()
    {
      var t = transform;
      var world2Local =
        Matrix3x3.FromRows
        (
          t.TransformVector(new Vector3(1.0f, 0.0f, 0.0f)),
          t.TransformVector(new Vector3(0.0f, 1.0f, 0.0f)),
          t.TransformVector(new Vector3(0.0f, 0.0f, 1.0f))
        );
      m_inverseInertiaWs = world2Local.Transposed * m_inverseInertiaLs * world2Local;
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
      InertiaLs = Matrix3x3.Identity;
      CenterOfMassLs = Vector3.zero;
    }

    private void OnEnable()
    {
      World.Register(this);

      UpdateInertiaWs();
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
        AngularVelocity += InverseInertiaWs * angularImpulse;
      }
    }

    public void Integrate(float dt)
    {
      if (!LockPosition)
      {
        transform.position = VectorUtil.Integrate(transform.position, LinearVelocity, dt);
      }

      if (!LockRotation)
      {
        transform.rotation = QuaternionUtil.Integrate(transform.rotation, AngularVelocity, dt);
      }
    }
  }
}


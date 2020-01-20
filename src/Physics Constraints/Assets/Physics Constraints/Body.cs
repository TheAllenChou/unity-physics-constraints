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

namespace PhysicsConstriants
{
  public class Body : MonoBehaviour
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
    public InertiaTensor m_inertia;
    public InertiaTensor m_inverseInertia;
    public InertiaTensor Inertia
    {
      get { return m_inertia; }
      set
      {
        m_inertia = value;
        m_inverseInertia = value; // TODO
      }
    }
    public InertiaTensor InverseInertia
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
    [HideInInspector]
    public Vector3 LinearVelocity;
    [HideInInspector]
    public Vector3 AngularVelocity;

    // transform
    public bool LockPosition = false;
    public bool LockRotation = false;

    public Body()
    {
      Mass = 1.0f;
      Inertia = InertiaTensor.Identity;
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
        Quaternion rotation = transform.rotation;
        Vector3 rotationVec = QuaternionUtil.GetAngle(rotation) * QuaternionUtil.GetAxis(rotation);
        rotationVec += AngularVelocity * dt;
        transform.rotation = QuaternionUtil.AxisAngle(VectorUtil.NormalizeSafe(rotationVec, Vector3.zero), rotationVec.magnitude);
      }
    }
  }
}


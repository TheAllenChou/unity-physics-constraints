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
  public class PlaneConstraint : MonoBehaviour, IPhysicsConstraint
  {
    public ConstraintParams ConstraintParams = new ConstraintParams();

    public float Restitution = 0.8f;

    public Transform Plane;
    private Vector3 m_n; // plane normal
    private Vector3 m_p; // plane point
    private float m_d;   // plane dot

    private Vector3 m_impulse;
    private float m_effectiveMass;
    private float m_gamma;
    private Vector3 m_bias;

    private void OnEnable()
    {
      World.Register(this);
    }

    private void OnDisable()
    {
      World.Unregister(this);
    }

    public void InitVelocityConstraint(float dt)
    {
      var body = GetComponent<PhysicsBody>();

      float beta;
      ConstraintUtil.VelocityConstraintBias(body.Mass, ConstraintParams, dt, out beta, out m_gamma);

      m_n = (Plane != null) ? Plane.transform.up : Vector3.up;
      m_p = (Plane != null) ? Plane.transform.position : transform.position;
      m_d = Vector3.Dot(transform.position - m_p, m_n);

      if (m_d > 0.0f)
        return;

      Vector3 cPos = m_d * m_n;
      m_bias = beta * cPos + Restitution * Vector3.Project(-body.LinearVelocity, m_n);
      m_effectiveMass = 1.0f / (body.InverseMass + m_gamma);

      // TODO: warm starting
      m_impulse = Vector3.zero;
    }

    public void SolveVelocityConstraint(float dt)
    {
      if (m_d > 0.0f)
        return;

      var body = GetComponent<PhysicsBody>();

      Vector3 cVel = Vector3.Project(body.LinearVelocity, m_n) + m_bias + m_gamma * m_impulse;

      Vector3 impulse = m_effectiveMass * (-cVel);
      // TODO: max impulse
      m_impulse += impulse;

      body.LinearVelocity += body.InverseMass * impulse;
    }
  }
}

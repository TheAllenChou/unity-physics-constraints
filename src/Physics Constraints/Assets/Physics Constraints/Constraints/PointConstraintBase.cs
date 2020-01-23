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
  [RequireComponent(typeof(Body))]
  public abstract class PointConstraintBase : MonoBehaviour, Constraint
  {
    public ConstraintParams ConstraintParams = new ConstraintParams();

    protected abstract Vector3 GetAnchor();

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
      var body = GetComponent<Body>();

      float beta;
      ConstraintUtil.VelocityConstraintBias(body.Mass, ConstraintParams, dt, out beta, out m_gamma);

      Vector3 anchor = GetAnchor();
      Vector3 cPos = transform.position - anchor;
      m_bias = beta * cPos;
      m_effectiveMass = 1.0f / (body.InverseMass + m_gamma);

      // TODO: warm starting
      m_impulse = Vector3.zero;
    }

    public void SolveVelocityConstraint(float dt)
    {
      var body = GetComponent<Body>();

      Vector3 cVel = body.LinearVelocity + m_bias + m_gamma * m_impulse;
      Vector3 impulse = m_effectiveMass * (-cVel);
      // TODO: max impulse
      m_impulse += impulse;

      body.LinearVelocity += body.InverseMass * impulse;
    }
  }
}

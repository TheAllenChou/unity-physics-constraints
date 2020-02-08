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
  public abstract class PointConstraintBase : MonoBehaviour, IPhysicsConstraint
  {
    public ConstraintParams ConstraintParams = new ConstraintParams();
    public bool EnableRotation = false;

    protected abstract Vector3 GetTarget();
    protected virtual Vector3 GetLocalAnchor() { return Vector3.zero; }

    private Vector3 m_lambda;
    private Matrix3x3 m_effectiveMass;
    private Matrix3x3 m_cross;
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

      Vector3 r = transform.rotation * GetLocalAnchor();

      Vector3 cPos = (transform.position + r) - GetTarget();
      Vector3 cVel = body.LinearVelocity + Vector3.Cross(body.AngularVelocity, r);

      m_cross = Matrix3x3.PostCross(r);
      Matrix3x3 k = body.InverseMass * Matrix3x3.Identity;
      if (EnableRotation)
        k += m_cross * body.InverseInertiaWs * m_cross.Transposed;

      k += m_gamma * Matrix3x3.Identity;

      m_bias = beta * cPos;
      m_effectiveMass = k.Inverted;

      // TODO: warm starting
      m_lambda = Vector3.zero;
    }

    public void SolveVelocityConstraint(float dt)
    {
      var body = GetComponent<PhysicsBody>();

      Vector3 cVel = body.LinearVelocity + m_bias + m_gamma * m_lambda;
      Vector3 lambda = m_effectiveMass * (-cVel);
      m_lambda += lambda;

      body.LinearVelocity += body.InverseMass * lambda;

      if (EnableRotation)
        body.AngularVelocity += body.InverseInertiaWs * m_cross.Transposed * lambda;
    }
  }
}

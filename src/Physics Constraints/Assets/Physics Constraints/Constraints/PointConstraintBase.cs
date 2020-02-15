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

    private Vector3 m_totalLambda;
    private Vector3 m_r;
    private Matrix3x3 m_effectiveMass;
    private Matrix3x3 m_cross;
    private float m_sbc;
    private Vector3 m_positionErrorBias;

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

      float pbc;
      ConstraintUtil.VelocityConstraintBias(body.Mass, ConstraintParams, dt, out pbc, out m_sbc);

      m_r = transform.rotation * (GetLocalAnchor() - body.CenterOfMassLs);

      Vector3 cPos = (transform.position + m_r) - GetTarget();
      Vector3 cVel = body.LinearVelocity + Vector3.Cross(body.AngularVelocity, m_r);

      m_cross = Matrix3x3.Skew(-m_r);
      Matrix3x3 k = body.InverseMass * Matrix3x3.Identity;
      if (EnableRotation)
        k += m_cross * body.InverseInertiaWs * m_cross.Transposed;

      k += m_sbc * Matrix3x3.Identity;

      m_positionErrorBias = pbc * cPos;
      m_effectiveMass = k.Inverted;

      // TODO: warm starting
      m_totalLambda = Vector3.zero;
    }

    public void SolveVelocityConstraint(float dt)
    {
      var body = GetComponent<PhysicsBody>();

      Vector3 cVel = body.LinearVelocity + Vector3.Cross(body.AngularVelocity, m_r);
      Vector3 jvb = cVel + m_positionErrorBias + m_sbc * m_totalLambda;
      Vector3 lambda = m_effectiveMass * (-jvb);
      m_totalLambda += lambda;

      body.LinearVelocity += body.InverseMass * lambda;

      if (EnableRotation)
        body.AngularVelocity += body.InverseInertiaWs * m_cross.Transposed * lambda;
    }
  }
}

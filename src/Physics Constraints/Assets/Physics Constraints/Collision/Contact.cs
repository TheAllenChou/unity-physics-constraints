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
  [RequireComponent(typeof(PhysicsBody))]
  public class Contact : IPhysicsConstraint
  {
    public PhysicsBody BodyA;
    public PhysicsBody BodyB;
    public Vector3 PositionA;
    public Vector3 PositionB;
    public Vector3 Normal;
    public float Penetration;

    private Vector3 m_rA;
    private Vector3 m_rB;

    private Jacobian m_jN;  // Jacobian for contact normal (collision resolution)
    private Jacobian m_jT; // Jacobian for contact tangent (friction)
    private Jacobian m_jB; // Jacobian for contact bi-tangent (friction)

    public Contact()
    {
      m_jN = new Jacobian(Jacobian.Type.Normal);
      m_jT = new Jacobian(Jacobian.Type.Tangent);
      m_jB = new Jacobian(Jacobian.Type.Tangent);
    }

    public void InitVelocityConstraint(float dt)
    {
      m_rA = PositionA - BodyA.transform.position;
      m_rB = PositionB - BodyB.transform.position;

      Vector3 tangent;
      Vector3 bitangent;
      VectorUtil.FormOrthogonalBasis(Normal, out tangent, out bitangent);

      m_jN.Init(this, Normal);
      m_jT.Init(this, tangent);
      m_jB.Init(this, bitangent);
    }

    public void SolveVelocityConstraint(float dt)
    {
      m_jN.Resolve(this, dt);
      m_jT.Resolve(this, dt);
      m_jB.Resolve(this, dt);
    }

    // Jacobian for eliminating relative velocity along a specific direction
    // (normal, tangent, or bi-tangent)
    private struct Jacobian
    {
      public enum Type
      {
        Normal, 
        Tangent, 
      }

      Type m_type;

      private Vector3 m_va; // Jacobian components for linear velocity of body A
      private Vector3 m_wa; // Jacobian components for angular velocity of body A
      private Vector3 m_vb; // Jacobian components for linear velocity of body B
      private Vector3 m_wb; // Jacobian components for angular velocity of body B
      private float m_effectiveMass;
      private float m_totalLambda;

      public Jacobian(Type type)
      {
        m_type = type;
        m_va = Vector3.zero;
        m_wa = Vector3.zero;
        m_vb = Vector3.zero;
        m_wb = Vector3.zero;
        m_effectiveMass = 0.0f;
        m_totalLambda = 0.0f;
      }

      public void Init(Contact contact, Vector3 dir)
      {
        m_va = -dir;
        m_wa = -Vector3.Cross(contact.m_rA, dir);
        m_vb = dir;
        m_wb = Vector3.Cross(contact.m_rB, dir);

        float k = 
            contact.BodyA.InverseMass 
          + Vector3.Dot(m_wa, contact.BodyA.InverseInertiaWs * m_wa) 
          + contact.BodyB.InverseMass 
          + Vector3.Dot(m_wb, contact.BodyB.InverseInertiaWs * m_wb);

        m_effectiveMass = 1.0f / k;
        m_totalLambda = 0.0f;
      }

      public void Resolve(Contact contact, float dt)
      {
        Vector3 dir = m_vb;

        // JV = Jacobian * velocity vector
        float jv = 
            Vector3.Dot(m_va, contact.BodyA.LinearVelocity) 
          + Vector3.Dot(m_wa, contact.BodyA.AngularVelocity) 
          + Vector3.Dot(m_vb, contact.BodyB.LinearVelocity) 
          + Vector3.Dot(m_wb, contact.BodyB.AngularVelocity);

        // b = bias
        float b = 0.0f;
        if (m_type == Type.Normal)
        {
          float beta = contact.BodyA.ContactBeta * contact.BodyB.ContactBeta;
          float restitution = contact.BodyA.Restitution * contact.BodyB.Restitution;
          Vector3 relativeVelocity = 
            - contact.BodyA.LinearVelocity 
            - Vector3.Cross(contact.BodyA.AngularVelocity, contact.m_rA) 
            + contact.BodyB.LinearVelocity 
            + Vector3.Cross(contact.BodyB.AngularVelocity, contact.m_rB);
          float closingVelocity = Vector3.Dot(relativeVelocity, dir);
          b = -(beta / dt) * contact.Penetration + restitution * closingVelocity;
        }

        // raw lambda
        float lambda = m_effectiveMass * (-(jv + b));

        // clamped lambda
        //   normal  / contact resolution  :  lambda >= 0
        //   tangent / friction            :  -maxFriction <= lambda <= maxFriction
        float oldTotalLambda = m_totalLambda;
        switch (m_type)
        {
          case Type.Normal:
            m_totalLambda = Mathf.Max(0.0f, m_totalLambda + lambda);
            break;

          case Type.Tangent:
            float friction = contact.BodyA.Friction * contact.BodyB.Friction;
            float maxFriction = friction * contact.m_jN.m_totalLambda;
            m_totalLambda = Mathf.Clamp(m_totalLambda + lambda, -maxFriction, maxFriction);
            break;
        }
        lambda = m_totalLambda - oldTotalLambda;

        // velocity correction
        contact.BodyA.LinearVelocity += contact.BodyA.InverseMass * m_va * lambda;
        contact.BodyA.AngularVelocity += contact.BodyA.InverseInertiaWs * m_wa * lambda;
        contact.BodyB.LinearVelocity += contact.BodyB.InverseMass * m_vb * lambda;
        contact.BodyB.AngularVelocity += contact.BodyB.InverseInertiaWs * m_wb * lambda;
      }
    }
  }
}

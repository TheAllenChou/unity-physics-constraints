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

using PhysicsConstraints;
using CjLib;

public class InlinePointConstraintWithRotationMain : MonoBehaviour
{
  public float Beta = 0.02f;

  public GameObject Object;
  public GameObject Target;

  public Vector3 Gravity = new Vector3(0.0f, -20.0f, 0.0f);

  private float mass;
  private float massInv;
  private Matrix3x3 inertiaLs;
  private Matrix3x3 inertiaInvLs;

  private Vector3 rLocal = Vector3.zero; // corner offset
  private Vector3 v = Vector3.zero; // linear velocity
  private Vector3 a = Vector3.zero; // angular velocity

  private void Start()
  {
    mass = 1.0f;
    massInv = 1.0f / mass;

    inertiaLs = Matrix3x3.Identity; // Inertia.SolidBox(mass, 1.0f * Vector3.one);
    inertiaInvLs = inertiaLs.Inverted;

    rLocal = 0.5f * Vector3.one;
  }

  private void FixedUpdate()
  {
    if (Object == null)
      return;

    if (Target == null)
      return;

    float dt = Time.fixedDeltaTime;

    Vector3 r = Object.transform.rotation * rLocal;

    var t = Object.transform;
    Matrix3x3 world2Local =
      Matrix3x3.FromRows
      (
        t.TransformVector(new Vector3(1.0f, 0.0f, 0.0f)),
        t.TransformVector(new Vector3(0.0f, 1.0f, 0.0f)),
        t.TransformVector(new Vector3(0.0f, 0.0f, 1.0f))
      );
    Matrix3x3 inertiaInvWs = world2Local.Transposed * inertiaInvLs * world2Local;

    // gravity
    v += Gravity * dt;

    // constraint errors
    Vector3 cPos = (Object.transform.position + r) - Target.transform.position;
    Vector3 cVel = v + Vector3.Cross(a, r);

    // constraint resolution
    Matrix3x3 s = Matrix3x3.Skew(-r);
    Matrix3x3 k = massInv * Matrix3x3.Identity + s * inertiaInvWs * s.Transposed;
    Matrix3x3 effectiveMass = k.Inverted;
    Vector3 lambda = effectiveMass * (-(cVel + (Beta / dt) * cPos));

    // velocity correction
    v += massInv * lambda;
    a += (inertiaInvWs * s.Transposed) * lambda;
    v *= 0.98f; // temp magic
    a *= 0.98f; // temp magic

    // integration
    Object.transform.position += v * dt;
    Object.transform.rotation = QuaternionUtil.Integrate(Object.transform.rotation, a, dt);
  }
}

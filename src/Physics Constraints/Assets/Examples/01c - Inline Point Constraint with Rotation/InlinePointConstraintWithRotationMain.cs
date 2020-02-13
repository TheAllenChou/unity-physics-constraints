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

  public Vector3 Gravity = new Vector3(0.0f, -9.8f, 0.0f);

  private float mass;
  private float massInv;
  private Matrix3x3 inertia;
  private Matrix3x3 inertiaInv;

  private Vector3 rLocal = Vector3.zero; // corner offset
  private Vector3 v = Vector3.zero; // linear velocity
  private Vector3 a = Vector3.zero; // angular velocity

  private void Start()
  {
    mass = 1.0f;
    massInv = 1.0f / mass;

    inertia = Inertia.SolidBox(mass, 1.0f * Vector3.one);
    inertiaInv = inertia.Inverted;

    rLocal = 0.5f * Vector3.one;
  }

  private void Update()
  {
    if (Object == null)
      return;

    if (Target == null)
      return;

    float dt = Time.deltaTime;

    Vector3 r = Object.transform.rotation * rLocal;

    // gravity
    v += Gravity * dt;

    // constraint errors
    Vector3 cPos = (Object.transform.position + r) - Target.transform.position;
    Vector3 cVel = v + Vector3.Cross(a, r);

    // constraint resolution
    Matrix3x3 s = Matrix3x3.Skew(-r);
    Matrix3x3 k = massInv * Matrix3x3.Identity + s * inertiaInv * s.Transposed;
    Matrix3x3 effectiveMass = k.Inverted;
    Vector3 lambda = effectiveMass * (-(cVel + (Beta / dt) * cPos));

    // velocity correction
    v += massInv * lambda;
    a += (inertiaInv * s.Transposed) * lambda;
    v *= 0.98f; // temp magic cheat
    a *= 0.98f; // temp magic cheat

    // integration
    Object.transform.position += v * dt;
    Quaternion q = QuaternionUtil.AxisAngle(VectorUtil.NormalizeSafe(a, Vector3.forward), a.magnitude * dt);
    Object.transform.rotation = q * Object.transform.rotation;
  }
}

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

using CjLib;

namespace PhysicsConstraints
{
  public class Collision
  {
    public static bool DetectCollision(PhysicsCollider colliderA, PhysicsCollider colliderB, ref Contact contact)
    {
      var bodyA = colliderA.GetComponent<PhysicsBody>();
      var bodyB = colliderB.GetComponent<PhysicsBody>();
      var transformA = colliderA.transform;
      var transformB = colliderB.transform;
      switch (colliderA.Type)
      {
        case PhysicsCollider.ShapeType.Sphere:
          var sphereA = (SphereCollider)colliderA;
          switch (colliderB.Type)
          {
            case PhysicsCollider.ShapeType.Sphere:
              var sphereB = (SphereCollider)colliderB;
              return SphereVsSphere(bodyA, transformA.position, sphereA.radius, bodyB, transformB.position, sphereB.radius, ref contact);

            case PhysicsCollider.ShapeType.Plane:
              var planeB = (PlaneCollider)colliderB;
              return SphereVsPlane(bodyA, transformA.position, sphereA.radius, bodyB, transformB.position, planeB.Normal, ref contact);
          }
          break;

        case PhysicsCollider.ShapeType.Plane:
          var planeA = (PlaneCollider)colliderA;
          switch (colliderB.Type)
          {
            case PhysicsCollider.ShapeType.Sphere:
              var sphereB = (SphereCollider)colliderB;
              return SphereVsPlane(bodyB, transformB.position, sphereB.radius, bodyA, transformA.position, planeA.Normal, ref contact);

            case PhysicsCollider.ShapeType.Plane:
              // no plane-plane collision detection
              return false;
          }
          break;
      }

      return false;
    }

    public static bool SphereVsSphere
    (
      PhysicsBody bodyA, 
      Vector3 centerA, 
      float radiusA, 
      PhysicsBody bodyB, 
      Vector3 centerB, 
      float radiusB, 
      ref Contact contactOut
    )
    {
      Vector3 vec = centerB - centerA;
      float distSqr = vec.sqrMagnitude;
      float radiusSum = radiusA + radiusB;
      float radiusSumSqr = radiusSum * radiusSum;

      if (distSqr > radiusSumSqr)
        return false;

      Vector3 dir = VectorUtil.NormalizeSafe(vec, Vector3.zero);
      contactOut = Pool<Contact>.Get();
      contactOut.BodyA = bodyA;
      contactOut.BodyB = bodyB;
      contactOut.PositionA = centerA + radiusA * dir;
      contactOut.PositionB = centerB - radiusB * dir;
      contactOut.Normal = dir;
      contactOut.Penetration = radiusSum - Mathf.Sqrt(distSqr);

      return true;
    }

    public static bool SphereVsPlane
    (
      PhysicsBody bodyA, 
      Vector3 centerA, 
      float radiusA, 
      PhysicsBody bodyB, 
      Vector3 pointB, 
      Vector3 normalB, 
      ref Contact contactOut
    )
    {
      Vector3 r = centerA - pointB;
      float d = Vector3.Dot(r, normalB);
      if (d > radiusA)
        return false;

      contactOut = Pool<Contact>.Get();
      contactOut.BodyA = bodyA;
      contactOut.BodyB = bodyB;
      contactOut.PositionA = centerA - radiusA * normalB;
      contactOut.PositionB = centerA - d * normalB;
      contactOut.Normal = -normalB;
      contactOut.Penetration = (radiusA - d);

      return true;
    }
  }
}

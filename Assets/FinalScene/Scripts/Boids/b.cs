// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;

// public class b : MonoBehaviour
// {
//     private Rigidbody rb;
//     private List<Collider> groundCollider = new List<Collider>();
//     private bool grounded = false;

//     public bool withGoto = false;
//     public bool withCohesion = false;
//     public bool withAvoid = false;
//     public bool withDEBUG = false;

//     public float speed;
//     public float wallRay;
//     public float avoidRay;
//     public float cohesionRay;
//     public float attractionRay;
//     public float filter;
//     public Transform _myTransform;

//     public void Init(float _speed, float _wallRay, float _avoidRay, float _cohesionRay, float _attractionRay, float _filter)
//     {
//         speed = _speed;
//         wallRay = _wallRay;
//         avoidRay = _avoidRay;
//         cohesionRay = _cohesionRay;
//         attractionRay = _attractionRay;
//         filter = _filter;
//         rb = GetComponent<Rigidbody>();
//     }

//     public void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         _myTransform = this.transform;
//     }

//     void Update()
//     {
//         if (!grounded)
//         {
//             rb.AddForce(Physics.gravity * 0.5f, ForceMode.Acceleration);
//         }
//         else
//         {
//             HandleBoidBehaviors();
//         }
//     }

//     private void HandleBoidBehaviors()
//     {
//         if (withDEBUG) { Debug.Log("//////////////////////////////////////////////////////////////////////////////////"); }

//         Quaternion newRota = new Quaternion(0, _myTransform.rotation.y, 0, _myTransform.rotation.w);
//         transform.rotation = newRota;

//         float rotation = 0.0f;
//         float oldRotate;
//         String modeUsed = "";

//         if (withGoto)
//         {
//             rotation = GoToBoidRcast(rotation, cohesionRay, attractionRay);
//             if (rotation != 0.0f) modeUsed = "GoTo Green Line";
//         }

//         if (withCohesion)
//         {
//             oldRotate = rotation;
//             rotation = CohesionBoidRcast(rotation, avoidRay, cohesionRay);
//             if (rotation != oldRotate) modeUsed = "Cohesion blue and pink line";
//         }

//         if (withAvoid)
//         {
//             oldRotate = rotation;
//             rotation = AvoidBoidRcast(rotation, avoidRay);
//             if (rotation != oldRotate) modeUsed = "Avoid yellow line";
//         }

//         oldRotate = rotation;
//         rotation = AvoidWallRcast(rotation, wallRay);
//         if (rotation != oldRotate) modeUsed = "Wall red";

//         if (withDEBUG) { Debug.Log("FINAL ROTATION : " + rotation + " " + modeUsed); }

//         ApplyRotation(rotation);
//     }

//     private void ApplyRotation(float rotation)
//     {
//         if (rotation == 0.0)
//         {
//             StopBoidMovement();
//             rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Force);
//         }
//         else
//         {
//             float angle = Mathf.Clamp(rotation * Time.deltaTime, -filter, filter);
//             if (withDEBUG) { Debug.Log("FINAL VALUE OF ANGLE AFTER TIME " + angle); }
//             _myTransform.Rotate(Vector3.up, angle);
//             StopBoidMovement();
//             rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Force);
//         }

//         if (withDEBUG) { Debug.Log("//////////////////////////////////////////////////////////////////////////////////"); }
//     }

//     private void StopBoidMovement()
//     {
//         rb.angularVelocity = Vector3.zero;
//         rb.velocity = Vector3.zero;
//         rb.inertiaTensor = Vector3.zero;
//         rb.inertiaTensorRotation = Quaternion.identity;
//     }

//     private void LateUpdate()
//     {
//         IsGrounded();
//     }

//     private float AvoidWallRcast(float rotate, float wallRay)
//     {
//         Vector3 myPos = rb.transform.position;
//         List<Ray> rays = new List<Ray>
//         {
//             new Ray(myPos, transform.forward),
//             new Ray(myPos, transform.right),
//             new Ray(myPos, -transform.right),
//             new Ray(myPos, transform.forward + transform.right),
//             new Ray(myPos, transform.forward - transform.right)
//         };

//         float[] rotations = { 60f, 55f, -55f, 35f, -35f };
//         int layerWall = 8;
//         LayerMask layermask = 1 << layerWall;
//         float minDistance = wallRay;
//         int minIndex = GetMinDistanceRayIndex(rays, layermask, minDistance);

//         if (minIndex == -1) return rotate;
//         return rotations[minIndex];
//     }

//     private float CohesionBoidRcast(float rotate, float minRay, float maxRay)
//     {
//         Vector3 myPos = rb.transform.position;
//         List<Ray> rays = GetBoidRays(myPos);
//         int layerBoid = LayerMask.NameToLayer("BOID");
//         LayerMask layermask = 1 << layerBoid;

//         List<Vector3> allPosCollide = GetAllCollisions(rays, layermask, minRay, maxRay);
//         if (allPosCollide.Count == 0) return rotate;
//         return GetCohesionRotation(myPos, allPosCollide);
//     }

//     private float AvoidBoidRcast(float rotate, float avoidRay)
//     {
//         Vector3 myPos = rb.transform.position;
//         List<Ray> rays = GetBoidRays(myPos);
//         int layerBoid = 6;
//         LayerMask layermask = 1 << layerBoid;

//         Vector3 closestBuddy = GetClosestCollision(rays, layermask, avoidRay);
//         if (closestBuddy == Vector3.zero) return rotate;
//         return GetAvoidRotation(myPos, closestBuddy);
//     }

//     private float GoToBoidRcast(float rotate, float minRay, float maxRay)
//     {
//         Vector3 myPos = rb.transform.position;
//         List<Ray> rays = GetBoidRays(myPos);
//         int layerWall = 6;
//         LayerMask layermask = 1 << layerWall;

//         // Vector3 closestPoint = GetClosestCollision(rays, layermask, minRay, maxRay);
//         // if (closestPoint == Vector3.zero) return rotate;
//         // return GetGotoRotation(myPos, closestPoint);
//     }

//     private List<Ray> GetBoidRays(Vector3 myPos)
//     {
//         Vector3 forward = transform.forward;
//         Vector3 right = transform.right;
//         return new List<Ray>
//         {
//             new Ray(myPos, forward),
//             new Ray(myPos, right),
//             new Ray(myPos, -right),
//             new Ray(myPos, forward + right),
//             new Ray(myPos, forward - right),
//             new Ray(myPos, -forward + right),
//             new Ray(myPos, -forward - right)
//         };
//     }

//     private int GetMinDistanceRayIndex(List<Ray> rays, LayerMask layermask, float maxDistance)
//     {
//         float minDistance = float.MaxValue;
//         int minIndex = -1;
//         for (int i = 0; i < rays.Count; i++)
//         {
//             if (Physics.Raycast(rays[i], out RaycastHit hit, maxDistance, layermask))
//             {
//                 if (hit.distance < minDistance)
//                 {
//                     minDistance = hit.distance;
//                     minIndex = i;
//                 }
//             }
//         }
//         return minIndex;
//     }

//     private List<Vector3> GetAllCollisions(List<Ray> rays, LayerMask layermask, float minDistance, float maxDistance)
//     {
//         List<Vector3> collisions = new List<Vector3>();
//         foreach (var ray in rays)
//         {
//             RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance, layermask);
//             foreach (var hit in hits)
//             {
//                 if (hit.distance >= minDistance && hit.distance <= maxDistance)
//                 {
//                     collisions.Add(hit.point);
//                 }
//             }
//         }
//         return collisions;
//     }

//     private Vector3 GetClosestCollision(List<Ray> rays, LayerMask layermask, float maxDistance)
//     {
//         Vector3 closest = Vector3.zero;
//         float minDistance = maxDistance;
//         foreach (var ray in rays)
//         {
//             RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance, layermask);
//             foreach (var hit in hits)
//             {
//                 if (hit.distance < minDistance)
//                 {
//                     minDistance = hit.distance;
//                     closest = hit.point;
//                 }
//             }
//         }
//         return closest;
//     }

//     private float GetCohesionRotation(Vector3 myPos, List<Vector3> collisions)
//     {
//         Vector3 averagePos = Vector3.zero;
//         foreach (var pos in collisions)
//         {
//             averagePos += pos;
//         }
//         averagePos /= collisions.Count;
//         return GetAngleTowards(myPos, averagePos);
//     }

//     private float GetAvoidRotation(Vector3 myPos, Vector3 closestBuddy)
//     {
//         return -GetAngleTowards(myPos, closestBuddy);
//     }

//     private float GetGotoRotation(Vector3 myPos, Vector3 closestPoint)
//     {
//         return GetAngleTowards(myPos, closestPoint);
//     }

//     private float GetAngleTowards(Vector3 myPos, Vector3 targetPos)
//     {
//         Vector3 localTarget = rb.transform.InverseTransformPoint(targetPos);
//         return Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
//     }

//     private void IsGrounded()
//     {
//         Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);
//         foreach (var collider in colliders)
//         {
//             if (collider.gameObject != gameObject)
//             {
//                 grounded = true;
//                 return;
//             }
//         }
//         grounded = false;
//     }
// }

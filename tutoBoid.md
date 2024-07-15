# Tuto Boid

## Location

In the file **boidTuning.cs** located at _/Asset/FinalScene/Scripts/Boids_ you will find the boid's code.

## Boid Prefab

The boid prefab has three game Object :
* The boid game object that contains the box collider and the paper plane asset to render the boid. The **boidTuning.cs** component is in this prefab.
* The ground gestion game object where you will find the **CounterG2.cs** script that handle ground collision for the boid
* a Sphere visualizer that is used to see approximately the range of the behaviors.



## Organization of the code - BoidTuning.cs
The code is following this structure : 

1. Variables 

2. Boid main behavior

3. Behavior functions

4. Rotations functions

5. Collisions detections functions

6. Ground gestion functions

### 1. Variables
--------------------------

The private variables are :
* a RigidBody called _rb_ used in gravity gestion, force movement and collision positions.

* a List < Collider > _groundCollider_ that handle the collisions with the ground

* a boolean _grounded_ that indicate if the boid is on the ground or not.

* a transform _ _myTransform_ a cache version of the transform.

The public variables are :
* booleans _withGoto_, _withCohesion_, _withAvoid_ and _withDEBUG_ that allow boids to use the related behavior. Useful if you want to debug or test a specific behavior

* float _speed_, indicate boids movement speed. The value is high but reduced by Time.DeltaTime to smooth the movement.

* float _wallRay_, _avoidRay_, _cohesionRay_, _attractionRay_, that regulate the range of each behavior. if you are testing the boid in the WorkShop scene you can modify it directly in the inspector. If you want to change it in the real scene modify the same parameters in the **InitScene** game Object.

* float _filter_ to ensure that rotation angles stay in the range of the filter.

### 2. Boid main behavior
----

The main behavior follow this pattern :

![BoidMainBehaviour](/image/MainBehaviourBoid.png)

This induces the following priority on the behaviors :

1. Avoid wall behavior
2. Avoid boid behavior
3. Cohesion behavior
4. Attraction behavior


### 3. Behavior functions
----
Each behavior follows this structure : 

![BoidMainBehaviour](/image/BehaviourScheme.png)

Each behavior needs the value of the previous behavior rotation and the value of ray minimal distance and maximal distance if needed.

### 4. Rotations functions
----
The rotations functions use the detected information's to get the rotation angle value. The avoidance behaviors return a fixed value while attraction and cohesion need more computation. Cohesion for example compute the correction value needed. This value can be really high so the filter is needed here.

### 5. Collisions detections functions
----

Each collisions detection function verifies if the collided objects are in the range specified by ray's value.

### 6. Ground gestion functions
----

These functions are not used by the boid directly. There are used by the ground gestion child game object of the boid. the _isGrounded()_ method check if the boid is on the ground and change the value of the boolean _grounded_.

## Organization of the code - CounterG2.cs

this code contains just the Trigger functions to add a collider to the _groundCollider_ attribute of the boid. it uses the layer of the collided object to check if it needs to be added 
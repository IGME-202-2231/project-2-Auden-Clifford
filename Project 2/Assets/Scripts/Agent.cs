using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class  Agent : MonoBehaviour
{
    [SerializeField] protected PhysicsObject physics;
    [SerializeField] protected float maxForce;

    protected float wanderAngle = 0;
    Vector2 screenMax;

    // this is the target agent of this agent, regardless
    // of whether the agent is seeking or fleeing
    //[SerializeField] protected GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        screenMax = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
    }

    // Update is called once per frame
    protected void Update()
    {
        CalcSteeringForces();
    }

    protected abstract void CalcSteeringForces();

    /// <summary>
    /// Calculates the steering force required to achive a desired velocity toward a target position.
    /// </summary>
    /// <param name="targetPos">Position to seek</param>
    /// <returns>A force vector that will steer the agent towards a location</returns>
    protected Vector3 Seek(Vector3 targetPos)
    {
        // calculate a desired velocity which is the vector from the object to it's target scaled by the max speed
        Vector3 desiredVelocity = (targetPos - transform.position).normalized * physics.CruiseSpeed;

        // return the force vector required to achive the desired velocity
        return desiredVelocity - physics.Velocity;
    }

    /// <summary>
    /// Calculates the steering force required to achive a desired velocity away from a target position.
    /// </summary>
    /// <param name="targetPos">Position to flee from</param>
    /// <returns>A force vector that will steer the agent away from a location</returns>
    protected Vector3 Flee(Vector3 targetPos)
    {
        // calculate a desired velocity which is the vector from the target to this object scaled by the max speed
        Vector3 desiredVelocity = (transform.position - targetPos).normalized * physics.CruiseSpeed;

        // return the force vector required to achive the desired velocity
        return desiredVelocity - physics.Velocity;
    }

    protected Vector3 Wander(float wanderRadius, float wanderDistance)
    {
        // choose a distance ahead
        Vector3 projectionPos = transform.position + physics.Direction * wanderDistance;

        // add a random amount to the wander angle
        wanderAngle += Random.Range(-Mathf.PI/24, Mathf.PI / 24);

        //project a circle into the space ahead and get a target position on the circle's radius based on the random angle
        Vector3 targetPos = projectionPos + new Vector3(Mathf.Cos(wanderAngle) * wanderRadius, Mathf.Sin(wanderAngle) * wanderRadius);

        // seek the target position
        return Seek(targetPos);
    }

    protected Vector3 StayInBounds(float buffer, float time)
    {
        Vector3 desiredVelocity = Vector3.zero;

        // will future position be inside the right buffer?
        if (physics.GetFuturePosition(time).x > screenMax.x - buffer)
        {
            // add a velocity away from the closest point edge with twice the weight of a normal force devided by the distance to the boundry (force will have more weight as it gets closer)
            desiredVelocity += (transform.position - new Vector3(screenMax.x, transform.position.y)).normalized * physics.CruiseSpeed 
                / Vector3.Distance(transform.position, new Vector3(screenMax.x, transform.position.y));

        }
        // will future position be inside the top buffer?
        if (physics.GetFuturePosition(time).y > screenMax.y - buffer)
        {
            // add a velocity away from the closest point edge with twice the weight of a normal force devided by the distance to the boundry (force will have more weight as it gets closer)
            desiredVelocity += (transform.position - new Vector3(transform.position.x, screenMax.y)).normalized * physics.CruiseSpeed
                / Vector3.Distance(transform.position, new Vector3(transform.position.x, screenMax.y));
        }
        // will future position be inside the left buffer?
        if (physics.GetFuturePosition(time).x < -screenMax.x + buffer)
        {
            // add a velocity away from the closest point edge with twice the weight of a normal force devided by the distance to the boundry (force will have more weight as it gets closer)
            desiredVelocity += (transform.position - new Vector3(-screenMax.x, transform.position.y)).normalized * physics.CruiseSpeed
                / Vector3.Distance(transform.position, new Vector3(-screenMax.x, transform.position.y));
        }
        // will future position be inside the right buffer?
        if (physics.GetFuturePosition(time).y < -screenMax.y + buffer)
        {
            // add a velocity away from the closest point edge with twice the weight of a normal force devided by the distance to the boundry (force will have more weight as it gets closer)
            desiredVelocity += (transform.position - new Vector3(transform.position.x, -screenMax.y)).normalized * physics.CruiseSpeed
                / Vector3.Distance(transform.position, new Vector3(transform.position.x, -screenMax.y));
        }

        // if the bounds are not ncountered, return no force
        return desiredVelocity;
    }
    
}

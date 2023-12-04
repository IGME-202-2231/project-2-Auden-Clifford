using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public abstract class Agent : MonoBehaviour
{
    [SerializeField] protected PhysicsObject physics;
    [SerializeField] protected float maxForce;
    [SerializeField] protected float cruiseSpeed;
    [SerializeField] protected float separationDistance;

    [SerializeField] protected float avoidTime;

    // weights
    [SerializeField] protected float fleeWeight;
    [SerializeField] protected float seekWeight;
    [SerializeField] protected float separationWeight;
    [SerializeField] protected float avoidWeight;
    //[SerializeField] protected float persueWeight;

    protected PhysicsObject player;
    protected List<PhysicsObject> enemies;

    [SerializeField] private GameObject markerPrefab;
    private GameObject marker;

    [SerializeField] private int score;
    
    /// <summary>
    /// Gets this Agent's tracking marker
    /// </summary>
    public GameObject Marker { get { return marker; } }

    //protected float wanderAngle = 0;
    //Vector2 screenMax;

    // this is the target agent of this agent, regardless
    // of whether the agent is seeking or fleeing
    //[SerializeField] protected GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        marker = Instantiate(markerPrefab);

        // get a reference to the player
        player = GameManager.Instance.Player;

        // get a reference to all the enemies
        enemies = GameManager.Instance.Enemies;
        //screenMax = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
    }

    // Update is called once per frame
    protected void Update()
    {
        if(GameManager.Instance.currentState == GameState.Gameplay)
        {
            CalcSteeringForces();
        }
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
        Vector3 desiredVelocity = (targetPos - transform.position).normalized * cruiseSpeed;

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
        Vector3 desiredVelocity = (transform.position - targetPos).normalized * cruiseSpeed;

        // return the force vector required to achive the desired velocity
        return (desiredVelocity - physics.Velocity) * fleeWeight;
    }

    /// <summary>
    /// Calculates a steering force which keeps this agent separated from other given objects
    /// </summary>
    /// <param name="others">list of other objects to separate from</param>
    /// <returns>a separating steering force vector</returns>
    protected Vector3 Separate(List<PhysicsObject> others)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (PhysicsObject other in others)
        {
            // check for agents within the separation distance and ensure agent does not attempt to separate from itself
            if(other != this.physics && Vector3.Distance(other.Position, this.physics.Position) < separationDistance)
            {
                sum += Flee(other.Position) / Vector3.Distance(other.Position, this.physics.Position); // separation force is greater if they are closer
                count++;
            }
        }

        // ensure there was at least one separation attempt
        if(count > 0)
        {
            sum /= count;
        }

        return sum * separationWeight;
    }

    protected Vector3 AvoidObstacles()
    {
        Vector3 totalForce = Vector3.zero;

        List <Vector3> foundObstacles = new List<Vector3>();

        foreach (PhysicsObject obstacle in GameManager.Instance.Obstacles)
        {
            Vector3 agentToObstacle = obstacle.transform.position - transform.position;
            float rightDot = 0, forwardDot = 0;

            forwardDot = Vector3.Dot(physics.Direction, agentToObstacle);

            // if in front of me
            if (forwardDot >= -obstacle.Radius)
            {
                Vector3 futurePos = physics.GetFuturePosition(avoidTime);
                float dist = Vector3.Distance(transform.position, futurePos) + physics.Radius;

                // within box in front of me
                if (forwardDot <= dist + obstacle.Radius)
                {
                    rightDot = Vector3.Dot(transform.right, agentToObstacle);

                    Vector3 steeringForce = transform.right * (forwardDot / dist) * cruiseSpeed;

                    // is the obstacle witin the box width?
                    if (Mathf.Abs(rightDot) <= physics.Radius + separationDistance + obstacle.Radius)
                    {
                        foundObstacles.Add(obstacle.transform.position);

                        // if left steer right
                        if (rightDot < 0)
                        {
                            totalForce += steeringForce;
                        }
                        // if right steer left
                        else
                        {
                            totalForce += -steeringForce;
                        }
                    }
                }
            }
        }

        return totalForce * avoidWeight;
    }

    /*
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
    */

    private void OnDestroy()
    {
        // when an enemy is destroyed, it should be removed from the game manager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Enemies.Remove(physics);
            Destroy(marker);

            if (GameManager.Instance.currentState == GameState.Gameplay)
            {
                GameManager.Instance.Score += score;
            }
        }

        if (player != null)
        {
            // the player gains rotational velocity equal to half this enemy's score
            player.SpeedUpSpin(score / 2);
        }
    }

    /*
    protected Vector3 StayInBounds(float buffer, float time)
    {
        Vector3 desiredVelocity = Vector3.zero;

        // will future position be inside the right buffer?
        if (physics.GetFuturePosition(time).x > screenMax.x - buffer)
        {
            // add a velocity away from the closest point edge with twice the weight of a normal force devided by the distance to the boundry (force will have more weight as it gets closer)
            desiredVelocity += (transform.position - new Vector3(screenMax.x, transform.position.y)).normalized * cruiseSpeed 
                / Vector3.Distance(transform.position, new Vector3(screenMax.x, transform.position.y));

        }
        // will future position be inside the top buffer?
        if (physics.GetFuturePosition(time).y > screenMax.y - buffer)
        {
            // add a velocity away from the closest point edge with twice the weight of a normal force devided by the distance to the boundry (force will have more weight as it gets closer)
            desiredVelocity += (transform.position - new Vector3(transform.position.x, screenMax.y)).normalized * cruiseSpeed
                / Vector3.Distance(transform.position, new Vector3(transform.position.x, screenMax.y));
        }
        // will future position be inside the left buffer?
        if (physics.GetFuturePosition(time).x < -screenMax.x + buffer)
        {
            // add a velocity away from the closest point edge with twice the weight of a normal force devided by the distance to the boundry (force will have more weight as it gets closer)
            desiredVelocity += (transform.position - new Vector3(-screenMax.x, transform.position.y)).normalized * cruiseSpeed
                / Vector3.Distance(transform.position, new Vector3(-screenMax.x, transform.position.y));
        }
        // will future position be inside the right buffer?
        if (physics.GetFuturePosition(time).y < -screenMax.y + buffer)
        {
            // add a velocity away from the closest point edge with twice the weight of a normal force devided by the distance to the boundry (force will have more weight as it gets closer)
            desiredVelocity += (transform.position - new Vector3(transform.position.x, -screenMax.y)).normalized * cruiseSpeed
                / Vector3.Distance(transform.position, new Vector3(transform.position.x, -screenMax.y));
        }

        // if the bounds are not ncountered, return no force
        return desiredVelocity;
    }
    */

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, separationDistance);
    }
}

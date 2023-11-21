using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    [SerializeField] GameObject sprite;

    private Vector3 position;
    private Vector3 direction;
    private Vector3 velocity;
    private Vector3 acceleration;

    [SerializeField] private float angularVelocity;
    private float totalRotation = 0;

    private float damageTimer = 0;

    //[SerializeField] private float mass;
    [SerializeField] private float radius;
    [SerializeField] private bool useFriction;
    [SerializeField] private float coefFriction;
    //[SerializeField] private bool useGravity;
    //[SerializeField] private float gravStrength;
    //[SerializeField] private float cruiseSpeed;

    private List<PhysicsObject> collisions = new List<PhysicsObject>();

    //private Vector3 screenMax;

    /// <summary>
    /// Gets the radius of this object
    /// </summary>
    public float Radius { get { return radius; } }

    /// <summary>
    /// Gets the mass of the object, equal to the area of the object
    /// </summary>
    public float Mass
    {
        get { /*return mass;*/ return Mathf.PI * Mathf.Pow(radius, 2); }
    }

    /*
    /// <summary>
    /// Gets the max speed of this object
    /// </summary>
    public float CruiseSpeed { get { return CruiseSpeed; } }
    */

    /// <summary>
    /// Gets the current velocity of this object
    /// </summary>
    public Vector3 Velocity { get { return velocity; } }

    /// <summary>
    /// Gets the object's angular velocity
    /// </summary>
    public float AngularVelocity { get { return angularVelocity; } }

    /// <summary>
    /// Gets or sets the object's position
    /// </summary>
    public Vector3 Position { get { return position; } set { position = value; } }

    /// <summary>
    /// Gets the direction this object is moving
    /// </summary>
    public Vector3 Direction { get { return direction; } }

    /// <summary>
    /// Gets the list of objects this object is colliding with
    /// </summary>
    public List<PhysicsObject> Collisions { get { return collisions; } }

    // Start is called before the first frame update
    void Start()
    {
        //screenMax = new Vector3(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);

        // initialize position and add to collision manager
        position = transform.position;
        CollisionManager.Instance.PhysicsObjects.Add(this);
    }

    private void OnDestroy()
    {
        // if the collision manager still exists, remove this object from it's list upon destroy
        if(CollisionManager.Instance != null)
        {
            CollisionManager.Instance.PhysicsObjects.Remove(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // physics object should only update during the game state
        if (GameManager.Instance.currentState == GameState.Gameplay)
        {
            // apply all forces first
            if (useFriction)
            {
                ApplyFriction(coefFriction);
            }

            ResolveCollisions(collisions);

            /*
            if (useGravity)
            {
                ApplyGravity(new Vector3(0, gravStrength, 0));
            }
            */

            // calculate Velocity
            velocity += acceleration * Time.deltaTime;

            //velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

            //get direction from velocity
            direction = velocity.normalized;

            // calculate position
            position += velocity * Time.deltaTime;

            // calculate the rotation
            totalRotation += angularVelocity * Time.deltaTime;

            // validate position
            //Bounce();

            // update actual position to calculated position
            transform.position = position;

            // rotate the sprite
            sprite.transform.rotation = Quaternion.Euler(0, 0, totalRotation);

            // reset acceleration
            acceleration = Vector3.zero;

            // display the sprite red whenever damage is taken
            if (damageTimer > 0)
            {
                sprite.GetComponent<SpriteRenderer>().color = Color.red;
                damageTimer -= Time.deltaTime;
            }
            else
            {
                sprite.GetComponent<SpriteRenderer>().color = Color.white;
            }

            // if the spinner stops spinning, destroy it
            if (angularVelocity <= 0)
            {
                Destroy(gameObject);
            }

            // make the sprite look in the direction of movement
            //transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
    }

    /// <summary>
    /// Applies a force vector to this object factoring in the object's mass
    /// </summary>
    /// <param name="force">Force vector applied</param>
    public void ApplyForce(Vector3 force)
    {
        acceleration += force / this.Mass;
    }

    /// <summary>
    /// Applies a force in the opposite direction of the spinner's velocity
    /// </summary>
    /// <param name="coeff">determines the force of friction</param>
    void ApplyFriction(float coeff)
    {
        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * coeff;
        ApplyForce(friction);
    }

    /// <summary>
    /// Slows down the spinner's rotational velocity by some amount
    /// </summary>
    /// <param name="amount">Amount deducted from rotational velocity</param>
    public void SlowSpin(float amount)
    {
        angularVelocity -= amount;
        //sprite.GetComponent<SpriteRenderer>().color = Color.red;

        //when angular velocity slows down the "hurt" anumation should play
        damageTimer = 0.15f;
    }

    /// <summary>
    /// Speeds up the spinner's rotational velocity by some amount
    /// </summary>
    /// <param name="amount">Amount added to rotational velocity</param>
    public void SpeedUpSpin(float amount)
    {
        angularVelocity += amount;
        //sprite.GetComponent<SpriteRenderer>().color = Color.green;
    }

    /// <summary>
    /// Resolves all collisions detected this frame by (roughly) simulating physics collisions
    /// </summary>
    /// <param name="collisions">List of objects being collided with</param>
    private void ResolveCollisions(List<PhysicsObject> collisions)
    {
        foreach (PhysicsObject otherObject in collisions)
        {
            // only calculate physics based collisions for spinners, for projectiles do nothing
            //if (otherObject.Type == ObjectType.Spinner)
            //{
                //Gizmos.color = Color.red;

                // the point where this object contacted the other will be the difference between the objects' centers normalized and scaled to the radius of this object
                Vector3 myContactPoint = position + (otherObject.Position - position).normalized * radius;

                // the point where the other object contacted this one will be the difference between the objects' centers normalized and scaled to the radius of the other object
                Vector3 otherContactPoint = otherObject.Position + (position - otherObject.Position).normalized * otherObject.Radius;

                // ensure the objcts are still intersecting (the intersect may have been eliminated by the other object)
                // this object should only be the one to move out of the intersection if it is smaller
                if (Vector3.Distance(position, otherObject.Position) < radius + otherObject.Radius && this.Mass <= otherObject.Mass)
                {
                    // move this object to eliminate overlap between contact points 
                    position += otherContactPoint - myContactPoint;
                }

                // apply a force equal to the amount of force required to stop this object
                otherObject.ApplyForce((otherObject.Position - position).normalized * (velocity - otherObject.Velocity).magnitude * this.Mass);

                // get the tangent vector to the contact point (normalized)
                Vector3 differenceVector = otherObject.Position - position;
                Vector3 TangentVector = new Vector3(differenceVector.y, -differenceVector.x).normalized;

                // apply force to the other object equal to the angular momentum 
                otherObject.ApplyForce(-TangentVector * angularVelocity  * radius);

                // you do more damage to a spinner based on momentum
                otherObject.SlowSpin(velocity.magnitude * this.Mass / 20);
            //}
        }
    }

    /*
    void ApplyGravity(Vector3 force)
    {
        ApplyForce(force * mass);
    }
    */

    /*
    void Bounce()
    {
        if(position.x > screenMax.x)
        {
            velocity.x *= -1;
            position.x = screenMax.x;
        }
        if (position.y > screenMax.y)
        {
            velocity.y *= -1;
            position.y = screenMax.y;
        }
        if (position.x < -screenMax.x)
        {
            velocity.x *= -1;
            position.x = -screenMax.x;
        }
        if (position.y < -screenMax.y)
        {
            velocity.y *= -1;
            position.y = -screenMax.y;
        }
    }
    */

    /// <summary>
    /// Gets where this object will be at some point in the future (if the velocity is at maximum)
    /// </summary>
    /// <param name="time">point in the future to get position</param>
    /// <returns>future position of this object</returns>
    public Vector3 GetFuturePosition(float time)
    {
        return transform.position + velocity * time;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + velocity);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + acceleration);


    }
}

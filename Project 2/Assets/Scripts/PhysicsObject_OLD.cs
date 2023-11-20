using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PhysicsObject_OLD : MonoBehaviour
{
    /*
    [SerializeField] ObjectInfo objectInfo;
    [SerializeField] GameObject sprite;

    private Vector3 position;
    private Vector3 velocity;
    private Vector3 acceleration;

    [SerializeField] private float angularVelocity;
    private float totalRotation = 0;

    private float maxSpeed = 50;
    private float damageTimer = 0;

    /// <summary>
    /// Gets the object's velocity
    /// </summary>
    public Vector3 Velocity { get { return velocity; } }

    /// <summary>
    /// Gets the object's angular velocity
    /// </summary>
    public float AngularVelocity { get { return angularVelocity; } }

    // Start is called before the first frame update
    void Start()
    {
        // get the object's starting postion
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // physics object should only update during the game state
        if(GameManager.Instance.currentState == GameState.Gameplay)
        {
            ResolveCollisions(objectInfo.collisions);

            // apply friction from ground
            ApplyFriction();

            // calculate velocity for this frame
            velocity += acceleration * Time.deltaTime;

            // validate the speed
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

            // calculate the postion
            position += velocity * Time.deltaTime;

            // calculate the rotation
            totalRotation += angularVelocity * Time.deltaTime;

            // set the object's postion to calculated position
            transform.position = position;

            // rotate the sprite
            sprite.transform.rotation = Quaternion.Euler(0, 0, totalRotation);

            // zero out acceleration
            acceleration = Vector3.zero;

            // display the sprite red whenever damage is taken
            if(damageTimer > 0)
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
        }
    }

    /// <summary>
    /// Applies a force vector to this object factoring in the object's mass
    /// </summary>
    /// <param name="force">Force vector applied</param>
    public void ApplyForce(Vector3 force)
    {
        acceleration += force / objectInfo.Mass;
    }

    /// <summary>
    /// Slows down the spinner's rotational velocity by some amount
    /// </summary>
    /// <param name="amount">Amount deducted from rotational velocity</param>
    public void SlowSpin(float amount)
    {
        angularVelocity -= amount;
        sprite.GetComponent<SpriteRenderer>().color = Color.red;

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
        sprite.GetComponent<SpriteRenderer>().color = Color.green;
    }

    /// <summary>
    /// Applies a force in the opposite direction of the spinner's velocity
    /// </summary>
    private void ApplyFriction()
    {
        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * 5;
        ApplyForce(friction);
    }

    /// <summary>
    /// Resolves all collisions detected this frame by (roughly) simulating physics collisions
    /// </summary>
    /// <param name="collisions">List of objects being collided with</param>
    private void ResolveCollisions(List<ObjectInfo> collisions)
    {
        foreach(ObjectInfo otherObject in collisions)
        {
            // only calculate physics based collisions for spinners, for projectiles do nothing
            if(otherObject.Type == ObjectType.Spinner)
            {
                Gizmos.color = Color.red;

                // the point where this object contacted the other will be the difference between the objects' centers normalized and scaled to the radius of this object
                Vector3 myContactPoint = position + (otherObject.Position - position).normalized * objectInfo.Radius;

                // the point where the other object contacted this one will be the difference between the objects' centers normalized and scaled to the radius of the other object
                Vector3 otherContactPoint = otherObject.Position + (position - otherObject.Position).normalized * otherObject.Radius;

                // ensure the objcts are still intersecting (the intersect may have been eliminated by the other object)
                // this object should only be the one to move out of the intersection if it is smaller
                if (Vector3.Distance(position, otherObject.Position) < objectInfo.Radius + otherObject.Radius && objectInfo.Mass <= otherObject.Mass)
                {
                    // move this object to eliminate overlap between contact points 
                    position += otherContactPoint - myContactPoint;
                }

                // apply a force equal to the amount of force required to stop this object
                otherObject.physics.ApplyForce((otherObject.Position - position).normalized * (velocity - otherObject.physics.Velocity).magnitude * objectInfo.Mass);

                // get the tangent vector to the contact point (normalized)
                Vector3 differenceVector = otherObject.Position - position;
                Vector3 TangentVector = new Vector3(differenceVector.y, -differenceVector.x).normalized;

                // apply force to the other object equal to the angulr momentum 
                otherObject.physics.ApplyForce(-TangentVector * angularVelocity * objectInfo.Radius);

                // you do more damage to a spinner based on momentum
                otherObject.physics.SlowSpin(velocity.magnitude * objectInfo.Mass / 20);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, objectInfo.Radius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + velocity);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + acceleration);

        Gizmos.color = Color.white;
    }
    */
}

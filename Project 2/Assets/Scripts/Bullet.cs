using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    //[SerializeField] ObjectInfo objectInfo;
    private List<PhysicsObject> collisions = new List<PhysicsObject>();

    private Vector3 position = Vector3.zero;
    private Vector3 direction = Vector3.zero;
    //private Vector3 initialVelocity = Vector3.zero;
    [SerializeField] private float radius;

    [SerializeField] private float speed = 20;
    [SerializeField] private float damage;
    // bullets should be removed after some amount of time
    [SerializeField] private float lifespan = 5;

    /// <summary>
    /// Gets or sets the object that shot this bullet
    /// </summary>
    public PhysicsObject Originator { get; set; }

    /// <summary>
    /// Gets or sets the direction of movement (normalized)
    /// </summary>
    internal Vector3 Direction
    {
        get { return direction; }
        set
        {
            // direction should only have an x and y value, no z value
            direction = new Vector2(value.x, value.y).normalized;
        }
    }

    /// <summary>
    /// Gets the radius of the bullet
    /// </summary>
    public float Radius { get { return radius; } }

    /// <summary>
    /// Gets the bullet's position
    /// </summary>
    public Vector3 Position { get { return position; } }

    /// <summary>
    /// Equivalent to the velocity of the originator at the time the bullet was fired
    /// </summary>
    //public Vector3 InitialVelocity { get { return initialVelocity; } set { initialVelocity = value; } }

    /// <summary>
    /// Gets the list of objects colliding with this bullet
    /// </summary>
    public List<PhysicsObject> Collisions { get { return collisions; } }

    // Start is called before the first frame update
    void Start()
    {
        // Grab the GameObject's starting position
        position = transform.position;
        
        // add to the collision manager
        CollisionManager.Instance.Bullets.Add(this);
    }
    
    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.currentState == GameState.Gameplay)
        {
            ResolveCollisions(collisions);

            // add caclulated velocity to position
            position += (direction * speed /*+ initialVelocity*/) * Time.deltaTime;

            // update actual position to calculated position
            transform.position = position;

            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

            // count down the seconds since the object was created
            lifespan -= Time.deltaTime;

            // remove the object when it's lifespan ends
            if (lifespan < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Resolves all collisions dretected this frame; ignores the originator and other bullets and dissapears when it hits something else
    /// </summary>
    /// <param name="collisions">A list containing all the collisions detected this frame</param>
    private void ResolveCollisions(List<PhysicsObject> collisions)
    {
        foreach(PhysicsObject physicsObject in collisions)
        {
            if(physicsObject != Originator /*&& otherObject.Type != ObjectType.Projectile*/)
            {
                //Gizmos.color = Color.red;

                physicsObject.SlowSpin(damage);

                // bullets should be destroyed when they reach a target
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + direction);

        Gizmos.color = Color.white;
    }
    
}

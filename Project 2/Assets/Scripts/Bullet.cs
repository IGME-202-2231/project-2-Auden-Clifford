using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] ObjectInfo objectInfo;

    private Vector3 position = Vector3.zero;
    [SerializeField] private Vector3 direction = Vector3.zero;

    private float speed = 20;
    [SerializeField] private float damage;

    // bullets should be removed after some amount of time
    [SerializeField] private float lifespan = 5;

    /// <summary>
    /// Gets or sets the object that shot this bullet
    /// </summary>
    public ObjectInfo Originator { get; set; }

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

    // Start is called before the first frame update
    void Start()
    {
        // Grab the GameObject's starting position
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.currentState == GameState.Gameplay)
        {
            ResolveCollisions(objectInfo.collisions);

            // add caclulated velocity to position
            position += direction * speed * Time.deltaTime;

            // update actual position to calculated position
            transform.position = position;

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
    private void ResolveCollisions(List<ObjectInfo> collisions)
    {
        foreach(ObjectInfo otherObject in collisions)
        {
            if(otherObject != Originator && otherObject.Type != ObjectType.Projectile)
            {
                Gizmos.color = Color.red;

                otherObject.physics.SlowSpin(damage);

                // bullets should be destroyed when the reach a target
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, objectInfo.Radius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + direction);

        Gizmos.color = Color.white;
    }
}

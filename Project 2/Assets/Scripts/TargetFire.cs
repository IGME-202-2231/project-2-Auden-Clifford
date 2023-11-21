using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFire : MonoBehaviour
{
    [SerializeField] private GameObject ammunition;
    [SerializeField] private GameObject barrelSprite;
    [SerializeField] private PhysicsObject physics;

    private Vector3 direction;

    /// <summary>
    /// Gets or sets the aim direction
    /// </summary>
    internal Vector3 Direction 
    { 
        get { return direction; }
        set { direction = value.normalized; }
    }

    // Update is called once per frame
    void Update()
    {
        // animate the gun barrel
        barrelSprite.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }

    /// <summary>
    /// Fires one bullet each time the function is called
    /// </summary>
    internal void Fire()
    {
        GameObject bullet = Instantiate(ammunition, transform.position, Quaternion.identity);

        bullet.GetComponent<Bullet>().Direction = direction;
        bullet.GetComponent<Bullet>().InitialVelocity = physics.Velocity;

        //assign this bullet's originator to this object
        bullet.GetComponent<Bullet>().Originator = this.GetComponent<PhysicsObject>();
    }
}

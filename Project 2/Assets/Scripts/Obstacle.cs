using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private PhysicsObject physics;

    /// <summary>
    /// Gets the damage done by this obstacle
    /// </summary>
    public float Damage { get { return damage; } }

    /// <summary>
    /// Gets the radius of the obstacle
    /// </summary>
    public float Radius { get { return physics.Radius; } }
}

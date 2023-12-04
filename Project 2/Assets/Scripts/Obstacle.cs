using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Timeline;

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

    private void OnDestroy()
    {
        // when an enemy is destroyed, it should be removed from the game manager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Obstacles.Remove(physics);
            //Destroy(marker);
        }
    }
}

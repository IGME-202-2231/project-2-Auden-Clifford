using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : Singleton<CollisionManager>
{
    private List<PhysicsObject> physicsObjects = new List<PhysicsObject>();
    private List<Bullet> bullets = new List<Bullet>();

    /// <summary>
    /// Gets a reference to the list of collidable game objects
    /// </summary>
    public List<PhysicsObject> PhysicsObjects
    {
        get { return physicsObjects; }
    }

    /// <summary>
    /// Gets a reference to the list of bullets
    /// </summary>
    public List<Bullet> Bullets { get { return bullets; } }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.currentState == GameState.Gameplay)
        {
            // loop through each object
            foreach (PhysicsObject collidable in physicsObjects)
            {
                // before calculating new collisions, clear the old ones
                collidable.Collisions.Clear();

                // check each object against each other object
                foreach (PhysicsObject otherCollidable in physicsObjects)
                {
                    // make sure objects are not checked against themselves
                    if (collidable != otherCollidable)
                    {
                        if (Vector2.Distance(collidable.Position, otherCollidable.Position) <= collidable.Radius + otherCollidable.Radius)
                        {
                            // if they're colliding, add the collision to the object's collisions list
                            collidable.Collisions.Add(otherCollidable);
                        }
                    }
                }

                // also check each object against bullets
                foreach (Bullet bullet in bullets)
                {
                    if (Vector2.Distance(collidable.Position, bullet.Position) <= collidable.Radius + bullet.Radius)
                    {
                        // if they're colliding, add the collision to the object's collisions list
                        bullet.Collisions.Add(collidable);
                    }
                }
            }
        }
    }
}

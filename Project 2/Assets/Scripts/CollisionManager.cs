using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : Singleton<CollisionManager>
{
    private List<ObjectInfo> gameObjects = new List<ObjectInfo>();

    /// <summary>
    /// Gets a reference to the list of collidable game objects
    /// </summary>
    public List<ObjectInfo> GameObjects
    {
        get { return gameObjects; }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.currentState == GameState.Gameplay)
        {
            // loop through each item
            foreach (ObjectInfo collidable in gameObjects)
            {
                // before calculating new collisions, clear the old ones
                collidable.collisions.Clear();

                // check each item against each other item
                foreach (ObjectInfo otherCollidable in gameObjects)
                {
                    // make sure objects are not checked against themselves
                    if (collidable != otherCollidable)
                    {
                        if (Vector2.Distance(collidable.Position, otherCollidable.Position) <= collidable.Radius + otherCollidable.Radius)
                        {
                            // if they're colliding, add the collision to the object's collisions list
                            collidable.collisions.Add(otherCollidable);
                        }
                    }
                }
            }
        }
    }
}

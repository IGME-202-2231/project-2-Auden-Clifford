using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StandardEnemyState
{
    Fight,
    Heal,
    Frenzy
}

public class StandardEnemy : Agent
{
    private StandardEnemyState currentState = StandardEnemyState.Fight;

    protected override void CalcSteeringForces()
    {
        Vector3 cumulativeForce;

        switch (currentState)
        {
            case StandardEnemyState.Fight:
                frenzyDecal.SetActive(false);

                cumulativeForce = Vector3.zero;
                cumulativeForce += Seek(player.Position) * seekWeight;
                cumulativeForce += Separate(GameManager.Instance.Enemies) * separationWeight;
                cumulativeForce += AvoidObstacles() * avoidWeight;

                cumulativeForce = Vector3.ClampMagnitude(cumulativeForce, maxForce);

                physics.ApplyForce(cumulativeForce);

                // transition to ANGER state
                if(physics.AngularVelocity < frenzyHealth)
                {
                    currentState = StandardEnemyState.Frenzy;
                }
                break; 

            case StandardEnemyState.Heal: 
                // TODO: make HEALING ITEM until then, go directly to ANGER state
                break; 

            case StandardEnemyState.Frenzy:
                frenzyDecal.SetActive(true);
                cumulativeForce = Vector3.zero;
                cumulativeForce += Seek(player.Position) * seekWeight * 2; // seek extra hard

                // no separation while ANGRY
                physics.ApplyForce(cumulativeForce);
                break;
        }
    }

    /*
    // Start is called before the first frame update
    void Start()
    {

    }
    */

    /*
    // Update is called once per frame
    void Update()
    {
        
    }
    */
}

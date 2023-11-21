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
        switch (currentState)
        {
            case StandardEnemyState.Fight:
                Vector3 cummulativeForce = Vector3.zero;
                cummulativeForce += Seek(player.Position);

                physics.ApplyForce(cummulativeForce);
                //print("I did an agent thing!");
                break; 
            case StandardEnemyState.Heal: 
                break; 
            case StandardEnemyState.Frenzy: 
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

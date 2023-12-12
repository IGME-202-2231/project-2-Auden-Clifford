using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FastEnemyState
{
    Fight,
    Frenzy
}

public class FastEnemy : Agent
{

    private FastEnemyState currentState = FastEnemyState.Fight;

    protected override void CalcSteeringForces()
    {
        Vector3 cumulativeForce;

        switch (currentState)
        {
            case FastEnemyState.Fight:
                frenzyDecal.SetActive(false);

                cumulativeForce = Vector3.zero;
                cumulativeForce += Pursue(player) * pursueWeight;
                cumulativeForce += Separate(GameManager.Instance.Enemies) * separationWeight;
                cumulativeForce += AvoidObstacles() * avoidWeight;

                cumulativeForce = Vector3.ClampMagnitude(cumulativeForce, maxForce);

                physics.ApplyForce(cumulativeForce);

                // transition to ANGER state
                if (physics.AngularVelocity < frenzyHealth)
                {
                    currentState = FastEnemyState.Frenzy;
                }
                break;

            case FastEnemyState.Frenzy:
                frenzyDecal.SetActive(true);
                cumulativeForce = Vector3.zero;
                cumulativeForce += Pursue(player) * pursueWeight * 2; // pursue extra hard

                // no separation or avoidance while ANGRY

                physics.ApplyForce(cumulativeForce);
                break;
        }
    }
}

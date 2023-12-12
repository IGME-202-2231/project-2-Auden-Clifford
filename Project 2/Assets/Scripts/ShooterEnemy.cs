using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShooterEnemyStates
{
    Seek,
    Shoot,
    Frenzy
}

public class ShooterEnemy : Agent
{
    [SerializeField] RadialFire weapon;

    [SerializeField] float shootDelay;

    private ShooterEnemyStates currentState = ShooterEnemyStates.Seek;

    protected override void CalcSteeringForces()
    {
        Vector3 cumulativeForce;

        switch (currentState)
        {
            case ShooterEnemyStates.Seek:
                frenzyDecal.SetActive(false);
                cumulativeForce = Vector3.zero;
                cumulativeForce += Seek(player.Position) * seekWeight;
                cumulativeForce += Separate(GameManager.Instance.Enemies) * separationWeight;
                cumulativeForce += AvoidObstacles() * avoidWeight;

                cumulativeForce = Vector3.ClampMagnitude(cumulativeForce, maxForce);

                physics.ApplyForce(cumulativeForce);

                // if low health transition to ANGER
                if(physics.AngularVelocity < frenzyHealth)
                {
                    currentState = ShooterEnemyStates.Frenzy;
                }

                // if the player enters the separation radius, transition to SHOOT
                if (Vector3.Distance(player.Position, this.physics.Position) < separationDistance)
                {
                    currentState = ShooterEnemyStates.Shoot;
                }
                break;

            case ShooterEnemyStates.Shoot:
                frenzyDecal.SetActive(false);
                cumulativeForce = Vector3.zero;
                cumulativeForce += Separate(GameManager.Instance.Enemies) * separationWeight;
                /*cumulativeForce += Separate(new List<PhysicsObject> { player }) * 50;*/ // separate extra hard from the player
                cumulativeForce += Flee(player.Position) * fleeWeight;
                cumulativeForce += AvoidObstacles() * avoidWeight;

                cumulativeForce = Vector3.ClampMagnitude(cumulativeForce, maxForce);

                physics.ApplyForce(cumulativeForce);

                // fire weapon
                weapon.Fire(shootDelay);

                // if low health, transition to  ANGER
                if(physics.AngularVelocity < 100)
                {
                    currentState = ShooterEnemyStates.Frenzy;
                }

                // if the player exits the separation radius (minus the buffer zone buffer zone), transition to SEEK
                if(Vector3.Distance(player.Position, this.physics.Position) > separationDistance - 2)
                {
                    currentState = ShooterEnemyStates.Seek;
                }
                break;

            case ShooterEnemyStates.Frenzy:
                frenzyDecal.SetActive(true);
                cumulativeForce = Vector3.zero;
                cumulativeForce += Seek(player.Position) * seekWeight;

                cumulativeForce = Vector3.ClampMagnitude(cumulativeForce, maxForce);

                physics.ApplyForce(cumulativeForce);

                // fire weapon
                weapon.Fire(shootDelay / 2);
                break;
        }

        print(currentState);
    }
    /*
// Start is called before the first frame update
void Start()
{

}

// Update is called once per frame
void Update()
{

}
*/
}

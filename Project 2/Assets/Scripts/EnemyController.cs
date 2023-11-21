using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    /*
    [SerializeField] float accelSpeed;
    GameObject player;
    [SerializeField] PhysicsObject physicsMovement;
    [SerializeField] int score;
    [SerializeField] RadialFire weapon;
    [SerializeField] float detectionRadius;
    [SerializeField] private GameObject markerPrefab;

    private GameObject marker;

    /// <summary>
    /// Gets this enemy's marker object
    /// </summary>
    public GameObject Marker { get { return marker; } }

    // Start is called before the first frame update
    void Start()
    {
        marker = Instantiate(markerPrefab);

        // get a reference to the player
        player = GameManager.Instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.currentState == GameState.Gameplay)
        {
            physicsMovement.ApplyForce((player.transform.position - transform.position).normalized * accelSpeed);

            // only shoot if the player is within the detection radius
            if (Vector3.Distance(player.transform.position, transform.position) < detectionRadius)
            {
                // if this enemy has the ability to fire a weapon, do so
                if (weapon != null)
                {
                    weapon.Fire();
                }
            }
        }
    }

    private void OnDestroy()
    {
        // when an enemy is destroyed, it should be removed from the game manager
        if(GameManager.Instance != null)
        {
            GameManager.Instance.Enemies.Remove(gameObject);
            Destroy(marker);

            if(GameManager.Instance.currentState == GameState.Gameplay)
            {
                GameManager.Instance.Score += score;
            }
        }

        if(player != null) 
        {
            // the player gains rotational velocity equal to half this enemy's score
            player.GetComponent<PhysicsObject>().SpeedUpSpin(score / 2);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    */
}

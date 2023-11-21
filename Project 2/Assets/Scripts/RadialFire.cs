using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialFire : MonoBehaviour
{
    [SerializeField] GameObject ammunition;

    // set up a timer and a default value for that timer
    [SerializeField] private float fireTime = 2; // 2 sec shoot delay
    private float fireTimer;

    // Start is called before the first frame update
    void Start()
    {
        fireTimer = fireTime;
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Fires a spread of 8 bullets out in a circle on a 2 second delay
    /// </summary>
    internal void Fire()
    {
        if(fireTimer < 0)
        {
            // shoot 8 bulets out in a circle
            for (int i = 0; i < 8; i++)
            {
                GameObject bullet = Instantiate(ammunition, transform.position, Quaternion.identity);

                bullet.GetComponent<Bullet>().Direction = new Vector3(Mathf.Cos((Mathf.PI * 2 / 8) * i), Mathf.Sin((Mathf.PI * 2 / 8) * i), 0);

                bullet.GetComponent<Bullet>().Originator = this.GetComponent<PhysicsObject>();
            }

            // restart the timer
            fireTimer = fireTime;
        }
    }
}

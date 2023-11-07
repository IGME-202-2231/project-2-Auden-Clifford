using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // get a reference to the player if you do not have one already
        if(player == null)
        {
            player = GameManager.Instance.Player;
        }

        if(GameManager.Instance.currentState == GameState.Gameplay)
        {
            // update ONLY the camera's X and Y to the player's position
            this.transform.position = new Vector3(
                    player.transform.position.x,
                    player.transform.position.y,
                    this.transform.position.z); // preserve the z value
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
     private GameObject player;

    // this (should) be the accurate height and width of the background panels
    private Vector2 backgroundSize = new Vector2(24.8f, 15.5f);

    // Update is called once per frame
    void Update()
    {
        //get a reference to the player if you do not have one already
        if(player == null)
        {
            player = GameManager.Instance.Player;
        }

        if(GameManager.Instance.currentState == GameState.Gameplay)
        {
            // check if the player has gone beyond the right edge
            if (player.transform.position.x >= this.transform.position.x + (backgroundSize.x / 2))
            {
                // move the background right by 1 background panel width
                this.transform.position = new Vector2(
                    this.transform.position.x + backgroundSize.x,
                    this.transform.position.y); // background y value is preserved
            }

            // check if the player has gone beyond the left edge
            if (player.transform.position.x <= this.transform.position.x - (backgroundSize.x / 2))
            {
                // move the background left by 1 background panel width
                this.transform.position = new Vector2(
                    this.transform.position.x - backgroundSize.x,
                    this.transform.position.y); // background y value is preserved
            }

            // check if the player has gone above the top edge
            if (player.transform.position.y >= this.transform.position.y + (backgroundSize.y / 2))
            {
                // move the background up by 1 background panel width
                this.transform.position = new Vector2(
                    this.transform.position.x, // background x is preserved
                    this.transform.position.y + backgroundSize.y);
            }

            // check if the player has gone below the bottom edge
            if (player.transform.position.y <= this.transform.position.y - (backgroundSize.y / 2))
            {
                // move the background down by 1 background panel width
                this.transform.position = new Vector2(
                    this.transform.position.x, // background x is preserved
                    this.transform.position.y - backgroundSize.y);
            }
        }
    }
}

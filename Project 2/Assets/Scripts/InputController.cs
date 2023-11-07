using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] private PhysicsObject playerControlledObject;
    [SerializeField] private float accelSpeed;
    [SerializeField] private TargetFire playerWeapon;

    private Vector2 direction;

    // The method that gets called to handle any player movement input
    /// <summary>
    /// Updates the direction of the player's movement when the directional inputs change
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Fires a bullet whn the player performs the fire action
    /// </summary>
    /// <param name="context"></param>
    public void OnFire(InputAction.CallbackContext context)
    {
        // only fire when the button is first pressed
        if (context.started)
        {
            playerWeapon.Fire();
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.currentState == GameState.Gameplay)
        {
            playerControlledObject.ApplyForce(direction * accelSpeed);

            playerWeapon.Direction = new Vector3(
                (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position).x, 
                (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position).y, 0);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            // when the player is destroyed (they die) set the game state to GameOver\
            GameManager.Instance.GameOver();
        }
    }
}

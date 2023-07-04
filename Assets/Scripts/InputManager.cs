using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private PlayerControls playerControls;

    private void Awake() {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    public Vector2 GetMovementVector() {
        Vector2 inputvector = playerControls.Player.Movement.ReadValue<Vector2>();
        inputvector.Normalize();
        return inputvector;
    }
}

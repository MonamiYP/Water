using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputManager inputManager;

    private bool isUnderwater;

    private void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        Vector2 inputVector = inputManager.GetMovementVector();

        float moveAmount = moveSpeed * Time.deltaTime;

        if (!isUnderwater)
            transform.position += new Vector3(inputVector.x, 0, 0) * moveAmount;
    }
}
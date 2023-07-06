using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpAmount = 5f;
    [SerializeField] private float gravityWater = 0f;
    [SerializeField] private float gravityNormal = 2f;
    [SerializeField] private float gravityFalling = 4f;

    [SerializeField] private InputManager inputManager;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] public bool isUnderwater = false;

    private void Start() {
        inputManager.OnJump += InputManager_OnJump;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        Vector2 inputVector = inputManager.GetMovementVector();

        if (isUnderwater) {
            rb.velocity = inputVector * moveSpeed;
        } else {
            rb.velocity = new Vector2(inputVector.x * moveSpeed, rb.velocity.y); 
        }
        
        HandleGravity();
    }

    private void InputManager_OnJump(object sender, System.EventArgs e) {
        if (!isUnderwater && rb.velocity.y == 0) {
            rb.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
        }
    }

    private void HandleGravity() {
        if (rb.velocity.y >= 0) {
            rb.gravityScale = gravityNormal;
        } else {
            rb.gravityScale = gravityFalling;
        }

        if (isUnderwater)
            rb.gravityScale = gravityWater;
    }
}
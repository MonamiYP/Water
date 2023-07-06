using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class WaterSpring : MonoBehaviour {

    public float velocity = 0f;
    public float force = 0f;
    public float height = 0f;
    public float targetHeight = 0f;
    public int waveIndex = 0;

    [SerializeField] private static SpriteShapeController spriteShapeController = null;
    private float resistance = 40f;

    public void Init(SpriteShapeController ssc) { 

        var index = transform.GetSiblingIndex();
        waveIndex = index+1;
        
        spriteShapeController = ssc;
        velocity = 0;
        height = transform.localPosition.y;
        targetHeight = transform.localPosition.y;
    }

    public void WaveSpringUpdate(float springStiffness, float dampening) {
        float extension = transform.localPosition.y - targetHeight;
        float loss = dampening * velocity;
        force = -springStiffness * extension - loss;
        velocity += force;

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + velocity, transform.localPosition.z);
    }

    public void WavePointUpdate() { 
        if (spriteShapeController != null) {
            Spline waterSpline = spriteShapeController.spline;
            Vector3 wavePosition = waterSpline.GetPosition(waveIndex);
            waterSpline.SetPosition(waveIndex, new Vector3(wavePosition.x, transform.localPosition.y, wavePosition.z));
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag.Equals("Player")) {
            PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
            Rigidbody2D rb = playerManager.GetComponent<Rigidbody2D>();
            var speed = rb.velocity;

            velocity += speed.y/resistance;
        }
    }
}

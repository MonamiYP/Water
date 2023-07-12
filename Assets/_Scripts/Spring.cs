using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Spring : MonoBehaviour {

    public float velocity;
    public float height;

    private float force;
    private float targetHeight;
    private int index;
    private float waveAmplitude;

    [SerializeField] private static SpriteShapeController spriteShapeController;
    [SerializeField] private float resistance = 40f;

    public void Init(SpriteShapeController _spriteShapeController) { 

        index = transform.GetSiblingIndex() + 1;
        
        spriteShapeController = _spriteShapeController;
        velocity = 0;
        height = transform.localPosition.y;
        targetHeight = transform.localPosition.y;
    }

    public void SpringUpdate(float k, float dampingRatio) {

        float extension = height - targetHeight;
        force = -k * extension - dampingRatio * velocity;
        velocity += force;
        height += velocity;
    }

    public void SplineUpdate() { 
        if (spriteShapeController != null) {
            Spline spline = spriteShapeController.spline;
            Vector3 splinePosition = spline.GetPosition(index);
            spline.SetPosition(index, new Vector3(splinePosition.x, transform.localPosition.y, splinePosition.z));
        }
    }

    public void SineWave() {
        waveAmplitude = 0.05f * Mathf.Sin(Time.time + index/2);
    }

    public void PositionUpdate() {
        transform.localPosition = new Vector3(transform.localPosition.x, height + waveAmplitude, transform.localPosition.z);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag.Equals("Player")) {
            PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
            Rigidbody2D rb = playerManager.GetComponent<Rigidbody2D>();

            velocity += rb.velocity.y / resistance;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public abstract class HealthBehaviour : MonoBehaviour {

    [Header("Settings")]
    public float maxHealth;
    [Header("References")]
    public Slider healthSlider;

    private bool isDead = false;
    private float health;
    private Transform canvas;
    private Transform cam;

    protected virtual void Awake() {
        health = maxHealth;
        healthSlider.minValue = 0f;
        healthSlider.maxValue = maxHealth;

        cam = Camera.main.transform;
        canvas = healthSlider.transform.parent;
    }

    protected virtual void Update() {
        if (health < maxHealth) {
            canvas.gameObject.SetActive(true);
            healthSlider.value = health;
            canvas.LookAt(cam);
        } else {
            canvas.gameObject.SetActive(false);
        }

        if (health <= 0 && !isDead) {
            Death();
            return;
        }
    }

    public virtual void Damage (float damage) {
        health -= damage;
    }

    protected virtual void Death() {
        isDead = true;
    }
}

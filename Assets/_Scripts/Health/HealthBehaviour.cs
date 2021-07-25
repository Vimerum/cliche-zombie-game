using UnityEngine;
using UnityEngine.UI;

public abstract class HealthBehaviour : MonoBehaviour {

    [Header("Settings")]
    public float maxHealth;
    [Header("References")]
    public Slider healthSlider;

    protected bool isDead = false;
    protected float health;
    protected float timeout = -1f;
    protected Transform canvas;
    protected Transform cam;

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

        if (timeout >= 0f) {
            timeout -= Time.deltaTime;
        }
    }

    public virtual void Damage (float damage) {
        if (timeout <= 0f) {
            health -= damage;
        }
    }

    protected virtual void Death() {
        isDead = true;
    }
}

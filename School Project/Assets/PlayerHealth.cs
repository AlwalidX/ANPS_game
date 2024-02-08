using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public Slider healthSlider;

    void Start()
    {
        currentHealth = maxHealth;

        // Make sure you have a Slider component attached to your player GameObject
        if (healthSlider == null)
        {
            Debug.LogError("Health Slider is not assigned to the script!");
        }
        else
        {
            // Set the maximum value of the slider to the maximum health
            healthSlider.maxValue = maxHealth;
            // Set the initial value of the slider to the current health
            healthSlider.value = currentHealth;
        }
    }

    void Update()
    {
        // For testing purposes, you can use the spacebar to simulate taking damage
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10f);
        }
    }

    // Function to handle taking damage
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        // Make sure health doesn't go below 0
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        // Update the slider value
        healthSlider.value = currentHealth;

        // Check if the player is dead
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // Function to handle the player's death
    void Die()
    {
        // For now, let's just destroy the player GameObject
        Destroy(gameObject);
    }
}

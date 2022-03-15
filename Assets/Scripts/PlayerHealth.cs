using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    /// <summary>
    /// Quantidade maxima de vida
    /// </summary>
    public float fullHealth;

    /// <summary>
    /// Quantidade de vida atual
    /// </summary>
    float currentHealth;
    
    public string endText = "Ganhou!";
    public Image deathScreen;

    // Tocar quando tomar dano
    public AudioClip damageAudio;

    AudioSource playerAudioSource;

    // Feedback visual de dano
    public Image damageIndicator;
    Color flashColor = new Color(255f, 255f, 255f, 0.5f);
    float indicatorSpeed = 5f;

    bool damaged;

    public Text bloodText;


    public CanvasGroup endCanvasGroup;
    public Text endGameUiText;

    void Start()
    {
        currentHealth = fullHealth;
        playerAudioSource = GetComponent<AudioSource>();

        bloodText.text = currentHealth.ToString();
    }

    void Update()
    {
        if (damaged)
        {
            damageIndicator.color = flashColor;
        }
        else
        {
            damageIndicator.color = Color.Lerp(damageIndicator.color, Color.clear, indicatorSpeed * Time.deltaTime);
        }

        damaged = false;
    }

    public void Die()
    {
        endText = "Morreu :(";
        EndGame();
        deathScreen.color = Color.white;
        // Mata o personagem
        Destroy(gameObject);
    }

    public void EndGame()
    {
        endGameUiText.text = endText;
        endCanvasGroup.alpha = 1;
        print(endText);
    }

    public void AddDamage(float damage)
    {
        // Se quisermos deixar o obstaculo somente como um bounce boost
        if(damage <= 0)
        {
            return;
        }

        currentHealth -= damage;
        playerAudioSource.PlayOneShot(damageAudio);
        damaged = true;
        bloodText.text = currentHealth.ToString();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
}

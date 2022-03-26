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

    public Animator animator;

    bool isPaused = false;

    void Start()
    {
        endText = "Start";
        endGameUiText.text = endText;
        endCanvasGroup.alpha = 1;
        Time.timeScale = 0;
        isPaused = !isPaused;

        currentHealth = fullHealth;
        playerAudioSource = GetComponent<AudioSource>();
        bloodText.text = currentHealth.ToString();
    }

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        if (!isPaused)
        {
            endText = "Paused";
            endGameUiText.text = endText;
            endCanvasGroup.alpha = 1;
            isPaused = !isPaused;
            Time.timeScale = 0;
        }
        else
        {
            endCanvasGroup.alpha = 0;
            isPaused = !isPaused;
            Time.timeScale = 1;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Pause();
        }

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
        Destroy(gameObject);
        print(endText);
    }

    public void AddDamage(float damage)
    {
        // Se quisermos deixar o obstaculo somente como um bounce boost
        if(damage <= 0)
        {
            return;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Block"))
        {
            damage -= 5;
        }

        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        playerAudioSource.PlayOneShot(damageAudio);
        damaged = true;
        bloodText.text = currentHealth.ToString();

        if (currentHealth <= 0)
        {
            animator.SetTrigger("Death");
            Die();
        }
    }
}

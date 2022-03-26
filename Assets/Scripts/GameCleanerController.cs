using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCleanerController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            // Mata o personagem, caso ele passe pelo colisor
            collision.GetComponent<PlayerHealth>().Die();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main");
        Time.timeScale = 1;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        FindObjectOfType<PlayerHealth>().Pause();
    }

    public void Stop()
    {
        print("Voce tentou sair");
        // Nao funciona no editor, somente se gerasse um executavel
        Application.Quit();
    }


}

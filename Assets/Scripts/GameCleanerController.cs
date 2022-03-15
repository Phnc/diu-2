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
    }

    public void Stop()
    {
        print("Voce tentou sair");
        // Nao funciona no editor, somente se gerasse um executavel
        Application.Quit();
    }


}

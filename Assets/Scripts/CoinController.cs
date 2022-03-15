using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public GameObject reward;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Checa se o trigger aconteceu por causa do jogador
        if(collision.tag == "Player")
        {
            Instantiate(reward, transform.position, Quaternion.identity);
            // Destroi todo o objeto, incluindo o parent
            Destroy(gameObject.transform.root.gameObject);
            // Adiciona a moeda ao personagem
            collision.gameObject.GetComponent<PlayerInventory>().AddCoin();
            
        }
    }
}

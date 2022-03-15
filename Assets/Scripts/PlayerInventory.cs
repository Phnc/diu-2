using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    int CoinsQuantity = 0;

    public Text coinCount;

    void Start()
    {
        coinCount.text = CoinsQuantity.ToString();    
    }

    public void AddCoin()
    {
        CoinsQuantity++;
        coinCount.text = CoinsQuantity.ToString();

        if( CoinsQuantity >= 3)
        {
            GetComponent<PlayerHealth>().EndGame();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.tag == "Player" && gameObject.activeSelf)
        {
            Observer.GetInstance().Trigger(Observer.Event.CoinCollection);
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IInteractable
{
    [SerializeField] private int amount;

    public void OnInteract()
    {
        GameManager.Instance.AddMoney(amount);

        // Debug.Log("Interact: " + amount + " Coin.");
        Destroy(gameObject);
    }

}

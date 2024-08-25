using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private List<GameObject> eventPrefabs;

    public void OnInteract()
    {
        Instantiate(eventPrefabs[Random.Range(0, eventPrefabs.Count)], Vector3.zero, Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] List<GameObject> scenePrefab;
    [SerializeField] private Transform foreground;
    [SerializeField] private Transform midground;
    [SerializeField] private Transform background;

    [Header("Scene Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float midgroundRatio;
    [SerializeField] private float backgroundRatio;

    private const float sceneLength = 20f;

    private void Start()
    {
        // Pre-gen scene
        for (int i = 0; i < 3; i++)
        {
            Instantiate(scenePrefab[Random.Range(0, scenePrefab.Count)], new Vector2(i * sceneLength, -4f), Quaternion.identity, foreground);
        }
    }

    void Update()
    {
        for (int i = 0; i < foreground.childCount; i++)
        {
            foreground.GetChild(i).Translate(speed * Time.deltaTime * -Vector2.right);
        }

        if (foreground.GetChild(0).position.x < -20)
        {
            Destroy(foreground.GetChild(0).gameObject);
            Instantiate(scenePrefab[Random.Range(0, scenePrefab.Count)], new Vector2(foreground.GetChild(foreground.childCount - 1).position.x + sceneLength, -4f), Quaternion.identity, foreground);
        }
    }
}

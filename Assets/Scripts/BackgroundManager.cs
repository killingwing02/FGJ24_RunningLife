using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> scenePrefab;
    [SerializeField] private Transform foreground;
    [SerializeField] private Transform midground;
    [SerializeField] private Transform background;

    [Header("Scene Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float midgroundRatio;
    [SerializeField] private float backgroundRatio;

    private float speedFixed = 1f;

    private const float sceneLength = 20f;

    private void Start()
    {
        // Pre-gen scene
        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
                Instantiate(scenePrefab[0], new Vector2(0, -4f), Quaternion.identity, foreground);
            else
                Instantiate(scenePrefab[Random.Range(1, scenePrefab.Count)], new Vector2(i * sceneLength, -4f), Quaternion.identity, foreground);
        }
    }

    void Update()
    {
        for (int i = 0; i < foreground.childCount; i++)
        {
            foreground.GetChild(i).Translate(speed * speedFixed * Time.deltaTime * Vector2.left);
        }

        if (foreground.GetChild(0).position.x < -20)
        {
            Destroy(foreground.GetChild(0).gameObject);
            Instantiate(scenePrefab[Random.Range(1, scenePrefab.Count)], new Vector2(foreground.GetChild(foreground.childCount - 1).position.x + sceneLength, -4f), Quaternion.identity, foreground);
        }
    }

    public void ChangeFixedSpeedRatio(float speed)
    {
        speedFixed = speed;
    }
}

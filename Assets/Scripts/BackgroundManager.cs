using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> scenePrefab;
    [SerializeField] private GameObject midgroundPrefab;
    [SerializeField] private Transform foreground;
    [SerializeField] private Transform midground;
    [SerializeField] private Transform background;

    [Header("Scene Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float midgroundRatio;
    [SerializeField] private float backgroundRatio;

    private float speedFixed = 1f;

    private const float sceneLength = 20f;
    private const float midgroundLength = 24.77999f;

    private void Start()
    {
        Initialize();
        GameManager.Instance.onGameStart.AddListener(OnGameStart);
    }

    void Update()
    {
        for (int i = 0; i < foreground.childCount; i++)
        {
            foreground.GetChild(i).Translate(speed * speedFixed * Time.deltaTime * Vector2.left);
            midground.GetChild(i).Translate(speed * speedFixed * midgroundRatio * Time.deltaTime * Vector2.left);
        }

        if (foreground.GetChild(0).position.x < -sceneLength)
        {
            Destroy(foreground.GetChild(0).gameObject);
            Instantiate(scenePrefab[Random.Range(1, scenePrefab.Count)], new Vector2(foreground.GetChild(foreground.childCount - 1).position.x + sceneLength, -4f), Quaternion.identity, foreground);
        }

        if (midground.GetChild(0).position.x < -midgroundLength)
        {
            Destroy(midground.GetChild(0).gameObject);
            Instantiate(midgroundPrefab, new Vector2(midground.GetChild(midground.childCount - 1).position.x + midgroundLength, 0f), Quaternion.identity, midground);
        }
    }

    public void Initialize()
    {
        foreach (GameObject child in foreground)
        {
            Destroy(child);
        }

        // Pre-gen scene
        for (int i = 0; i < 3; i++)
        {
            // Foreground
            if (i == 0)
                Instantiate(scenePrefab[0], new Vector2(0, -4f), Quaternion.identity, foreground);
            else
                Instantiate(scenePrefab[Random.Range(1, scenePrefab.Count)], new Vector2(i * sceneLength, -4f), Quaternion.identity, foreground);

            // Midground
            Instantiate(midgroundPrefab, new Vector2(i * midgroundLength, 0f), Quaternion.identity, midground);
        }
    }

    public void OnGameStart()
    {
        speedFixed = 1f;
    }

    public void ChangeFixedSpeedRatio(float speed)
    {
        speedFixed = speed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;

    private void Start()
    {
        LeanTween.alphaCanvas(group, .3f, 1f).setLoopPingPong();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(1);
        }
    }
}

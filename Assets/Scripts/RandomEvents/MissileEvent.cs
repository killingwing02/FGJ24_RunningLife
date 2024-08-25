using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileEvent : MonoBehaviour
{
    [SerializeField] GameObject missile;
    [SerializeField] GameObject explode;
    [SerializeField] List<Sprite> frames;
    [SerializeField] CanvasGroup canvas;
    [SerializeField] private float previewTime;
    [SerializeField] private float delayTime;

    private void OnEnable()
    {
        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation()
    {
        LeanTween.moveY(missile, 6.7f, previewTime);
        yield return new WaitForSeconds(previewTime + delayTime);

        explode.SetActive(true);
        var image = explode.GetComponent<Image>();

        for (int i = 0; i < frames.Count; i++)
        {
            image.sprite = frames[i];
            yield return new WaitForSeconds(1f / 24f);
        }

        LeanTween.alphaCanvas(canvas, 0f, 1f).setOnComplete(() =>
        {
            GameManager.Instance.RemoveCurrentHp(99999);
            Destroy(gameObject);
        });
    }
}


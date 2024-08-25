using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderEvent : MonoBehaviour
{
    [SerializeField] GameObject rider;
    [SerializeField] float performanceTime;
    [SerializeField] RectTransform riderUI;
    [SerializeField] CanvasGroup canvas;
    [SerializeField] CanvasGroup worldCanvas;

    private IEnumerator Start()
    {
        LeanTween.moveX(rider, -13.6f, performanceTime);

        yield return new WaitForSeconds(performanceTime / 2f);

        canvas.alpha = 1.0f;
        LeanTween.scale(riderUI, Vector3.one * 2f, 1f).setEaseOutCubic();
        
        yield return new WaitForSeconds(3f);

        canvas.alpha = 0f;
        GameManager.Instance.RemoveCurrentHp(30);

        LeanTween.alphaCanvas(worldCanvas, 1f, 3f);
        LeanTween.move(worldCanvas.gameObject, new Vector3(-2f, 0, 0), 3f);
        LeanTween.rotateZ(worldCanvas.gameObject, -20f, 3f);

        yield return new WaitForSeconds(3f);
        LeanTween.alphaCanvas(worldCanvas, 0f, 1f);
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}

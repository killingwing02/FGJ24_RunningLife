using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneEvent : MonoBehaviour
{
    [SerializeField] GameObject planeObj;
    [SerializeField] RectTransform planeUI;
    [SerializeField] Vector3 startPos;
    [SerializeField] Vector3 endPos;
    [SerializeField] Transform startTrans;
    [SerializeField] Transform endTrans;
    [SerializeField] CanvasGroup canvas;
    [SerializeField] float previewTime;
    [SerializeField] float delayTime;

    IEnumerator Start()
    {
        planeObj.transform.position = startPos;
        LeanTween.move(planeObj, endPos, previewTime);

        yield return new WaitForSeconds(previewTime + delayTime);
        
        planeUI.position = startTrans.position;
        planeUI.rotation = startTrans.rotation;
        planeUI.localScale = Vector3.zero;

        LeanTween.move(planeUI, endTrans.position, 3f);
        //LeanTween.rotate(planeUI, endTrans.eulerAngles, 3f);
        LeanTween.scale(planeUI, Vector3.one, 6f);
        LeanTween.color(planeUI, Color.black, 5f).setOnComplete(() => LeanTween.alphaCanvas(canvas, 0f, 1f));

        yield return new WaitForSeconds(6f);
        GameManager.Instance.RemoveCurrentHp(99999);
        Destroy(gameObject);
    }
}

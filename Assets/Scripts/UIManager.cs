using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> heartsSprite;
    [SerializeField] private Transform heartsTrans;
    [SerializeField] private RectTransform diedText;
    [SerializeField] private CanvasGroup diedGroup;

    [Header("Value Settings")]
    [SerializeField] private float hurtTurnRedDuration;
    [SerializeField] private LeanTweenType hurtTurnRedType;

    private void Start()
    {
        //YouDiedAnimation();
    }

    public void HpUiDisplayCalculator(int health, int maxHealth)
    {
        Debug.Log("Current HP: " + health);
        if (health < 0) return;

        // Every half heart is 0.1f * 10 = 1f
        // Maxmin hearts is 10f
        var ratio = health * 10 / (float)maxHealth;
        var fullHeart = (int)(ratio / 2);
        var halfHeart = (int)(ratio % 2) != 0 ? true : false;
        if (fullHeart <= 0 && ratio != 0) halfHeart = true;

        for (int i = 0; i < heartsTrans.childCount; i++)
        {
            if (fullHeart > 0)
            {
                heartsTrans.GetChild(i).GetComponent<Image>().sprite = heartsSprite[2];
                fullHeart--;
            }
            else if (fullHeart <= 0 && halfHeart)
            {
                heartsTrans.GetChild(i).GetComponent<Image>().sprite = heartsSprite[1];
                halfHeart = false;
            }
            else
            {
                heartsTrans.GetChild(i).GetComponent<Image>().sprite = heartsSprite[0];
            }
        }

        HurtHeartsAnimation();
    }

    public void HurtHeartsAnimation()
    {
        foreach (RectTransform heart in heartsTrans)
        {
            LeanTween.cancel(heart.gameObject);

            heart.GetComponent<Image>().color = Color.red;
            LeanTween.color(heart, Color.white, hurtTurnRedDuration).setEase(hurtTurnRedType);

            var pos = heart.position;
            pos.y += Random.Range(-10f, 10f);
            heart.position = pos;
            LeanTween.moveY(heart, -45f, hurtTurnRedDuration).setEase(hurtTurnRedType);
        }
    }

    public void YouDiedAnimation()
    {
        diedGroup.gameObject.SetActive(true);
        diedGroup.alpha = 0f;
        diedText.localScale = Vector3.one;
        LeanTween.alphaCanvas(diedGroup, 1f, 2f);
        LeanTween.scale(diedText, Vector3.one * 1.3f, 7f);
        LeanTween.delayedCall(5f, () => LeanTween.alphaCanvas(diedGroup, 0f, 2f).setOnComplete(() => diedGroup.gameObject.SetActive(false)));
    }
}

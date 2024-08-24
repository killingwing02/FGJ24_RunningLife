using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> heartsSprite;

    public void UiDisplayCalculator(int health, int maxHealth)
    {
        Debug.Log("Current HP: " + health);
        if (health < 0) return;

        // Every half heart is 0.1f * 10 = 1f
        // Maxmin hearts is 10f
        var ratio = health * 10 / (float)maxHealth ;
        var fullHeart = (int)(ratio / 2);
        var halfHeart = (int)(ratio % 2) != 0 ? true : false;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (fullHeart > 0)
            {
                transform.GetChild(i).GetComponent<Image>().sprite = heartsSprite[2];
                fullHeart--;
            }
            else if (fullHeart <= 0 && halfHeart)
            {
                transform.GetChild(i).GetComponent<Image>().sprite = heartsSprite[1];
                halfHeart = false;
            }
            else
            {
                transform.GetChild(i).GetComponent<Image>().sprite = heartsSprite[0];
            }
        }
    }
}

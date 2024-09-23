using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemFader : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public void FadeIn()
    {
        colorValue(new Color(1, 1, 1, 1));
    }

    public void FadeOut()
    {
        colorValue(new Color(1, 1, 1, Settings.targetAlpha));
    }

    private void colorValue(Color value)
    {
        if (value == new Color(1, 1, 1, 1))
        {
            Color targetColor = new Color(1, 1, 1, 1);
            spriteRenderer.DOColor(targetColor, Settings.itemFadeDuration);
        }
        else
        {
            Color targetColor = new Color(1, 1, 1, Settings.targetAlpha);
            spriteRenderer.DOColor(targetColor, Settings.itemFadeDuration);
        }
    }
}

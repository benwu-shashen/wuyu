using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileFader : MonoBehaviour
{
    private Tilemap tileMap;
    private Color tilemapColor;
    // Start is called before the first frame update
    void Awake()
    {
        tileMap = GetComponent<Tilemap>();
        tilemapColor = tileMap.color;
    }

    // Update is called once per frame
    public void FadeIn()
    {
        DOTween.To(() => tilemapColor.a, a => tilemapColor.a = a, Settings.InitialAlpha, Settings.itemFadeDuration)
       .OnUpdate(() => tileMap.color = tilemapColor);
    }

    public void FadeOut()
    {
        DOTween.To(() => tilemapColor.a, a => tilemapColor.a = a, Settings.targetAlpha, Settings.itemFadeDuration)
       .OnUpdate(() => tileMap.color = tilemapColor);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerItemFader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemFader[] itemFaders = other.GetComponentsInChildren<ItemFader>();
        TileFader[] tileFaders = other.GetComponentsInChildren<TileFader>();
        if (itemFaders.Length > 0)
        {
            foreach (var item in itemFaders)
                item.FadeOut();
        }
        if (tileFaders.Length > 0)
        {
            foreach (var tile in tileFaders)
                tile.FadeOut();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ItemFader[] itemFaders = other.GetComponentsInChildren<ItemFader>();
        TileFader[] tileFaders = other.GetComponentsInChildren<TileFader>();
        if (itemFaders.Length > 0)
        {
            foreach (var item in itemFaders)
            {
                item.FadeIn();
            }
        }
        if (tileFaders.Length > 0)
        {
            foreach (var tile in tileFaders)
                tile.FadeIn();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public Color emptyColor = Color.white;
    public Color currentColor = Color.white;
    public int currentCount = 0;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Initialize the count to 1 when the game starts (if the terminal is empty)
        if (currentColor == emptyColor)
        {
            currentCount = 0;
        }
    }


    public void ChangeColor(Color newColor)
    {
        spriteRenderer.color = newColor;
        currentColor = newColor;
    }

    public void IncrementCount()
    {
        currentCount++;
    }

    public void ResetCount(int count)
    {
        currentCount = count;
    }
}
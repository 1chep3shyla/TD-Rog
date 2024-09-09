using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
   // Array of sprites to cycle through
    public Sprite[] sprites;
    // Time delay between sprite changes
    public float delay = 0.5f;
    // Boolean to determine if the animation should loop
    public bool loop = true;

    // Reference to the SpriteRenderer component
    private SpriteRenderer spriteRenderer;
    // Index to keep track of the current sprite
    private int currentIndex = 0;
    // Coroutine reference for the animation
    private Coroutine animationCoroutine;

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start the animation coroutine
        if (sprites.Length > 0)
        {
            animationCoroutine = StartCoroutine(AnimateSprites());
        }
        else
        {
            Debug.LogWarning("No sprites assigned to the SpriteAnimator.");
        }
    }

    private IEnumerator AnimateSprites()
    {
        while (true)
        {
            // Set the current sprite
            spriteRenderer.sprite = sprites[currentIndex];
            // Increment the index
            currentIndex++;

            // Reset the index if it exceeds the array length
            if (currentIndex >= sprites.Length)
            {
                if (loop)
                {
                    currentIndex = 0;
                }
                else
                {
                    // Stop the animation if not looping
                    break;
                }
            }

            // Wait for the specified delay
            yield return new WaitForSeconds(delay);
        }
    }

    // Method to start the animation
    public void StartAnimation()
    {
        if (animationCoroutine == null)
        {
            animationCoroutine = StartCoroutine(AnimateSprites());
        }
    }

    // Method to stop the animation
    public void StopAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }
}
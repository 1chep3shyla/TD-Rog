using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Button[] buttons;

    private void Update()
    {
        // Check if any number key is pressed
        for (int i = 0; i < buttons.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                GameManager.Instance.DownLay();
                SelectButton(i);
            }
        }
    }

    public void SelectButton(int selectedIndex)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            Animator buttonAnimator = button.GetComponent<Animator>();

            // Set the "Selected" parameter in the button's animator
            if (buttonAnimator != null && button.interactable)
            {
                buttonAnimator.SetBool("Selected", i == selectedIndex);
                buttonAnimator.SetBool("Normal", i != selectedIndex);
            }
            

            // Simulate button click for the selected button
            if (i == selectedIndex && button.interactable)
            {
                button.onClick.Invoke(); // This line is likely causing the recursive call
            }
        }
    }

    public void OffBut(int selectedIndex)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            Animator buttonAnimator = button.GetComponent<Animator>();

            // Check if the button is active before updating its animation
            if (button.interactable)
            {
                // Set the "Selected" parameter in the button's animator
                if (buttonAnimator != null)
                {
                    buttonAnimator.SetBool("Normal", i != selectedIndex);
                }
            }
        }
    }
}
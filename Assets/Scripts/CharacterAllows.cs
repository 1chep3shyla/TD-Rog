using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterAllows : MonoBehaviour
{
    public Character[] allChar;
    public Button[] buttons;
    public CursorWork cursorWork;

    void Update()
    {
        buttons = cursorWork.buttons;
        for(int i =1; i < buttons.Length; i++)
        {
            if(allChar[i].isHave)
            {
                buttons[i].interactable = true;
            }
            else
            {
                buttons[i].interactable = false;
            }
        }
    }
} 
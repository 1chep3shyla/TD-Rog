using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beastiar 
{
    public bool[] seeThis = new bool[50];


    private static Beastiar instance;
    private Beastiar() { }

    public static Beastiar Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Beastiar();
                //instance.seeThis[2] = true;
            }
            return instance;
        }
    }
}
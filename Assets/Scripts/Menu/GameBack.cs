using UnityEngine;
public class GameBack
{
    private static GameBack instance;
    public Sprite iconChar;
    public ICharSet charData;
    public string curFormula;
    public int indexState = 1;
    public static GameBack Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameBack();
            }
            return instance;
        }
    }
}

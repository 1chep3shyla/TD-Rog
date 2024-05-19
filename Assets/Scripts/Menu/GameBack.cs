using UnityEngine;
public class GameBack
{
    public static GameBack instance;
    public Sprite iconChar;
    public ICharSet charData;
    public string curFormula;
    public int indexState = 1;
    public float volumeMusic;
    public float volumeSFX;
    [SerializeField]
    public bool saveThis;

    public int souls;
    public int gold;
    public int waveCount;
    public int gamePlayed;
    public int winGame;
    public bool loseFirstWave;
    public int enemiesKilled;
    public float[] buff = new float[9];
    public float[] buffGlobal = new float[9];
    public bool[] boughtStat = new bool[32];
    public int getItem;
    public int sellItem;
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

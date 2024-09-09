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
    public float[] secondsBuff = new float[16];
    public bool[] boughtStat = new bool[72]; // you need change this 
    public int[] waveInStage = new int[14];
    public int getItem;
    public int sellItem;
    public int damageAll;

    public int healthWin;
    public int healthBreak;
    public int towerSet;
    public int poisonedCount;
    public int fireCount;
    public int iceCount;
    public bool minSpeed;
    public int hardWinning;
    public int perfecto;
    public bool firstEvolve;
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

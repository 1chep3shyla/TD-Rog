public class GameBack
{
    private static GameBack instance;
    public ICharSet charData;
    public string curFormula;
    public int indexState;
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

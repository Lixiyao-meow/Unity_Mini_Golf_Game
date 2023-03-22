using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard: MonoBehaviour
{
    public Text puttsCounter;

    private int putts;

    public void AddPutt()
    {
        putts++;
        puttsCounter.text = putts.ToString();
    }

    public void ResetPutts()
    {
        putts = 0;
    }
}

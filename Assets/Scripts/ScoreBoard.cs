using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard: MonoBehaviour
{
    public Text puttsCounter;

    public void changePuttNumber(int putts)
    {
        puttsCounter.text = putts.ToString();
    }
}

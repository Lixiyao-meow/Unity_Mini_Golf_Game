using UnityEngine;
using UnityEngine.UI;
using Ubiq.Messaging;

public class ScoreBoard : MonoBehaviour
{
    public Text puttsCounter;

    private int putts;

    NetworkContext context;

    private void Start()
    {
        context = NetworkScene.Register(this);
    }

    private struct Message
    {
        public int putts;
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        // Parse the message
        var m = message.FromJson<Message>();

        putts = m.putts;
        puttsCounter.text = putts.ToString();
    }

    public void AddPutt()
    {
        putts++;
        puttsCounter.text = putts.ToString();

        context.SendJson(new Message()
        {
            putts = putts
        });
    }

    public void ResetPutts()
    {
        putts = 0;
    }
}

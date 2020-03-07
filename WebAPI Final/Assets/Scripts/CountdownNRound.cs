using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using V966;

public class CountdownNRound : MonoBehaviour
{

    Timer start;
    Timer round;
    bool first = false;

    public Text countDown;
    public Slider roundTimer;

    // Start is called before the first frame update
    void Start()
    {
        start = new Timer(5);
        round = new Timer(30);
        start.startTimer();
    }

    // Update is called once per frame
    void Update()
    {
        start.timerUpdate();
        
        if (start.checkTimer())
        {
            if (!first)
            {
                //send round start message
                JSONObject data = new JSONObject();
                data.AddField("roomCode", Network.I.roomCode);
                Network.socket.Emit("roundStart", data);
                start.pauseTimer();
                start.zeroTimer();
                countDown.text = "Start!";
                roundTimer.gameObject.SetActive(true);
                round.startTimer();
                first = true;
            }
            countDown.CrossFadeAlpha(0, 2.0f, false);
            round.timerUpdate();
            roundTimer.value = round.getPercent();
            if (round.checkTimer())
            {
                JSONObject data = new JSONObject();
                data.AddField("roomCode", Network.I.roomCode);
                Network.socket.Emit("roundEnd", data);
                round.resetTimer();
            }
            

        }
        else
        {
            countDown.text = Mathf.Round(start.getTimeLeft()).ToString();
        }
    }
}

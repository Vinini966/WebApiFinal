using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SocketIO;
using System;

public class Network : MonoBehaviour
{
    public static SocketIOComponent socket;
    public static Network I;
    const string roomGenString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    System.Random r = new System.Random();
    public string roomCode;

    [SerializeField]
    public Dictionary<string, int> players;

    public Text RoomCode;
    public Text userNames;


    // Start is called before the first frame update
    void Start()
    {
        if(I == null)
        {
            I = this;
            DontDestroyOnLoad(this.gameObject);
        }
        RollRoomCode();
        players = new Dictionary<string, int>();

        socket = GetComponent<SocketIOComponent>();

        socket.On("err", errHandling);
        socket.On("handshake", OnHandshake);
        socket.On("clientJoin", OnPlayerJoin);
        socket.On("gameBegining", BeginGame);
        socket.On("score", SetScores);
        
    }

    private void SetScores(SocketIOEvent e)
    {
        string tmp = e.data["userName"].ToString();
        tmp = tmp.Replace("\"", "");
        players[tmp] = Int32.Parse(e.data["playerScore"].ToString());
        SceneManager.LoadScene("Score");
    }

    private void BeginGame(SocketIOEvent obj)
    {
        SceneManager.LoadScene("Game");
    }

    private void OnPlayerJoin(SocketIOEvent e)
    {
        string tmp = e.data["userName"].ToString();
        tmp = tmp.Replace("\"", "");
        players.Add(tmp, 0);
        userNames.text = "";
        foreach(KeyValuePair<string,int> u in players)
        {
            userNames.text += u.Key + '\n';
        }

    }

    private void OnHandshake(SocketIOEvent e)
    {
        userNames.text = "";
        JSONObject data = new JSONObject();
        data.AddField("roomCode", roomCode);
        data.AddField("client", false);
        socket.Emit("connInfo", data);
    }

    private void errHandling(SocketIOEvent e)
    {
        Debug.Log(e.data["err"].ToString());
        switch (Int32.Parse(e.data["errCode"].ToString()))
        {
            case 1:
                RollRoomCode();
                OnHandshake(null);
                break;
        }
    }

    private void RollRoomCode()
    {
        roomCode = "";
        for(int i = 0; i < 4; i++)
        {
            roomCode += roomGenString[r.Next(0, roomGenString.Length)];
        }
        RoomCode.text = roomCode;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

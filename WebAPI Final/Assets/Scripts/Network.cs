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
    static Network I;
    const string roomGenString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    System.Random r = new System.Random();
    public string roomCode;
    [SerializeField]
    public List<string> players;

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

        socket = GetComponent<SocketIOComponent>();

        socket.On("err", errHandling);
        socket.On("handshake", OnHandshake);
        socket.On("clientJoin", OnPlayerJoin);
        socket.On("gameBegining", BeginGame);
        
    }

    private void BeginGame(SocketIOEvent obj)
    {
        SceneManager.LoadScene("Game");
    }

    private void OnPlayerJoin(SocketIOEvent e)
    {
        string tmp = e.data["userName"].ToString();
        tmp = tmp.Replace("\"", "");
        players.Add(tmp);
        foreach(string u in players)
        {
            userNames.text += u + '\n';
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

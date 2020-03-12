using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class ChaseManager : MonoBehaviour
{
    [SerializeField]
    static public Dictionary<string, ChaceBox> userChaseBox;
    public GameObject chaseBox;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < Network.I.playerChaseData.Count; i++)
        {
            string tmp = Network.I.playerChaseData[i]["userName"].ToString();
            tmp = tmp.Replace("\"", "");
            var box = Instantiate(chaseBox, this.transform);
            box.GetComponent<ChaceBox>().playerLoc = Int32.Parse(Network.I.playerChaseData[i]["playerStart"].ToString());
            box.GetComponent<ChaceBox>().scoreTotal = Int32.Parse(Network.I.playerChaseData[i]["playerScore"].ToString());
            box.GetComponent<ChaceBox>().Username.text = tmp;
            userChaseBox.Add(tmp, box.GetComponent<ChaceBox>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

}

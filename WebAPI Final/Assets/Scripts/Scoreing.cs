using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Scoreing : MonoBehaviour
{
    public GameObject scoreCard;
    public Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        var sortedDict = Network.I.players.OrderByDescending(x => x.Value).ToList();
        for(int i = 0; i < sortedDict.Count(); i++)
        {
            var card = Instantiate(scoreCard, parent);
            card.GetComponent<Text>().text = "-----" + (i+1).ToString() + "-----\n" +
                                             sortedDict[i].Key.ToString() + "\n" +
                                             sortedDict[i].Value.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

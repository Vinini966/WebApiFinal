using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaceBox : MonoBehaviour
{
    [SerializeField]
    public List<Image> boxesList;

    public Text Username;

    public int playerLoc;
    public int chacerLoc;
    public int scoreTotal;

    // Start is called before the first frame update
    void Start()
    {

        chacerLoc = boxesList.Count;

    }

    // Update is called once per frame
    void Update()
    {
        CalculateColors();
    }

    public void CalculateColors()
    {

        boxesList[playerLoc].color = Color.green;

        if(chacerLoc < boxesList.Count)
        {
            for(int i = chacerLoc; i < boxesList.Count; i++)
            {
                boxesList[i].color = Color.red;
            }
        }

        for(int i = playerLoc + 1; i < chacerLoc; i++)
        {
            boxesList[i].color = Color.white;
        }

        for (int i = playerLoc - 1; i >= 0; i--)
        {
            boxesList[i].color = Color.black;
        }

    }
}

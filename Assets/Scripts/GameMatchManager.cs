using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameMatchManager : MonoBehaviour
{

    public Text playerWinText;
    public Button menuButton;
    public GameObject victoryPanel;
    //public List<PlayerInput> players = new List<PlayerInput>();
    public List<GameObject> _players = new List<GameObject>();
    //public int playerNum;
    public GameObject winnerModel;
    public Material winnerModelMainColor;
    public Material winnerModelAltColor;

    public Material winnerColMain;
    public Material winnerColAlt;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        victoryPanel.SetActive(false);
        winnerModel.SetActive(false);
    }

    public void DeclareWinner()
    {
        victoryPanel.SetActive(true);
        winnerModel.SetActive(true);
        print("Winner!");
        /* foreach (GameObject player in _players)
         {
             GameObject winnerPlayer = player.gameObject;
             winnerColMain = winnerPlayer.GetComponent<SkinnedMeshRenderer>().materials[0];
             winnerColAlt = winnerPlayer.GetComponent<SkinnedMeshRenderer>().materials[1];
             break;
         }*/

        winnerColMain = _players[0].GetComponent<playerAttackScript>().mainColor;
        winnerColAlt = _players[0].GetComponent<playerAttackScript>().altColor;

        winnerModel.GetComponent<SkinnedMeshRenderer>().materials.SetValue(winnerColMain, 0);
        winnerModel.GetComponent<SkinnedMeshRenderer>().materials.SetValue(winnerColAlt, 1);
        /*if (_players.Count == 1)
        {
            //winnerModel = _players[0].gameObject;
            winnerColMain = _players[0].GetComponent<playerAttackScript>().mainColor;
            winnerColAlt = _players[0].GetComponent<playerAttackScript>().altColor;

            winnerModel.GetComponent<SkinnedMeshRenderer>().materials.SetValue(winnerColMain, 0);
            winnerModel.GetComponent<SkinnedMeshRenderer>().materials.SetValue(winnerColAlt, 1);
        }*/


    }

    public void RemovePlayer(GameObject player)
    {
        _players.Remove(player);
        print(_players.Count);
        if (_players.Count <= 1)
        {
            DeclareWinner();
        }
    }
}

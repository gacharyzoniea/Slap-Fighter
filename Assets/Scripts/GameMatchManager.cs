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

        winnerColMain = winnerModel.GetComponent<SkinnedMeshRenderer>().materials[0];
        winnerColAlt = winnerModel.GetComponent<SkinnedMeshRenderer>().materials[1];
        victoryPanel.SetActive(false);
        winnerModel.SetActive(false);
    }

    public void DeclareWinner(int playernum)
    {
        victoryPanel.SetActive(true);
        winnerModel.SetActive(true);
        //winnerColMain = _players[playernum -1].GetComponent<playerAttackScript>().mainColor;
        //winnerColAlt = _players[playernum -1].GetComponent<playerAttackScript>().altColor;
        winnerColMain = _players[playernum - 1].GetComponentInChildren<SkinnedMeshRenderer>().materials[0];
        print(_players[playernum - 1].GetComponentInChildren<SkinnedMeshRenderer>().materials[0]);
        winnerColAlt = _players[playernum - 1].GetComponentInChildren<SkinnedMeshRenderer>().materials[1];
        GameObject victoryModel = Instantiate(winnerModel, winnerModel.transform.position, winnerModel.transform.rotation, gameObject.transform);
        victoryModel.GetComponent<Renderer>().material = winnerColMain;
        winnerModel.SetActive(false);
        //var player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);

        //winnerModel.GetComponent<SkinnedMeshRenderer>().materials.SetValue(winnerColMain, 0);
        //winnerModel.GetComponent<SkinnedMeshRenderer>().materials.SetValue(winnerColAlt, 1);
        winnerModel.GetComponent<SkinnedMeshRenderer>().materials[0] = winnerColMain;
        print(winnerModel.GetComponent<SkinnedMeshRenderer>().materials[0]);
        //winnerModel.GetComponent<SkinnedMeshRenderer>().materials.SetValue(winnerColMain, 0);
        print(winnerModel.GetComponent<SkinnedMeshRenderer>().materials[0]);
        print(winnerColMain);
        
        winnerModel.GetComponent<SkinnedMeshRenderer>().materials[1] = winnerColAlt;
        winnerModel.GetComponent<Renderer>().materials[0] = winnerColMain;
        winnerModel.GetComponent<Renderer>().materials[1] = winnerColAlt;
        print("Winner!");

        playerWinText.text = "Player " + playernum + " wins!";
        /* foreach (GameObject player in _players)
         {
             GameObject winnerPlayer = player.gameObject;
             winnerColMain = winnerPlayer.GetComponent<SkinnedMeshRenderer>().materials[0];
             winnerColAlt = winnerPlayer.GetComponent<SkinnedMeshRenderer>().materials[1];
             break;
         }*/

        
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
        int winnerNum;
        int i = _players.IndexOf(player);
        int index = i;
        //_players.Remove(player);
        if (index == 0)
        {
            winnerNum = 2;
        }
        else if (index == 1)
        {
            winnerNum = 1;
        }
        else
        {
            winnerNum = 0;
            
        }
        //print(_players.Count);
        //if (_players.Count <= 1)
        //{
            print(index + " " + i + " " + winnerNum);
            DeclareWinner(winnerNum);
        
        //}
    }
}

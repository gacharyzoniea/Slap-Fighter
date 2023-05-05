using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameMatchManager : MonoBehaviour
{

    public Text playerWinText;
    public Button menuButton;
    public List<PlayerInput> players = new List<PlayerInput>();
    public List<GameObject> _players = new List<GameObject>();
    //public int playerNum;
    public GameObject winnerModel;

    Material winnerColMain;
    Material winnerColAlt;

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
        
    }

    public void DeclareWinner()
    {
        
        foreach (GameObject player in _players)
        {
            GameObject winnerPlayer = player.gameObject;
            winnerColMain = winnerPlayer.GetComponent<SkinnedMeshRenderer>().materials[0];
            winnerColAlt = winnerPlayer.GetComponent<SkinnedMeshRenderer>().materials[1];
            break;
        }
        winnerModel.GetComponent<SkinnedMeshRenderer>().materials[0] = winnerColMain;
        winnerModel.GetComponent<SkinnedMeshRenderer>().materials[1] = winnerColAlt;

    }

    public void RemovePlayer(GameObject player)
    {
        _players.Remove(player);
        if (_players.Count == 1)
        {
            DeclareWinner();
        }
    }
}

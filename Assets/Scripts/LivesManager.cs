using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LivesManager : MonoBehaviour
{

    public int stocks = 2;
    public bool assigned = false;
    public GameObject _player;
    public GameMatchManager _gmm;
    public PlayerManagerScript _playerManager;
    public int _playerNum;
    public PlayerInput _playerInput;


    GameObject stock1GameObject;
    GameObject stock2GameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        stock1GameObject = transform.GetChild(0).gameObject;
        stock2GameObject = transform.GetChild(1).gameObject;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (stocks >= 2)
        {
            stock1GameObject.SetActive(true);
            stock2GameObject.SetActive(true);
        }
        if (stocks == 1)
        {
            stock1GameObject.SetActive(true);
            stock2GameObject.SetActive(false);
        }
        if (stocks <= 0)
        {
            stock1GameObject.SetActive(false);
            stock2GameObject.SetActive(false);
            //Remove player from the list, then report to the game match manager that this player is out
            //_playerManager.players.Remove(_playerInput);
            _gmm.RemovePlayer(_playerInput.gameObject);
        }
        /*else
        {
            stock1GameObject.SetActive(false);
            stock2GameObject.SetActive(false);
            print("Game Over!");
        }*/
    }

    public void assign()
    {
        assigned = true;
    }
}

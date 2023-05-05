using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManagerScript : MonoBehaviour
{
    public List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField]
    private List<Transform> startingPoints;
    [SerializeField]
    //private List<LayerMask> playerLayers;
    //private GameObject[] playerList;
    public GameObject[] healthList;

    public GameMatchManager _gmm;


    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        
        //healthList = GameObject.FindGameObjectsWithTag("Right");
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }
    private void AddPlayer(PlayerInput player)
    {
        players.Add(player);

        Transform playerParent = player.transform;
        playerParent.position = startingPoints[players.Count - 1].position;

        _gmm._players.Add(player.gameObject);

    }

    private void Update()
    {
        //playerList = GameObject.FindGameObjectsWithTag("Player");
        //foreach (GameObject player in playerList){
        //    player.playerAttackScript()
        //}
    }

    
}

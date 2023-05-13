using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeLevel : MonoBehaviour
{
    [SerializeField]
    private Transform[] playerSpawns;
    [SerializeField]
    private GameObject playerPrefab;
    public GameMatchManager _gmm;
    public CameraMoveScript _cameraScript;

    // Start is called before the first frame update
    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        for(int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
            player.GetComponent<PlayerInputScript>().InitializePlayer(playerConfigs[i]);
            _gmm._players.Add(player);
        }
        _cameraScript.playerOne = _gmm._players[0];
        _cameraScript.playerTwo = _gmm._players[1];
    }
    private void Update()
    {
        if(transform.childCount > 0)
        {
            transform.DetachChildren();
        }
    }

}

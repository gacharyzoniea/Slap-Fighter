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
    public CameraScript _cam;

    // Start is called before the first frame update
    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        for(int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
            player.GetComponent<PlayerInputScript>().InitializePlayer(playerConfigs[i]);
            _gmm._players.Add(player);
            _cam.players.Add(player);
        }

    }
    private void Update()
    {
        if(transform.childCount > 0)
        {
            transform.DetachChildren();
        }
    }

}

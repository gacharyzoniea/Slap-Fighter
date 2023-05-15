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
    public LevelFocusScript _lfs;
    private void Awake()
    {
        GameObject musicObj = GameObject.FindGameObjectWithTag("GameMusic");
        Destroy(musicObj);
    }

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
            _lfs.players.Add(player);
        }

        _gmm._players[0].layer = 15;
        _gmm._players[1].layer = 16;

    }
    private void Update()
    {
        if(transform.childCount > 0)
        {
            transform.DetachChildren();
        }
    }

}

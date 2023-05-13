using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{

    public GameObject playerOne;
    public GameObject playerTwo;
    public GameMatchManager _gmm;
    public float travelTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        //playerOne = _gmm._players[0];
        //playerTwo = _gmm._players[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        
    }

    private void LateUpdate()
    {
        if (playerOne && playerTwo)
        {
            Vector3.Lerp(playerOne.transform.position, playerTwo.transform.position, Time.fixedDeltaTime * travelTime);
        }
       
    }
}

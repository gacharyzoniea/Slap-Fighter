using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformInteractScript : MonoBehaviour
{

    //public LayerMask platformLayer = 6;
    //public LayerMask groundLayer = 3;
    int platformLayer = 6;
    int groundLayer = 3;
    int hitboxLayer = 12;
    public Vector3 playerPos;
    public PlayerMovementFixed _pm;

    //private PlayerMovementFixed _pm;
    //private InputAction _move;
    // Start is called before the first frame update
    void Start()
    {
       // _pm = GetComponent<PlayerMovementFixed>();
       // _move = player.FindAction("Move");

    }

    // Update is called once per frame
    void Update()
    {
        

            if (Input.GetKey(KeyCode.S))
            {

            //Vector3.Slerp(playerPos, new Vector3(playerPos.x, playerPos.y - 2f, playerPos.z), 0.01f);
            //(new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 0.5f, this.gameObject.transform.position.z));
                Physics.IgnoreLayerCollision(12, platformLayer, true);

            }
            else
            {
                Physics.IgnoreLayerCollision(12, platformLayer, false);
            }

    }
}

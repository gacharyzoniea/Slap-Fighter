using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformInteractScript : MonoBehaviour
{

    public LayerMask platformLayer = 6;
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
            Physics.IgnoreLayerCollision(this.gameObject.layer, platformLayer, true);

        }
        else
        {
            Physics.IgnoreLayerCollision(this.gameObject.layer, platformLayer, false);
        }
    }
}

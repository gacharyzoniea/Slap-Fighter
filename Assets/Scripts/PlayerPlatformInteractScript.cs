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
    playerAttackScript _playerAttackScript;
    private PlayerMovementFixed pm;

    //private PlayerMovementFixed _pm;
    //private InputAction _move;
    // Start is called before the first frame update
    void Start()
    {
        // _pm = GetComponent<PlayerMovementFixed>();
        // _move = player.FindAction("Move");
        _playerAttackScript = GetComponent<playerAttackScript>();
        pm = GetComponent<PlayerMovementFixed>();

    }

    // Update is called once per frame
    void Update()
    {
        

            if (pm._movement.y < -0.7 && !_playerAttackScript.moveLag && !pm._isDashing)
            {

            //Vector3.Slerp(playerPos, new Vector3(playerPos.x, playerPos.y - 2f, playerPos.z), 0.01f);
            //(new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 0.5f, this.gameObject.transform.position.z));
                
                StartCoroutine(endLag(0.8f));

            }
            

    }

    public void DropThrough()
    {
        if (!_playerAttackScript.moveLag && !pm._isDashing)
        {
            StartCoroutine(endLag(0.8f));
        }
    }

    IEnumerator endLag(float endlag)
    {
        Physics.IgnoreLayerCollision(this.gameObject.layer, platformLayer, true);
        //_playerAttackScript.moveLag = true;
        yield return new WaitForSeconds(endlag);
        //_playerAttackScript.moveLag = false;
        Physics.IgnoreLayerCollision(this.gameObject.layer, platformLayer, false);
        
    }
}

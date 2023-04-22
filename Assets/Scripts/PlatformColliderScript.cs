using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlatformColliderScript : MonoBehaviour
{

    //public PlayerMovementFixed _player;
    [SerializeField] Vector3 _entryDirection = Vector3.up;
    [SerializeField] bool _localDirection = false;
    [SerializeField, Range(1.0f, 2.0f)] private float triggerScale = 1.25f;
    private new BoxCollider _collider = null;

    BoxCollider _collisionCheckTrigger = null;

    public PlayerMovementFixed _player;


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
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;


        _collisionCheckTrigger = gameObject.AddComponent<BoxCollider>();

        _collisionCheckTrigger.size = _collider.size * triggerScale;
        _collisionCheckTrigger.center = _collider.center;
        _collisionCheckTrigger.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {

        float depth = other.gameObject.transform.localScale.y;
        if (other.gameObject.tag.Equals("Player"))
            
           
        {

            if (Physics.ComputePenetration(
                _collisionCheckTrigger, transform.position, transform.rotation,
                other, other.transform.position, other.transform.rotation, out Vector3 collisionDirection, out float pDepth
                ))
            {

                Vector3 direction;
                if (_localDirection)
                {
                    direction = transform.TransformDirection(_entryDirection.normalized);

                }
                else
                {
                    direction = _entryDirection;
                }


                float dot = Vector3.Dot(direction, collisionDirection);
                print(dot + ", " + collisionDirection);
                //Opposite direction
                if (dot <= 0)
                {

                    //other.gameObject.GetComponent<PlayerMovementFixed>().isGrounded = true;

                    //if (_player._playerBase.transform.localPosition.y < transform.localPosition.y)
                    //{
                    
                    //Physics.IgnoreCollision(_collider, other, false);


                    //Physics.IgnoreCollision(_collider, other, false);


                    //}
                    //else {Physics.IgnoreCollision(_collider, other, false); }



                }
                /*else
                {
                    Physics.IgnoreCollision(_collider, other, true);
                }*/
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (Physics.ComputePenetration(
               _collisionCheckTrigger, transform.position, transform.rotation,
               other, other.transform.position, other.transform.rotation, out Vector3 collisionDirection, out float pDepth
               ))
        {

            Vector3 direction;
            if (_localDirection)
            {
                direction = transform.TransformDirection(_entryDirection.normalized);

            }
            else
            {
                direction = _entryDirection;
            }


            float dot = Vector3.Dot(direction, collisionDirection);
            print(dot + ", " + collisionDirection);
            //Opposite direction
            if (dot <= 0)
            {
                //if (_player._playerBase.transform.localPosition.y < transform.localPosition.y)
                //{

                //Physics.IgnoreCollision(_collider, other, false);


                //Physics.IgnoreCollision(_collider, other, false);


                //}
                //else {Physics.IgnoreCollision(_collider, other, false); }



                }
            }

        }

    private void OnDrawGizmosSelected()
    {
        Vector3 direction;
        if (_localDirection)
        {
            direction = transform.TransformDirection(_entryDirection.normalized);

        }
        else
        {
            direction = _entryDirection;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _entryDirection);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, -_entryDirection);
    }
}

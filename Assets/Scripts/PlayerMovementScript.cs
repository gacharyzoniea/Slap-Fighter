using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    Vector3 _movement;
    CharacterController _characterController;
    float _speed = 30;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }
    
    public void onMove(InputValue value)
    {
        _movement = value.Get<Vector3>();
    }

    

    // Update is called once per frame
    void Update()
    {
        _characterController.Move(_movement * _speed);
    }
}

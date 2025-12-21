using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{

    public CharacterController characterController;
    public Rigidbody body;
    private Vector2 _direction = Vector2.zero;
    
    void Start()
    {
        this.characterController = this.GetComponent<CharacterController>();
        
        if (InputController.Instance != null)
        {
            InputController.Instance.OnDirectionChange += SetDirection;
            InputController.Instance.OnDirectionStart += SetDirection;
            InputController.Instance.OnDirectionStop += SetDirection;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.characterController.Move(new Vector3(_direction.x, 0, _direction.y) * Time.deltaTime);
    }

    private void SetDirection(Vector2 dir)
    {
        this._direction = dir;
    }
}

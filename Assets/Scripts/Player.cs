using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{

    public float playerSpeed = 10.0f;
    
    public Animator animator;
    private CharacterController _characterController;
    
    private Vector3 _direction = Vector3.zero;
    private float _runThreshold = 0.1f;
    
    void Start()
    {
        this._characterController = this.GetComponent<CharacterController>();
        
        if (InputController.Instance != null)
        {
            InputController.Instance.OnDirectionChange += SetDirection;
            InputController.Instance.OnDirectionStart += SetDirection;
            InputController.Instance.OnDirectionStop += SetDirection;
        }
        
        if(this.animator == null)
            throw new Exception("Animator is not set");
    }

    // Update is called once per frame
    void Update()
    {
        this.animator.SetBool("isRunning", this._direction.magnitude > this._runThreshold);
        this._characterController.Move(_direction * (this.playerSpeed * Time.deltaTime));
        if(this._direction != Vector3.zero)
            this.animator.transform.rotation = Quaternion.LookRotation(this._direction);
    }

    private void SetDirection(Vector2 dir)
    {
        // direction from input is converted to 3D world.
        Vector3 newDir = new Vector3(dir.x, 0, dir.y);
        // apply camera orientation
        float cameraYRotate = GameController.Instance.camera.transform.rotation.eulerAngles.y;
        newDir = Quaternion.AngleAxis(cameraYRotate, Vector3.up) * newDir;

        this._direction = newDir;
    }
}

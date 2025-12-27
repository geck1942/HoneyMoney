using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Input controller listens to the controller joystick
/// to read movements and sends events for the GameController.
/// </summary>
public class InputController : BaseController<InputController>
{

    public InputActionReference moveAction;
    
    public delegate void DirectionEvent (Vector2 direction);
    public event DirectionEvent OnDirectionChange;
    public event DirectionEvent OnDirectionStart;
    public event DirectionEvent OnDirectionStop;

    private float moveFactor = 1f;
    private Vector2 lastDirection =  Vector2.zero;
    
    void Update()
    {
        // Read joystick input
        Vector2 direction = this.moveAction.action.ReadValue<Vector2>()
                        * this.moveFactor;
        
        if(lastDirection == Vector2.zero && direction != Vector2.zero)
        {
            this.OnDirectionStart?.Invoke(direction);
        }
        else if (lastDirection != Vector2.zero)
        {
            if(lastDirection != direction)
            {
                this.OnDirectionChange?.Invoke(direction);
            }
            else if(direction == Vector2.zero)
            {
                this.OnDirectionStop?.Invoke(direction);
            }
        }
        this.lastDirection = direction;
    }
}

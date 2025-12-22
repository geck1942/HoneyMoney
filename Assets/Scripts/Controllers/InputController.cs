using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : BaseController<InputController>
{
    /* Input controller listens to the controller joystick to read movements
     * and sends events for the GameController
     */
    
    public delegate void DirectionEvent (Vector2 direction);
    public event DirectionEvent OnDirectionChange;
    public event DirectionEvent OnDirectionStart;
    public event DirectionEvent OnDirectionStop;

    private float moveFactor = 1f;
    private Vector2 lastDirection =  Vector2.zero;
    
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        // Read joystick input
        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Vertical");
        
        Vector2 direction = new Vector2(moveX, moveY) * this.moveFactor;
        
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

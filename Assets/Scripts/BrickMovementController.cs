using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickMovementController : MonoBehaviour
{

    public enum brickState {
        stop,
        move
    }


    public brickState currentState;
    private bool hasMoved;
    void Start()
    {
        hasMoved = false;
        currentState = brickState.stop; 
    }

    void Update()
    {
        if (currentState== brickState.stop)
        {
            hasMoved = false;
        }
        if(currentState == brickState.move)
        {
            if (hasMoved == false) 
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - 1);
                currentState = brickState.stop;
                hasMoved = true;
            }
           

        }
    }
}

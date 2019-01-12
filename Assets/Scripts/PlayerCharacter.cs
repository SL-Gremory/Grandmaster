using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    // Start is called before the first frame update
    /*
    void Start()
    {
      
    }
    */


    protected override void Update()
    {
        // GetInput is solely a player function
        GetInput();

        // Execute's character's Update function
        base.Update();
    }


    private void GetInput()
    {
        direction = Vector2.zero;

        // GetKeyDown executed when you press a key
        // GetKey executed when you press and hold a key
        if (Input.GetKey(KeyCode.W)) {
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
        }

        // Change HP 
        if (Input.GetKeyDown(KeyCode.N))
        {
            hp -= 1;
            Debug.Log("HP = " + hp);
        }
    }
}

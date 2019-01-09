using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player and enemies inherit from this class
public abstract class Character : MonoBehaviour
{

    // Encapsulate variables
    [SerializeField]
    private float speed;
    protected Vector3 direction;
    protected int hp, mov, mp, atk, def, spr, spd, bst;

    // Start is called before the first frame update
    void Start()
    {
        hp = 10;
        mov = 5;
        mp = 10;
        atk = 1;
        def = 2;
        spr = 2;
        spd = 2;
        bst = (hp / 2) + mov + mp + atk + def + spr + spd;
        
    }

    // Update is called once per frame
    // Protected virtual overrides the parent's Update function
    protected virtual void Update()
    {
        Move();
    }

    public void Move()
    {
        // The transform is just the absolute position of the object
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void AnimateMovement(Vector3 direction) {

    }
}

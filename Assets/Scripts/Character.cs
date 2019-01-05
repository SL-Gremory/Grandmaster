using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player and enemies inherit from this class
public abstract class Character : MonoBehaviour
{

    // Encapsulate variables
    [SerializeField]
    private float speed;
    protected Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        
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

    public void AnimateMovement(Vector2 direction) {

    }
}

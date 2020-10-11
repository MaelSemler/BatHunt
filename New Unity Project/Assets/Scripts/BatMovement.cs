using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMovement : MonoBehaviour
{

    Rigidbody2D mRigidBody2D;
    
    void Awake()
    {
        
        // Must be done in Awake() because SetDirection() will be called early. Start() won't work.
        mRigidBody2D = GetComponent<Rigidbody2D>();

        float yVelocity = Random.Range(3, 7);
        float xVelocity = Random.Range(-3, 3);

        mRigidBody2D.velocity = new Vector2(xVelocity, yVelocity);
    }

    // Update is called once per frame
    void Update()
    {

        //Random variable

        float xPos = Random.Range(-3, 3);
        



        //Move on X axis

        if(this.transform.position.y < -1.88)
        {
           /* xPos = Random.Range(-3, 3);
            this.transform.position = new Vector2(xPos, -2);*/

            Vector2 currentVelocity = this.GetComponent<Rigidbody2D>().velocity;
            mRigidBody2D.velocity = new Vector2(currentVelocity.x, -currentVelocity.y);

        }

        if(this.transform.position.x < -3.64)
        {
            Vector2 currentVelocity = this.GetComponent<Rigidbody2D>().velocity;

            mRigidBody2D.velocity = new Vector2(-currentVelocity.x, currentVelocity.y);
        }

        if (this.transform.position.x > 3.64)
        {
            Vector2 currentVelocity = this.GetComponent<Rigidbody2D>().velocity;

            mRigidBody2D.velocity = new Vector2(-currentVelocity.x, currentVelocity.y);
        }

        if (this.transform.position.y > 1.87 && this.transform.position.y < 2)
        {
            Vector2 currentVelocity = this.GetComponent<Rigidbody2D>().velocity;

            mRigidBody2D.velocity = new Vector2(currentVelocity.x, -currentVelocity.y);
        }



    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("Click!!");
        }
    }

    
}

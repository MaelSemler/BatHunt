/*
 * Made By: Maël Semler
 * 40061228
 * Comp 376
 * Concordia Fall 2020
 *
 */

using UnityEngine;
using UnityEngine.UI;

public class BatMovement : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D mRigidBody2D;
    [SerializeField]
    Vector3 lastVelocity;
    [SerializeField]
    SpriteRenderer mSpriteRenderer;

    public float timeOnScreen;

    void Awake()
    {
        mRigidBody2D = GetComponent<Rigidbody2D>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();

        transform.position = new Vector3(-10f, -10f, 0f);

        lastVelocity = mRigidBody2D.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = mRigidBody2D.velocity;
    }

    //Collides with border, will change direction
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        //Flip the sprite if the bat changes direction
        if(lastVelocity.x > 0 && direction.x < 0)
        {
            flipSprite();
        }
        else if (lastVelocity.x < 0 && direction.x > 0)
        {
            flipSprite();
        }
       

        mRigidBody2D.velocity = direction * Mathf.Max(speed, 0f);
    }

    //Simple sprite flip depending on the current value
    public void flipSprite()
    {
        if (mSpriteRenderer.flipX) {
            mSpriteRenderer.flipX = false;
        }
        else
        {
            mSpriteRenderer.flipX = true;
        }
    }


}

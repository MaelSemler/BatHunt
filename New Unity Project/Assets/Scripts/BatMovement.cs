using UnityEngine;
using UnityEngine.UI;

public class BatMovement : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D mRigidBody2D;
    [SerializeField]
    Vector3 lastVelocity;


    public float timeOnScreen;

    void Awake()
    {
        mRigidBody2D = GetComponent<Rigidbody2D>();

        transform.position = new Vector3(-10f, -10f, 0f);

        lastVelocity = mRigidBody2D.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = mRigidBody2D.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        mRigidBody2D.velocity = direction * Mathf.Max(speed, 0f);
    }


}

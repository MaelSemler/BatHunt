using UnityEngine;
using UnityEngine.UI;

public class BatMovement : MonoBehaviour
{

    Rigidbody2D mRigidBody2D;
    [SerializeField]
    Vector3 lastVelocity;
    [SerializeField]

    public Text duckKilledText;
    public int duckKilled;

    void Awake()
    {
        mRigidBody2D = GetComponent<Rigidbody2D>();

        float xPosition = Random.Range(-2.5f, 2.5f);
        float yVelocity = Random.Range(1, 3);
        float xVelocity = Random.Range(-3, 3);

        transform.position = new Vector3(xPosition, -0.5f, 0f);

        mRigidBody2D.velocity = new Vector2(xVelocity, yVelocity);
        lastVelocity = mRigidBody2D.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = mRigidBody2D.velocity;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
           // Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        mRigidBody2D.velocity = direction * Mathf.Max(speed, 0f);
    }


}

using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public int score = 0;
    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var myCrossHair = GameObject.Find("Crosshair");

        myCrossHair.transform.position = new Vector3(position.x, position.y, 0);

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos2D = new Vector2(position.x, position.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, -Vector2.up);

            if (hit.collider != null)
            {
                if(hit.collider.gameObject.name == "Bat")
                {
                    score += 3;
                    Destroy(hit.collider.gameObject);
                }
                else if(hit.collider.gameObject.name == "Witch")
                {
                    hit.collider.gameObject.GetComponent<WitchMovement>().hitPoints--;
                    if(hit.collider.gameObject.GetComponent<WitchMovement>().hitPoints == 0)
                    {
                        score += 5;
                        Destroy(hit.collider.gameObject);
                        GameObject.Find("GameManager").GetComponent<Game>().witchSent = false;
                    }
                }
                else
                {
                    score -= 1;
                }
                scoreText.text = score.ToString();
            }
        }
    }




}

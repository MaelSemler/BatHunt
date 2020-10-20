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

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, -Vector2.up, 50f);


            if (hits[0].collider != null)
            {
                if(hits[0].collider.gameObject.name == "Bat")
                {
                    if(hits[1].collider != null && hits[1].collider.gameObject.name == "Bat")
                    {
                        //2 kills for 3 points each and then extra 5
                        score += 11;
                        Destroy(hits[0].collider.gameObject);
                        Destroy(hits[1].collider.gameObject);
                        GameObject.Find("GameManager").GetComponent<Game>().batKilledThisRound += 2;
                    }
                    else
                    {
                        score += 3;
                        Destroy(hits[0].collider.gameObject);
                        GameObject.Find("GameManager").GetComponent<Game>().batKilledThisRound++;
                    }

                }
                else if(hits[0].collider.gameObject.name == "Witch")
                {
                    hits[0].collider.gameObject.GetComponent<WitchMovement>().hitPoints--;
                    if(hits[0].collider.gameObject.GetComponent<WitchMovement>().hitPoints == 0)
                    {
                        score += 5;
                        Destroy(hits[0].collider.gameObject);
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

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

        myCrossHair.transform.position = new Vector3(position.x, position.y, -7.0f);

        if (!GameObject.Find("GameManager").GetComponent<Game>().normalVersion && Input.GetMouseButton(0))
        {
            checkHit(position);
        }
        else if (GameObject.Find("GameManager").GetComponent<Game>().normalVersion && Input.GetMouseButtonDown(0))
        {
            checkHit(position);
        }

    }

    void checkHit(Vector3 position)
    {
        Vector2 mousePos = new Vector3(position.x, position.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, -Vector2.up, 50f);
       
        /* Debug.DrawRay(mousePos, Vector3.forward, Color.green);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            print("hit " + hit);
        }*/


        if (hits[0].collider != null)
        {
            if (hits[0].collider.gameObject.layer == 8)
            {
                GetComponent<AudioSource>().Play();
                if (hits[1].collider != null && hits[1].collider.gameObject.layer == 8)
                {
                    //2 kills for 3 points each and then extra 5
                    score += 11;
                    Destroy(hits[0].collider.gameObject);
                    Destroy(hits[1].collider.gameObject);
                    GameObject.Find("GameManager").GetComponent<Game>().batKilledThisRound += 2;
                    GameObject.Find("GameManager").GetComponent<Game>().numberOfBatAlive -= 2;
                    GameObject.Find("GameManager").GetComponent<Game>().batCount += 2;
                }
                else
                {
                    if (hits[0].collider.gameObject.name == "Bat2")
                    {
                        score += 5;
                    }
                    else
                    {
                        score += 3;
                    }

                    Destroy(hits[0].collider.gameObject);
                    GameObject.Find("GameManager").GetComponent<Game>().batKilledThisRound++;
                    GameObject.Find("GameManager").GetComponent<Game>().batCount++;
                    GameObject.Find("GameManager").GetComponent<Game>().numberOfBatAlive--;
                    
                    
                }

            }
            else if (hits[0].collider.gameObject.name == "Witch")
            {
                GetComponent<AudioSource>().Play();
                hits[0].collider.gameObject.GetComponent<WitchMovement>().hitPoints--;
                if (hits[0].collider.gameObject.GetComponent<WitchMovement>().hitPoints == 0)
                {
                    score += 5;
                    Destroy(hits[0].collider.gameObject);
                    GameObject.Find("GameManager").GetComponent<Game>().witchSent = false;
                }
            }
            else if(GameObject.Find("GameManager").GetComponent<Game>().normalVersion)
            {
                score -= 1;
                GameObject.Find("Dog").GetComponent<Animator>().SetTrigger("Show");
                GameObject.Find("Dog").GetComponent<AudioSource>().Play();
                print("Should Animate");
            }
            scoreText.text = score.ToString();
        }
    }




}

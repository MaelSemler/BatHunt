/*
 * Made By: Maël Semler
 * 40061228
 * Comp 376
 * Concordia Fall 2020
 *
 */

using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public int score = 0;
    public Text scoreText;
    public GameObject blood;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Move the crosshair to follow the mouse cursor
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var myCrossHair = GameObject.Find("Crosshair");

        myCrossHair.transform.position = new Vector3(position.x, position.y, -7.0f);

        //Check if the user clicked, for special mode is button
        if (!GameObject.Find("GameManager").GetComponent<Game>().normalVersion && Input.GetMouseButton(0))
        {
            checkHit(position);
        }
        else if (GameObject.Find("GameManager").GetComponent<Game>().normalVersion && Input.GetMouseButtonDown(0))
        {
            checkHit(position);
        }

    }

    //Check ith Raycasting if the mouse position is above a bat, 2 bats or a witch
    void checkHit(Vector3 position)
    {
        Vector2 mousePos = new Vector3(position.x, position.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, -Vector2.up, 50f);

        //This means it hit something
        if (hits[0].collider != null)
        {
            //This means it hit a bat
            if (hits[0].collider.gameObject.layer == 8)
            {
                GetComponent<AudioSource>().Play();
                Instantiate(blood, position, Quaternion.identity);
                //Check if a 2nd bat was hit
                if (hits[1].collider != null && hits[1].collider.gameObject.layer == 8)
                {
                    //2 kills for 3 points each and then extra 5
                    score += 11;
                    Destroy(hits[0].collider.gameObject);
                    Destroy(hits[1].collider.gameObject);
                    GameObject.Find("GameManager").GetComponent<Game>().batKilledThisRound += 2;
                    GameObject.Find("GameManager").GetComponent<Game>().numberOfBatAlive -= 2;
                    GameObject.Find("GameManager").GetComponent<Game>().batCount += 2;
                    if (GameObject.Find("GameManager").GetComponent<Game>().numberOfBatAlive == 0)
                    {
                        GameObject.Find("GameManager").GetComponent<Game>().shouldSend = true;
                    }
                }
                else
                {
                    //Check the type of the bat, red bats gives more points
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
                    if (GameObject.Find("GameManager").GetComponent<Game>().numberOfBatAlive == 0)
                    {
                        GameObject.Find("GameManager").GetComponent<Game>().shouldSend = true;
                    }

                }

            }
            else if (hits[0].collider.gameObject.name == "Witch")
            {
                //Deletes the witch, play sound, blood, add score 
                Instantiate(blood, position, Quaternion.identity);
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
                //Only remove points in the normal version
                score -= 1;
                GameObject.Find("Dog").GetComponent<Animator>().SetTrigger("Show");
                GameObject.Find("Dog").GetComponent<AudioSource>().Play();
            }
            scoreText.text = score.ToString();
        }
    }




}

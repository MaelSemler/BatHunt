using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public int batCount;
    public Text batCountText;
    GameObject originalObject;
    public int difficulty = 1;
    public int roundNumber = 1;
    public Text roundNumberText;
    private bool  alreadySent = true;
    GameObject[] bats = new GameObject[10];

    void Start()
    {
        generateNewBats();
        sendBats(0);
    }

    // Update is called once per frame
    void Update()
    {

        batCount = 11 - originalObject.transform.parent.childCount;
        batCountText.text = batCount.ToString();
        if (batCount == 10)
        {
            setNextLevel();
        }
        else if (batCount % 2 == 0 & !alreadySent)
        {
            sendBats(batCount);
        }

        if (batCount % 2 == 1 & alreadySent)
        {
            alreadySent = false;
        }
        
    }

    void generateNewBats()
    {
        originalObject = GameObject.Find("OGBat");

        for (int i = 0; i < 10; i++)
        {
            bats[i] = Instantiate(originalObject, new Vector3(0, 0, 0), Quaternion.identity);
            bats[i].transform.parent = originalObject.transform.parent;
            bats[i].name = "Bat";
            bats[i].transform.position = new Vector2(-5f, -5f);
            bats[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }

        originalObject.transform.position = new Vector2(-5f, -5f);
    }

    void sendBats(int batNumber)
    {
        float xPosition = Random.Range(-2.5f, 2.5f);
        float xVelocity = Random.Range(-3, 3);
        float yVelocity = Random.Range(1, 3);

        bats[batNumber].transform.position = new Vector2(xPosition, -0.7f);
        bats[batNumber].GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity, yVelocity);
        StartCoroutine(waitToSendNextBat(batNumber + 1));
       
    }

    IEnumerator waitToSendNextBat(int batNumber)
    {
        float xPosition = Random.Range(-2.5f, 2.5f);
        float xVelocity = Random.Range(-3, 3);
        float yVelocity = Random.Range(1, 3);

        float waitTime = Random.Range(0.1f, 1.5f);
        yield return new WaitForSeconds(waitTime);

        bats[batNumber].transform.position = new Vector2(xPosition, -0.8f);
        bats[batNumber].GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity, yVelocity);
        alreadySent = true;
    }

    void setNextLevel() {
        difficulty++;
        roundNumber++;
        roundNumberText.text = roundNumber.ToString();
        generateNewBats();
        sendBats(0);

    }


}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public int batCount;
    public Text batCountText;
    GameObject originalObject;
    public float difficulty = 0;
    public int roundNumber = 1;
    public Text roundNumberText;
    private bool  alreadySent = true;
    GameObject[] bats = new GameObject[10];
    private int numberOfWitchLeft;
    public bool witchSent;

    [SerializeField]
    public bool normalVersion = true;

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
            alreadySent = true;
            sendBats(batCount);
            if(numberOfWitchLeft > 0 && !witchSent)
            {
                print("Should send witch");
                StartCoroutine(sendWitch());
                numberOfWitchLeft--;
            }
        }
        else
        {
            checkBatTimeAlive(batCount);
        }

        if (batCount % 2 == 1 & alreadySent)
        {
            alreadySent = false;
        }

        if (Input.GetKeyDown("space"))
        {
            normalVersion = normalVersion ? false : true;
            print("space key was pressed " + normalVersion);
        }
    }

    //Check If the bat has been alive for more than 5 seconds, if yes the bat will disapear.
    void checkBatTimeAlive(int batNumber)
    {
        if (bats[batNumber] && Time.time - bats[batNumber].GetComponent<BatMovement>().timeOnScreen  >= 5f)
        {
            print(batNumber);
            Destroy(bats[batNumber]);
        }
        if (witchSent)
        {
            GameObject currentWitch = GameObject.Find("Witch");
            if(currentWitch && Time.time - currentWitch.GetComponent<WitchMovement>().timeOnScreen > 2.5f)
            {
                Destroy(currentWitch);
                witchSent = false;
            }
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

        numberOfWitchLeft = 2;

     
    }

    void sendBats(int batNumber)
    {
        float xPosition = Random.Range(-2.5f, 2.5f);
        float xVelocity = Random.Range(-1 - difficulty, 1 + difficulty);
        float yVelocity = Random.Range(difficulty, 1 + difficulty);

        bats[batNumber].transform.position = new Vector2(xPosition, -0.7f);
        bats[batNumber].GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity, yVelocity);
        bats[batNumber].GetComponent<BatMovement>().timeOnScreen = Time.time;
       
        StartCoroutine(waitToSendNextBat(batNumber + 1));
       
    }

    IEnumerator sendWitch()
    {
        float waitTime = Random.Range(0.5f, 3f);
        witchSent = true;
        yield return new WaitForSeconds(waitTime);

        float xPosition = Random.Range(-2.5f, 2.5f);
        float xVelocity = Random.Range(-1 - difficulty, 1 + difficulty);
        float yVelocity = Random.Range(1, 3);

        GameObject ogWitch = GameObject.Find("WitchOG");
        GameObject witch = Instantiate(ogWitch, new Vector3(0, 0, 0), Quaternion.identity);
        witch.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        witch.name = "Witch";

        witch.transform.position = new Vector2(xPosition, -0.5f);
        witch.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity, yVelocity);
        witch.GetComponent<WitchMovement>().timeOnScreen = Time.time;
        
    }

    IEnumerator waitToSendNextBat(int batNumber)
    {
       
       
        float waitTime = Random.Range(0.1f, 1.5f);
        yield return new WaitForSeconds(waitTime);

        float xPosition = Random.Range(-2.5f, 2.5f);
        float xVelocity = Random.Range(-1 - difficulty, 1 + difficulty);
        float yVelocity = Random.Range(difficulty, 1 + difficulty);
        if (bats[batNumber])
        {
            bats[batNumber].transform.position = new Vector2(xPosition, -0.7f);
            bats[batNumber].GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity, yVelocity);
            bats[batNumber].GetComponent<BatMovement>().timeOnScreen = Time.time;
        }
        

    }

    void setNextLevel() {
        difficulty += 0.25f;
        roundNumber++;
        roundNumberText.text = roundNumber.ToString();
        numberOfWitchLeft = 2;
        generateNewBats();
        sendBats(0);

    }


}

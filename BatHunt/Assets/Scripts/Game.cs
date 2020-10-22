/*
 * Made By: Maël Semler
 * 40061228
 * Comp 376
 * Concordia Fall 2020
 *
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public int batCount;
    [SerializeField]
    public Text batKilledRoundText;
    [SerializeField]
    public Text minimumBatKilled;
    [SerializeField]
    public Text newLevelText;
    [SerializeField]
    public Text resetText;
    [SerializeField]
    public Text specialLeftText;
    GameObject originalObject;
    public float difficulty = 0;
    public int roundNumber = 1;
    [SerializeField]
    public Text roundNumberText;

    [SerializeField]
    private int numberOfSpecialModes;

    [SerializeField]
    private int numberOfBatsPerLevel;
    [SerializeField]
    private int numberOfBatsPerWave;

    GameObject[] bats;
    [SerializeField]
    private int numberOfWitchLeft;
    public bool witchSent = false;
    public int numberOfBatAlive;

    public int batKilledThisRound;
    [SerializeField]
    public int minimumBatRequired;

    public bool normalVersion = true;
    public bool shouldSend = true;

    void Start()
    {
        //Initiate many variables
        batKilledThisRound = 0;
        resetText.text = "";
        originalObject = GameObject.Find("OGBat");
        bats = new GameObject[(numberOfBatsPerWave*2)+1];
        batCount = 0;
        numberOfBatAlive = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Set the number of bat killed
        batKilledRoundText.text = batKilledThisRound.ToString();

        //Check if the number of bats have been killed
        if (batCount >= numberOfBatsPerLevel && normalVersion)
        {
            print("Set New level");
            batCount = 0;
            StartCoroutine(pauseBetweenLevel());
        }
        //Check if the wave is finished
        else if (numberOfBatAlive == 0 && shouldSend)
        {
            print("BAts: " + bats.Length);
            shouldSend = false;
            sendBats();            
        }
        else
        {
           checkBatTimeAlive();
        }

        //Check if it should send a witch
        if (numberOfWitchLeft > 0 && !witchSent)
        {
            StartCoroutine(sendWitch());
            numberOfWitchLeft--;
        }

        //Check if spacebar has been pressed and check if special modes are left
        if (Input.GetKeyDown("space") && normalVersion && numberOfSpecialModes != 0)
        {
            numberOfSpecialModes--;
            specialLeftText.text = numberOfSpecialModes.ToString();
            StartCoroutine(specialMode());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown("r"))
        {
            resetGame();
        }
    }

    //Check If the bat has been alive for more than 5 seconds, if yes the bat will disapear.
    void checkBatTimeAlive()
    {
        //Goes through the array of bats and delete them if they have been alive for more than 5 seconds
        for (int i = 0; i < numberOfBatsPerWave; i++)
        {
            if (bats[i] && (Time.time - bats[i].GetComponent<BatMovement>().timeOnScreen) >= 5f)
            {
                batCount++;
                numberOfBatAlive--;
                Destroy(bats[i]);
            }
        }
        
        //Check if the witch has been alive for more than 2.5 seconds
        if (witchSent)
        {
            GameObject currentWitch = GameObject.Find("Witch");
            if(currentWitch && Time.time - currentWitch.GetComponent<WitchMovement>().timeOnScreen > 2.5f)
            {
                Destroy(currentWitch);
                witchSent = false;
            }
        }
        if (numberOfBatAlive == 0)
        {
            shouldSend = true;
        }
    }

    //Start a Coroutine for each bat
    void sendBats()
    {
        if (normalVersion)
        {
            for (int i = 0; i < numberOfBatsPerWave; i++)
            {
                numberOfBatAlive++;
                StartCoroutine(waitToSendNextBat(i));
            }
        }
        else
        {
            for (int i = 0; i < numberOfBatsPerWave*2; i++)
            {
                StartCoroutine(waitToSendNextBat(i));
            }
        }
        
    }

    //Wait X amount of time to send a bat, with random position and velocity
    IEnumerator waitToSendNextBat(int batNumber)
    {
        float waitTime = Random.Range(0.1f, 1.5f);
        yield return new WaitForSeconds(waitTime);

        float xPosition = Random.Range(-2.5f, 2.5f);
        float xVelocity = Random.Range(-1 - difficulty, 1 + difficulty);
        float yVelocity = Random.Range(difficulty, 1 + difficulty);

        string nameToGive = "";

        //Decides if its a special red bat or not
        if(Random.Range(0,8) == 0)
        {
            originalObject = GameObject.Find("OGBat2");
            nameToGive = "Bat2";
        }
        else
        {
            originalObject = GameObject.Find("OGBat");
            nameToGive = "Bat1";
        }

        if(bats[batNumber] != null)
        {
            
        }
        else
        {
            bats[batNumber] = Instantiate(originalObject, new Vector3(0, 0, 0), Quaternion.identity);
            bats[batNumber].transform.parent = originalObject.transform.parent;
            bats[batNumber].name = nameToGive;
            bats[batNumber].transform.position = new Vector2(xPosition, -1.0f);
            bats[batNumber].GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity, yVelocity);
            bats[batNumber].GetComponent<BatMovement>().timeOnScreen = Time.time;

            //Check the direciton they are going, and flip the sprite if required
            if (xVelocity < 0)
            {
                bats[batNumber].GetComponent<BatMovement>().flipSprite();
            }
        }
        


    }

    //Instantiate a witch on the screen with random position and velocity
    IEnumerator sendWitch()
    {
        float waitTime = Random.Range(0.5f, 3f);
        witchSent = true;
        yield return new WaitForSeconds(waitTime);

        float xPosition = Random.Range(-2.5f, 2.5f);
        float xVelocity = Random.Range(-1 - difficulty, 1 + difficulty);
        float yVelocity = Random.Range(difficulty, 1 + difficulty);

        GameObject ogWitch = GameObject.Find("WitchOG");
        GameObject witch = Instantiate(ogWitch, new Vector3(0, 0, 0), Quaternion.identity);
        witch.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        witch.name = "Witch";

        witch.transform.position = new Vector2(xPosition, -1.0f);
        witch.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity, yVelocity);
        witch.GetComponent<WitchMovement>().timeOnScreen = Time.time;
        witch.GetComponent<AudioSource>().Play();

        //Check the direciton they are going, and flip the sprite if required
        if (xVelocity < 0)
        {
            witch.GetComponent<WitchMovement>().flipSprite();
        }

    }

    //Reset some variables and increase difficulty for next round
    void setNextLevel() {
        difficulty += 0.25f;

        if ((difficulty * 4) % 4 == 0  && minimumBatRequired < numberOfBatsPerLevel)
        {
            minimumBatRequired++;
            minimumBatKilled.text = minimumBatRequired.ToString();
        }

        roundNumber++;
        roundNumberText.text = roundNumber.ToString();
        numberOfWitchLeft = 2;
        batKilledThisRound = 0;
        numberOfBatAlive = 0;
        batCount = 0;

    }

    //Pause for the Next Level screen
    IEnumerator pauseBetweenLevel()
    {
        //If the player hasnt killed enough bats, the game over screen will appear
        if (batKilledThisRound < minimumBatRequired)
        {
            print("You did not killed enough bats this round. Game Over");
            Time.timeScale = 0;
            newLevelText.text = "Game Over";
            resetText.text = "Press R to Reset";
            yield return new WaitForSecondsRealtime(1.0f);
        }
        else
        {
            //Make sure that all the witches and bats are killed before the start of next round
            killWitches();
            batCount = 0;
            Time.timeScale = 0;
            newLevelText.text = "New Level";
            yield return new WaitForSecondsRealtime(2.0f);
            newLevelText.text = "";
            Time.timeScale = 1;

            setNextLevel();
        }
        
    }

    //Sets special mode
    IEnumerator specialMode()
    {
        normalVersion = normalVersion ? false : true;
        print("Special Mode Activated");
        numberOfBatAlive += numberOfBatsPerWave;
        print(numberOfBatAlive);
        sendBats();
        GetComponent<AudioSource>().Play();
        yield return new WaitForSecondsRealtime(5.0f);
        normalVersion = normalVersion ? false : true;
        print("Special Mode Finished");
        killWitches();
        numberOfBatAlive = 0;

    }

    //Kill all characters on screen
    void killWitches()
    {
        numberOfBatAlive = 0;
        if (witchSent)
        {
            GameObject currentWitch = GameObject.Find("Witch");
            if (currentWitch)
            {
                Destroy(currentWitch);
                witchSent = false;
            }
        }
        for (int i = 0; i < numberOfBatsPerWave * 2; i++)
        {
            if (bats[i])
            {
                numberOfBatAlive--;
                Destroy(bats[i]);
            }
        }
    }

    void resetGame()
    {
        Time.timeScale = 1;
        difficulty = 0.5f;
        roundNumber = 0;
        resetText.text = "";
        numberOfSpecialModes = 2;
        specialLeftText.text = "2";
        newLevelText.text = "";
        GameObject.Find("Crosshair").GetComponent<Crosshair>().score = 0;
        GameObject.Find("Crosshair").GetComponent<Crosshair>().scoreText.text = "0";
        setNextLevel();
    }


    }

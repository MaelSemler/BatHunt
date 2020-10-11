using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var myCrossHair = GameObject.Find("Crosshair");

        myCrossHair.transform.position = new Vector3(position.x, position.y, 0);
        
    }
}

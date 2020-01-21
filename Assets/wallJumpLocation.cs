using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallJumpLocation : MonoBehaviour
{
    public GameObject Player;
    public GameObject endMarker;
    public GameObject endMarker2;
    private int position = 1;
    public Vector3 flip;


    // Start is called before the first frame update
    void Start()
    {
        //PlayerScript = Player.GetComponent<PlayerController>();
        Player = GameObject.Find("Player");
        endMarker = GameObject.Find("endMarker");

       // Vector3 flip = new Vector3(-5.7f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 flip = new Vector3(-5.7f, 0, 0);

        if (Player.GetComponent<PlayerController>().isTouchingWall == true)
        {
            endMarker.transform.position = endMarker2.transform.position;
            //endMarker.transform.position.x *= -1;
        }

    }
}

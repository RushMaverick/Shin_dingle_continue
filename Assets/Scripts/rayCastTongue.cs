using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rayCastTongue : MonoBehaviour
{
    public LineRenderer line;
    public TongueCollision script;
    Collider2D tongue_collider;
    Collider2D tongue_collider2;
    Collider2D tongue_collider3;
    SpringJoint2D joint;
    public Rigidbody2D rb;
    public float lickRate = 0.3f;
    public float tongueRange = 25f;
    public GameObject tongueTip;
    public GameObject Tongue;
    public GameObject Tongue_2;
    public GameObject Tongue_3;
    public GameObject Player;
    public GameObject Point_Up;
    public GameObject Point_45;
    public GameObject Point_Forward;
    public GameObject target;

    Vector2 T_Up;
    Vector2 T_Forward;
    Vector2 T_45;
    Vector2 SlingForce;

    private float tongue;
    private bool lickHeld;


    void Awake()
    {

        joint = GetComponent<SpringJoint2D>();
        tongue_collider = Tongue.GetComponent<Collider2D>();
        tongue_collider2 = Tongue_2.GetComponent<Collider2D>();
        tongue_collider3 = Tongue_3.GetComponent<Collider2D>();

        tongueTip = GameObject.Find("tongueTip");
        Point_Up = GameObject.Find("Point_Up");
        Point_Forward = GameObject.Find("Point_Forward");
        Point_45 = GameObject.Find("Point_45");
        Tongue = GameObject.Find("Tongue");
        Tongue_2 = GameObject.Find("Tongue_2");
        Tongue_3 = GameObject.Find("Tongue_3");
        Player = GameObject.Find("Player");
        target = GameObject.Find("Look");

    }


    void Start()
    {
        tongue_collider.enabled = false;
        tongue_collider2.enabled = false;
        tongue_collider3.enabled = false;
        rb = Player.GetComponent<Rigidbody2D>();

        lickHeld = false;
    }

    // Update is called once per frame
    private void Update()
    {

        T_Up = new Vector2(Point_Up.transform.position.x, Point_Up.transform.position.y);
        T_Forward = new Vector2(Point_Forward.transform.position.x, Point_Forward.transform.position.y);
        T_45 = new Vector2(Point_45.transform.position.x, Point_45.transform.position.y);

        // Plain L
        if (Input.GetButtonDown("Lick") && lickHeld == false && Input.GetAxis("Vertical") == 0 && Tongue.GetComponent<TongueCollision>().swinging == false && Tongue_2.GetComponent<TongueCollision>().swinging == false && Tongue_3.GetComponent<TongueCollision>().swinging == false)
        {
            StartCoroutine(ButtonTimer());

            //Malin prövar saker

            //tongueTip.transform.position = T_Forward;
            //target.transform.position = T_Forward;
            //tongueTip.transform.position = Point_Forward.transform.position;
            //target.transform.position = Point_Forward.transform.position;
            //StartCoroutine(Tongue1());
            Player.GetComponent<PlayerController>().isLicking0 = true;
        }

        //45 degree L
        if (Input.GetButtonDown("Lick") && lickHeld == false && Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0 && Tongue.GetComponent<TongueCollision>().swinging == false && Tongue_2.GetComponent<TongueCollision>().swinging == false && Tongue_3.GetComponent<TongueCollision>().swinging == false)
        {
            StartCoroutine(ButtonTimer());

            //Malin prövar saker
            //tongueTip.transform.position = T_45;
            //target.transform.position = T_45;
            //tongueTip.transform.position = Point_45.transform.position;
            //target.transform.position = Point_45.transform.position;
            //StartCoroutine(Tongue2());
            Player.GetComponent<PlayerController>().isLicking45 = true;

        }

        //90 Degree L 
        if (Input.GetButtonDown("Lick") && lickHeld == false && Input.GetAxis("Vertical") == 1 && Input.GetAxis("Horizontal") == 0 && Tongue.GetComponent<TongueCollision>().swinging == false && Tongue_2.GetComponent<TongueCollision>().swinging == false && Tongue_3.GetComponent<TongueCollision>().swinging == false)
        {
            StartCoroutine(ButtonTimer());

            //Malin prövar saker
            //tongueTip.transform.position = T_Up;
            //target.transform.position = T_Up;
            //tongueTip.transform.position = Point_Up.transform.position;
            //target.transform.position = Point_Up.transform.position;

            //StartCoroutine(Tongue3());
            Player.GetComponent<PlayerController>().isLicking90 = true;
        }

        //Malin prövar saker

        // Plain L hitbox
        if (Player.GetComponent<PlayerController>().isLicking0 == true)
        {

            Player.GetComponent<PlayerController>().isLicking45 = false;
            Player.GetComponent<PlayerController>().isLicking90 = false;
            tongueTip.transform.position = Point_Forward.transform.position;
            target.transform.position = Point_Forward.transform.position;

            if (tongue_collider2.enabled == true || tongue_collider3.enabled == true)
            {
                tongue_collider2.enabled = false;
                tongue_collider3.enabled = false;
            }

            tongue_collider.enabled = true;
            tongueTip.SetActive(true);
        }

        if (Player.GetComponent<PlayerController>().isLicking0 == false)
        {
            tongue_collider.enabled = false;
            tongueTip.SetActive(false);
        }

        //45 degree L hitbox

        if (Player.GetComponent<PlayerController>().isLicking45 == true)
        {

            Player.GetComponent<PlayerController>().isLicking0 = false;
            Player.GetComponent<PlayerController>().isLicking90 = false;
            tongueTip.transform.position = Point_45.transform.position;
            target.transform.position = Point_45.transform.position;

            if (tongue_collider.enabled == true || tongue_collider3.enabled == true)
            {
                tongue_collider.enabled = false;
                tongue_collider3.enabled = false;
            }

            tongue_collider2.enabled = true;
            tongueTip.SetActive(true);
        }

        if (Player.GetComponent<PlayerController>().isLicking45 == false)
        {
            tongue_collider2.enabled = false;
            tongueTip.SetActive(false);
        }

        //90 degree L hitbox

        if (Player.GetComponent<PlayerController>().isLicking90 == true)
        {

            Player.GetComponent<PlayerController>().isLicking0 = false;
            Player.GetComponent<PlayerController>().isLicking45 = false;
            tongueTip.transform.position = Point_Up.transform.position;
            target.transform.position = Point_Up.transform.position;

            if (tongue_collider.enabled == true || tongue_collider2.enabled == true)
            {
                tongue_collider.enabled = false;
                tongue_collider2.enabled = false;
            }

            tongue_collider3.enabled = true;
            tongueTip.SetActive(true);
        }

        if (Player.GetComponent<PlayerController>().isLicking90 == false)
        {
            tongue_collider3.enabled = false;
            tongueTip.SetActive(false);
        }
        //end of malin prövar saker

        if (Input.GetButtonUp("Lick"))
        {
            lickHeld = false;
        }

        if (Input.GetButton("Lick") && lickHeld == true)
        {
            joint.distance -= 5f * Time.deltaTime;
            //DingleSling();
            Debug.Log("Slingshot");
        }

        if (Tongue.GetComponent<TongueCollision>().swinging == true && Tongue_2.GetComponent<TongueCollision>().swinging == true && Tongue_3.GetComponent<TongueCollision>().swinging == true)
        {
            lickHeld = false;
            Debug.Log("Lickheld is false");
        }




        //If player presses space while swinging, all colliders for the tongue are disabled as well as the line and joint. Sets swinging to false.
        if (Input.GetButtonDown("Jump") && Tongue.GetComponent<TongueCollision>().swinging == true || Input.GetButtonDown("Jump") && Tongue_2.GetComponent<TongueCollision>().swinging == true || Input.GetButtonDown("Jump") && Tongue_3.GetComponent<TongueCollision>().swinging == true)
        {

            tongue_collider3.enabled = false;
            tongue_collider2.enabled = false;
            tongue_collider.enabled = false;

            joint.enabled = false;
            line.enabled = false;

            rb.rotation = 0f;

            if (Player.GetComponent<PlayerController>().movementInputDirection == 1)
            {
                Vector2 SwingJumpForce = new Vector2(25.0f, 5.0f);
                rb.AddForce(SwingJumpForce, ForceMode2D.Impulse);
            }

            if (Player.GetComponent<PlayerController>().movementInputDirection == -1)
            {
                Vector2 SwingJumpForce = new Vector2(-25.0f, 5.0f);
                rb.AddForce(SwingJumpForce, ForceMode2D.Impulse);
            }

            Tongue.GetComponent<TongueCollision>().swinging = false;
            Tongue_2.GetComponent<TongueCollision>().swinging = false;
            Tongue_3.GetComponent<TongueCollision>().swinging = false;
        }
    }
    void FixedUpdate()
    {

        if (joint.enabled == true)
        {
            line.SetPosition(0, transform.position);
        }

        //if (joint.enabled == true && Player.GetComponent<PlayerController>().isGrounded == false)
        //{
        //   transform.up = target.transform.position - transform.position;
        //}

        if (joint.enabled == true && Player.GetComponent<PlayerController>().isGrounded == false)
        {
            transform.up = target.transform.position - transform.position;
        }

    }

    public IEnumerator ButtonTimer()
    {
        yield return new WaitForSeconds(1f);

        if (Input.GetButton("Lick"))
        {
            lickHeld = true;
            Debug.Log("Lick");
        }

    }

    public void DingleSling()
    {
        if (Tongue.GetComponent<TongueCollision>().swinging == true || Tongue_2.GetComponent<TongueCollision>().swinging == true || Tongue_3.GetComponent<TongueCollision>().swinging == true)
        {

            Vector2 SlingForce = new Vector2(0f, 20f);
            rb.AddForce(transform.up * SlingForce, ForceMode2D.Impulse);
            Debug.Log("Heynow");
        }
    }


    public IEnumerator Tongue1()
    {
        if (tongue_collider2.enabled == true || tongue_collider3.enabled == true)
        {
            tongue_collider2.enabled = false;
            tongue_collider3.enabled = false;
        }

        tongue_collider.enabled = true;
        tongueTip.SetActive(true);

        //Plays animation, animation stops in PlayerController
        Player.GetComponent<PlayerController>().isLicking0 = true;
        Player.GetComponent<PlayerController>().isLicking45 = false;
        Player.GetComponent<PlayerController>().isLicking90 = false;

        yield return new WaitForSeconds(0.10f); // waits 0.3 seconds


        tongue_collider.enabled = false;
        tongueTip.SetActive(false);
    }

    public IEnumerator Tongue2()
    {
        if (tongue_collider.enabled == true || tongue_collider3.enabled == true)
        {
            tongue_collider.enabled = false;
            tongue_collider3.enabled = false;
        }

        tongue_collider2.enabled = true;
        tongueTip.SetActive(true);

        //Plays animation, animation stops in PlayerController
        Player.GetComponent<PlayerController>().isLicking45 = true;
        Player.GetComponent<PlayerController>().isLicking0 = false;
        Player.GetComponent<PlayerController>().isLicking90 = false;

        yield return new WaitForSeconds(0.10f); // waits 0.3 seconds


        tongue_collider2.enabled = false;
        tongueTip.SetActive(false);
    }

    public IEnumerator Tongue3()
    {
        if (tongue_collider2.enabled == true || tongue_collider.enabled == true)
        {
            tongue_collider2.enabled = false;
            tongue_collider.enabled = false;
        }
        tongue_collider3.enabled = true;
        tongueTip.SetActive(true);

        //Plays animation, animation stops in PlayerController
        Player.GetComponent<PlayerController>().isLicking90 = true;
        Player.GetComponent<PlayerController>().isLicking45 = false;
        Player.GetComponent<PlayerController>().isLicking0 = false;

        yield return new WaitForSeconds(0.10f); // waits 0.3 seconds


        tongue_collider3.enabled = false;
        tongueTip.SetActive(false);
    }


}


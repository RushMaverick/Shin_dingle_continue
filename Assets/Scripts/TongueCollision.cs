using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueCollision : MonoBehaviour
{

    public LineRenderer line;
    Collider2D tongue_collider;
    Collider2D tongue_collider2;
    Collider2D tongue_collider3;
    public SpringJoint2D joint;
    public float tongueRange = 5f;
    float tongueTimer;
    public GameObject tongueTip;
    public GameObject Tongue;
    public GameObject Tongue_2;
    public GameObject Tongue_3;
    public GameObject Player;

    public Vector3 targetPos;
    Vector3 HitPos;

    private WaitForSeconds lickDuration = new WaitForSeconds(1f);
    private float nextLick;
    private float tongue;
    public bool swinging;

    //Gets the bitmask of layer 8(Player)
    int layerMask = 1 << 8;

    //Malin new stuff
    public LayerMask whatIsGround;


    void Awake()
    {
        //pröööt
        tongueTip = GameObject.Find("tongueTip");
        Player = GameObject.Find("Player");
        Tongue = GameObject.Find("Tongue");
        Tongue_2 = GameObject.Find("Tongue_2");
        Tongue_3 = GameObject.Find("Tongue_3");

        tongue_collider = Tongue.GetComponent<Collider2D>();
        tongue_collider2 = Tongue_2.GetComponent<Collider2D>();
        tongue_collider3 = Tongue_3.GetComponent<Collider2D>();
        joint = Player.GetComponent<SpringJoint2D>();

        //Inverts the bitmask to target ALL but Player Layer
        layerMask = ~layerMask;

    }

    void Start()
    {
        swinging = false;
        joint.enabled = false;
        line.enabled = false;

        tongueTip.SetActive(false);
    }



    //Possible to create a OnTriggerEnter in order to latch on with the tip
    void OnTriggerEnter2D(Collider2D other)
    {
        targetPos = tongueTip.transform.position;

        //whatisground is new/changed
        RaycastHit2D hit = Physics2D.Raycast(Player.transform.position, targetPos - Player.transform.position, 4f, whatIsGround);

        if (hit.collider != null && hit.collider.gameObject.GetComponent<Rigidbody2D>() != null && swinging == false)
        {
            swinging = true;
            line.enabled = true;
            line.SetPosition(0, Player.transform.position);
            line.SetPosition(1, hit.point);

            joint.enabled = true;
            joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
            joint.connectedAnchor = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
            joint.distance = Vector2.Distance(Player.transform.position, hit.point);

            if (joint.distance < 3f)
            {
                joint.distance = 3f;
            }

            if (tongue_collider2.enabled == true || tongue_collider.enabled == true || tongue_collider3.enabled == true)
            {
                tongue_collider3.enabled = false;
                tongue_collider2.enabled = false;
                tongue_collider.enabled = false;
            }

            tongueTip.SetActive(false);

            GameObject g = hit.collider.gameObject;
            string name = g.name;

            Debug.Log(g.name + " is hit.");

            
        }


    }
}

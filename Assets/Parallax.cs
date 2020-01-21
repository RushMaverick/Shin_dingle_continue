using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startposx;
    private float height, startposy;
    public GameObject cam;
    public GameObject Player;
    public float parallaxEffectx;
    public float parallaxEffecty;
    void Start()
    {
        startposx = transform.position.x;
        startposy = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;

    } 

    void Update()
    {
        float distx = (cam.transform.position.x * parallaxEffectx);
        float disty = (cam.transform.position.y * parallaxEffecty);

        transform.position = new Vector3(startposx + distx, startposx - disty, transform.position.z);

    }
}

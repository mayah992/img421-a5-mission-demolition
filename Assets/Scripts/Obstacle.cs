using System;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    private Transform target;
    public float speed = 5.0f;
    public AudioSource obstacleHit;

    // Start is called before the first frame update
    void Start()
    {
        target = pointA;
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        if (transform.position.y == target.position.y)
        {
            SwitchTarget();
            print("switching target");
        }
    }

    void SwitchTarget()
    {
        if( target == pointA)
        {
            target = pointB;
        }
        else
        {
            target = pointA;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // find out what hit the obstacle
        GameObject collidedWith = collision.gameObject;
        if(collidedWith.CompareTag("CurrentProjectile"))
        {
            // make sound
            obstacleHit.PlayOneShot(obstacleHit.clip);
        }
    }
}

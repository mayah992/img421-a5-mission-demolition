using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    SphereCollider area;
    public float speed = 5.0f;
    public AudioSource enemySound;
    public AudioSource enemyHit;
    public AudioSource enemyAttack;
    public float damagePerHit = 10f;

    bool isInside;

    static List<Enemy> ENEMIES = new List<Enemy>();

    public float attackCooldownLength = 3f;
    private float attackCooldown = 3f;

    private Rigidbody enemyRB;
    bool isDestroyed = false;

    public HealthCounter healthCounter;

    void Start()
    {
        enemyRB = this.GetComponent<Rigidbody>();

        target = GameObject.FindGameObjectWithTag("Slingshot").transform;
        area = target.GetComponent<SphereCollider>();

        // find reference to score counter game object
        GameObject healthGO = GameObject.Find("HealthCounter");
            // get the text component
        healthCounter = healthGO.GetComponent<HealthCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDestroyed == false)
        {
            attackCooldown -= Time.deltaTime;
            isInside = area.ClosestPoint(transform.position) == transform.position;
            if(isInside == false)
            {
                float step = speed * Time.deltaTime;
                Vector3 targetX = new Vector3(target.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetX, step);
            }
            else if(isInside == true)
            {
                if (attackCooldown <= 0) 
                {
                    Attack(damagePerHit);
                    print("enemy attacked!");
                    attackCooldown = attackCooldownLength;
                }
            }
        }
    }

    void Attack(float damagePerHit)
    {
        enemyAttack.Play();
        // deal damage to player's health
        healthCounter.health -= 10;
    }

    void OnCollisionEnter(Collision collision)
    {
        // find out what hit the enemy
        GameObject collidedWith = collision.gameObject;
        if(collidedWith.CompareTag("CurrentProjectile"))
        {
            print("enemy destroyed");

            enemyRB.isKinematic = false;
            enemyRB.AddForce(Vector3.right * 10f, ForceMode.Impulse);
            isDestroyed = true;
            enemyHit.Play();
        }
    }
    private void OnDestroy()
    {
        ENEMIES.Remove(this);
    }
}

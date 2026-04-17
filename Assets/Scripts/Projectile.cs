using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    const int LOOKBACK_COUNT = 10;
    static List<Projectile> PROJECTILES = new List<Projectile>();

    [SerializeField]
    private bool _awake = true;
    public bool awake{
        get{return _awake;}
        private set{_awake = value;}
    }

    private Vector3 prevPos;
    private List<float> deltas = new List<float>();
    private Rigidbody rigid;

    public AudioSource castleHit;


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        gameObject.tag = "CurrentProjectile";
        awake = true;
        prevPos = new Vector3(1000,1000,0);
        deltas.Add(1000);
        PROJECTILES.Add(this);

        if(SceneManager.GetActiveScene().name == "EasyMode")
        {
            // Set scale to 2x in all directions
            transform.localScale = new Vector3(5f, 5f, 5f);
            rigid.mass = 30f;
        }

        if(SceneManager.GetActiveScene().name == "MediumMode")
        {
            // Set scale to 2x in all directions
            transform.localScale = new Vector3(2f, 2f, 2f);
            rigid.mass = 10f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rigid.isKinematic || !awake) return;

        Vector3 deltaV3 = transform.position - prevPos;
        deltas.Add(deltaV3.magnitude);
        prevPos = transform.position;

        while (deltas.Count > LOOKBACK_COUNT)
        {
            deltas.RemoveAt(0);
        }
        float maxDelta = 0;
        foreach(float f in deltas)
        {
            if(f>maxDelta) maxDelta = f;
        }
        if(maxDelta <= Physics.sleepThreshold)
        {
            awake = false;
            rigid.Sleep();
            gameObject.tag = "Projectile";
            gameObject.layer = 6;
        }
    }
    private void OnDestroy()
    {
        PROJECTILES.Remove(this);
    }
    static public void DESTROY_PROJECTILES()
    {
        foreach (Projectile p in PROJECTILES)
        {
            Destroy(p.gameObject);
        } 
    }

    void OnCollisionEnter(Collision collision)
    {
        // find out what hit the projectile
        GameObject collidedWith = collision.gameObject;
        if(collidedWith.CompareTag("Castle"))
        {
            // make sound
            castleHit.PlayOneShot(castleHit.clip);
        }
    }
}

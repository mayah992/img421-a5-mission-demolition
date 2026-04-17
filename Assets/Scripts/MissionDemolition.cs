using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameMode{
    idle,
    playing,
    levelEnd
}
public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;
    [Header("Inscribed")]
    public Text uitLevel;
    public Text uitShots;
    public Vector3 castlePos;
    public GameObject[] castles;
    public GameObject goodJobAnim;
    public GameObject levelDescription;
    public float timeShowDescription = 3f;

    // evil clown variables
    public GameObject enemyPrefab;
    public float minEnemyDelay = 5f;
    public float maxEnemyDelay = 15f;


    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";
    // Start is called before the first frame update
    void Start()
    {
        S = this;
        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        goodJobAnim.SetActive(false);
        StartLevel();

        if (SceneManager.GetActiveScene().name == "HardMode")
        {
            Invoke("SpawnEnemy", 1f);
        }
    }
    void StartLevel()
    {
        if(castle != null)
        {
            Destroy(castle);   
        }

        Projectile.DESTROY_PROJECTILES();
        GameObject[] ENEMIES = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in ENEMIES)
        {
            Destroy(enemy);
        }
        
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        Goal.goalMet = false;
        UpdateGUI();
        mode = GameMode.playing;
        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
    }

    void UpdateGUI()
    {
        uitLevel.text = "Level: "+(level+1)+" of " + levelMax;
        uitShots.text = "Shots Taken: "+shotsTaken;
    }

    // Update is called once per frame
    void Update()
    {
        // esc to main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu"); 
        }

        if (timeShowDescription > 0) 
        {
            timeShowDescription -= Time.deltaTime;
        } else 
        {
            levelDescription.SetActive(false);
        }

        UpdateGUI();

        if((mode == GameMode.playing) && Goal.goalMet == true)
        {
            // play animation
            goodJobAnim.SetActive(true);
            
            mode = GameMode.levelEnd;
            FollowCam.SWITCH_VIEW(FollowCam.eView.both);
            Invoke("NextLevel", 2f);
        }
        
    }

    void NextLevel()
    {
        level++;
        minEnemyDelay--;
        maxEnemyDelay--;
        goodJobAnim.SetActive(false);
        if(level >= levelMax)
        {
            SceneManager.LoadScene("GameComplete");
        }
        else
        {
            StartLevel();
        }
    }
    static public void SHOT_FIRED()
    {
        S.shotsTaken++;
    }
    static public GameObject GET_CASTLE()
    {
        return S.castle;
    }

    // evil crow spawn function
    void SpawnEnemy()
    {
        Vector3 spawnPos = new Vector3(Random.Range(20f, 40f), -8.5f, 0f);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        Debug.Log("EVIL CLOWN SPAWNED");

        // spawn next crow
        Invoke("SpawnEnemy", Random.Range(minEnemyDelay, maxEnemyDelay));
    }
}

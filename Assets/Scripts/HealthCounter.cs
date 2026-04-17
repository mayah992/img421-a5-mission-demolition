using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthCounter : MonoBehaviour
{
    public int health = 100;
    private TextMeshProUGUI uiText;

    // Start is called before the first frame update
    void Start()
    {
        uiText = GameObject.Find("UIText_Health").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        uiText.text = health.ToString("#,0");
        if(health <= 0)
        {
            SceneManager.LoadScene("Defeat");
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void EasyMode()
    {
        SceneManager.LoadScene("EasyMode");
    }

    public void MediumMode()
    {
        SceneManager.LoadScene("MediumMode");
    }

    public void HardMode()
    {
        SceneManager.LoadScene("HardMode");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}

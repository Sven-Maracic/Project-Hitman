using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    Canvas[] canvases;


    [SerializeField] Canvas VictoryScreen;
    [SerializeField] Canvas DeathScreen;
    [SerializeField] Canvas SettingsScreen;
    [SerializeField] Canvas MenuScreen;


    public bool gamePlayable = true;

    // Start is called before the first frame update
    void Awake()
    {
        canvases = GetComponentsInChildren<Canvas>();
        SetCanvases(false);
        VictoryScreen.enabled = false;
        DeathScreen.enabled = false;
        SettingsScreen.enabled = false;
    }

    public void SetCanvases(bool value)
    {
        foreach (Canvas c in canvases)
        {
            c.enabled = value;
        }
        SettingsScreen.enabled = false;

    }

    public void SetDeathCanvas(bool value)
    {
        DeathScreen.enabled = value;
    }
    public void SetWinCanvas(bool value)
    {
        VictoryScreen.enabled = value;
    }


    public void OnResumePressed()
    {
        LevelStateMachine.instance.ResumeGame();
    }

    public void OnSettingsPressed()
    {
        MenuScreen.enabled = false;
        SettingsScreen.enabled = true;
    }
    public void OnBackPressed()
    {
        MenuScreen.enabled = true;
        SettingsScreen.enabled = false;
    }


    public void OnExitPressed()
    {
        Application.Quit();
    }

    public void OnRestartPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        LevelStateMachine.instance.ResumeGame();
    }
}

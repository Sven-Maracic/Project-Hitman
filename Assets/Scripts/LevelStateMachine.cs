
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelStateMachine : MonoBehaviour
{
    public static LevelStateMachine instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }




    enum States { Gameplay, Paused, Won, Died }
    States currentState = States.Gameplay;

    [SerializeField] PauseMenuController pauseMenuController;
    [SerializeField] PlayerCameraControls cameraScript;
    [SerializeField] PlayerMovement movementScript;
    [SerializeField] PlayerAttack attackScript;
    [SerializeField] PlayerSightlineMesh sightlineScript;
    [SerializeField] EventSystem eventSystem;



    private void Start()
    {
        Time.timeScale = 1.0f; //not sure why, but if the game is restarted after winning (not losing), it is stuck on timescale 0, so i added this as a fix
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && currentState != States.Won && currentState != States.Died) // dont allow player to pause game if he already won/lost
        {
            if (currentState == States.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }




    public void OnWin()
    {
        currentState = States.Won;
        pauseMenuController.SetWinCanvas(true);
        cameraScript.enabled = false;
        movementScript.enabled = false;
        attackScript.enabled = false;
        sightlineScript.enabled = false;
        Time.timeScale = 0f;
    }

    public void OnDeath()
    {
        currentState = States.Died;
        pauseMenuController.SetDeathCanvas(true);
        cameraScript.enabled = false;
        movementScript.enabled = false;
        attackScript.enabled = false;
        sightlineScript.enabled= false;
        Time.timeScale = 0f;
    }


    internal void PauseGame()
    {
        eventSystem.enabled = true;
        currentState = States.Paused;
        cameraScript.enabled = false;
        movementScript.enabled = false;
        attackScript.enabled = false;
        sightlineScript.enabled = false;
        Time.timeScale = 0f;
        pauseMenuController.SetCanvases(true);
    }

    internal void ResumeGame()
    {
        eventSystem.enabled = false;
        currentState = States.Gameplay;
        cameraScript.enabled = true;
        movementScript.enabled = true;
        attackScript.enabled = true;
        sightlineScript.enabled = true;
        Time.timeScale = 1f;
        pauseMenuController.SetCanvases(false);
    }
}

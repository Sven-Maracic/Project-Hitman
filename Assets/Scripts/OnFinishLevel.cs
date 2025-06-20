using UnityEngine;
using UnityEngine.InputSystem.XR;

public class OnFinishLevel : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStateController controller;

        if (controller = collision.GetComponent<PlayerStateController>())
        {
            LevelStateMachine.instance.OnWin();
        }
    }
}

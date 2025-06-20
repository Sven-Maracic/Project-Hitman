using System;
using System.Collections;
using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    public enum AvailableStates { Patrol, Chasing, Lost, Dead }
    public AvailableStates currentState = AvailableStates.Patrol;

    EnemyMovement movementScript;
    EnemyAttack attackScript;

    [SerializeField] Sprite deadSprite;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GetComponent<EnemyMovement>();
        attackScript = GetComponent<EnemyAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case AvailableStates.Patrol:
                Patrol();
                break;
            case AvailableStates.Chasing:
                if (!attackScript.CheckIfInRange())
                    Chase();
                break;
            case AvailableStates.Lost:
                LookFor();
                break;
        }
    }


    public void UpdateState(AvailableStates state)
    {
        Debug.Log("current state updated + " + state);
        currentState = state;
        attackScript.ResetValues();
    }


    private void Patrol()
    {
        movementScript.Patrol();
    }

    private void Chase()
    {
        attackScript.OnPlayerSpotted();
    }

    private void LookFor()
    {
        attackScript.OnPlayerLost();
    }



    public void TakeDamage()
    {
        GetComponent<SpriteRenderer>().sprite = deadSprite;
        currentState = AvailableStates.Dead;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}

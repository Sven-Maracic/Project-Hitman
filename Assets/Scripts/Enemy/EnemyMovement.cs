using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] GameObject[] patrolPoints;


    [SerializeField] float lookAroundAngle;
    [SerializeField] float lookAroundSpeed;
    [SerializeField] float movementSpeed = 1;



    public GameObject nextPoint;
    private GameObject curentPoint;

    public int nextPointIndex = 1;
    private bool isReversed = false;
    private bool isLookingAround = false;
    public bool doneLookingForPlayer = false;

    private enum MovementStates { Moving, Waiting, Lost }
    private MovementStates currentState = MovementStates.Moving;




    private void Start()
    {
        curentPoint = patrolPoints[0];
        nextPoint = patrolPoints[1];
    }

    public void Patrol()
    {
        if (currentState == MovementStates.Lost)
        {
            nextPoint = GetClosestPoint(transform.position, patrolPoints);
            FindNextPointIndex();
            StartCoroutine(SmoothRotateCoroutine(gameObject.transform, nextPoint.transform.position));
            currentState = MovementStates.Moving;
            doneLookingForPlayer = true;
        }
        else if (currentState == MovementStates.Moving)
        {
            MoveTo(transform, nextPoint.transform.position, true);
        }
        else if (currentState == MovementStates.Waiting && !isLookingAround)
        {
            StopAllCoroutines();
            currentState = MovementStates.Moving;
        }
        else { currentState = MovementStates.Waiting; }
    }

    GameObject GetClosestPoint(Vector3 point, GameObject[] availablePoints)
    {
        GameObject closestObj = null;
        float closestObjDist = 2500f;
        foreach (GameObject patrolPoint in availablePoints)
        {
            if (((patrolPoint.transform.position - point).magnitude) < (closestObjDist))
            {
                closestObj = patrolPoint;
                closestObjDist = ((patrolPoint.transform.position - point).magnitude);
            }
        }
        return closestObj;
    }
    private void PatrolGetNextPoint()
    {
        if (nextPoint == patrolPoints[patrolPoints.Length - 1])
        {
            nextPointIndex = patrolPoints.Length - 2;
            isReversed = true;
        }
        else if (nextPoint == patrolPoints[0])
        {
            nextPointIndex = 1;
            isReversed = false;
        }
        else if (!isReversed)
        {
            nextPointIndex += 1;
        }
        else
        {
            nextPointIndex -= 1;
        }
        nextPoint = patrolPoints[nextPointIndex];
    }
    private void FindNextPointIndex()
    {
        for(int i = 0; i < patrolPoints.Length; i++)
        {
            if (patrolPoints[i] == nextPoint)
            {
                nextPointIndex = i;
            }
        }
    }

    void MoveTo(Transform gameObject, Vector3 position, bool lookAround)
    {
        gameObject.position = Vector3.MoveTowards(gameObject.position, position, Time.deltaTime * movementSpeed);
        if (lookAround && (gameObject.position - position).magnitude < 0.1f)
        {
            currentState = MovementStates.Waiting;
            curentPoint = nextPoint;
            if (LookAtCorrectAngle(gameObject, curentPoint.transform, gameObject.rotation))
            {
                StartCoroutine(LookAround(gameObject, lookAroundAngle));
            }
        }
    }

    public void MoveUrgent(Transform gameObject, Vector3 position)
    {
        MoveTo(gameObject, position, true);
        SmoothRotateFunc(gameObject, position, gameObject.rotation);
    }

    public void StartLookingForPlayer(Transform gameObject, Vector3 position)
    {
        doneLookingForPlayer = false;
        StartCoroutine(MoveAndLook(gameObject, position));
    }
    public IEnumerator MoveAndLook(Transform gameObject, Vector3 position)
    {
        while (gameObject.position != position)
        {
            MoveTo(gameObject, position, false);
            yield return new WaitForEndOfFrame();
        }
        yield return LookAround(gameObject, lookAroundAngle);
        currentState = MovementStates.Lost;
        doneLookingForPlayer = !isLookingAround;
        StopCoroutine(MoveAndLook(gameObject, position));
    }

    bool LookAtCorrectAngle(Transform gameObject, Transform nextPoint, Quaternion startingRotation)
    {
        float angle = Quaternion.Angle(startingRotation, nextPoint.rotation);
        if (Vector2.Angle(gameObject.transform.right, nextPoint.right) < 0.05f)
        {
            return true;
        }
        //gameObject.rotation = Quaternion.Slerp(gameObject.rotation, Quaternion.Euler(startingRotation.eulerAngles.x, startingRotation.eulerAngles.y, startingRotation.eulerAngles.z + angle), Time.deltaTime * lookAroundSpeed);
        gameObject.rotation = Quaternion.RotateTowards(gameObject.rotation,nextPoint.rotation, lookAroundSpeed * 100f * Time.deltaTime);
        return false;
    }

    IEnumerator LookAround(Transform gameObject, float angle)
    {
        isLookingAround = true;
        float tempAngle = angle;
        yield return SmoothRotateCoroutine(gameObject, tempAngle);

        tempAngle = -angle * 2;
        yield return SmoothRotateCoroutine(gameObject, tempAngle);

        if (currentState != MovementStates.Lost)
        {
            PatrolGetNextPoint();
            yield return SmoothRotateCoroutine(gameObject, nextPoint.transform.position);
        }
        isLookingAround = false;

        StopCoroutine(LookAround(gameObject, angle));
    }
    IEnumerator SmoothRotateCoroutine(Transform gameObject, float angle)
    {
        Quaternion startingRotation = gameObject.rotation;

        while (Quaternion.Angle(gameObject.rotation, Quaternion.Euler(startingRotation.eulerAngles.x, startingRotation.eulerAngles.y, startingRotation.eulerAngles.z + angle)) >= 0.05f)
        {

            gameObject.rotation = Quaternion.Slerp(gameObject.rotation, Quaternion.Euler(startingRotation.eulerAngles.x, startingRotation.eulerAngles.y, startingRotation.eulerAngles.z + angle), Time.deltaTime * lookAroundSpeed);
            yield return new WaitForEndOfFrame();
        }
        StopCoroutine(SmoothRotateCoroutine(gameObject, angle));
    }
    IEnumerator SmoothRotateCoroutine(Transform gameObject, Vector3 target)
    {
        Vector3 vector = target - gameObject.position;
        if (Vector3.SignedAngle(gameObject.up, vector, Vector3.right) < Vector3.SignedAngle(-gameObject.up, vector, Vector3.right))
        {
            yield return SmoothRotateCoroutine(gameObject, Vector3.SignedAngle(gameObject.right, vector, Vector3.right));
        }
        else
        {
            yield return SmoothRotateCoroutine(gameObject, -Vector3.SignedAngle(gameObject.right, vector, Vector3.right));
        }
        StopCoroutine(SmoothRotateCoroutine(gameObject, target));

    }

    void SmoothRotateFunc(Transform gameObject, Vector3 target, Quaternion startingRotation)
    {
        Vector3 vector = target - gameObject.position;


        if (Vector3.SignedAngle(gameObject.right, vector, Vector3.forward) > 0.1f)
        {
            SmoothRotateFunc(gameObject, Vector3.Angle(gameObject.right, vector), startingRotation);
        }
        else
        {
            SmoothRotateFunc(gameObject, -Vector3.Angle(gameObject.right, vector), startingRotation);
        }
    }
    void SmoothRotateFunc(Transform gameObject, float angle, Quaternion startingRotation)
    {
        if (Quaternion.Angle(gameObject.rotation, Quaternion.Euler(startingRotation.eulerAngles.x, startingRotation.eulerAngles.y, startingRotation.eulerAngles.z + angle)) >= 0.05f)
        {
            gameObject.rotation = Quaternion.Slerp(gameObject.rotation, Quaternion.Euler(startingRotation.eulerAngles.x, startingRotation.eulerAngles.y, startingRotation.eulerAngles.z + angle), Time.deltaTime * lookAroundSpeed);
        }
    }
}

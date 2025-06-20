using System.Collections;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private static int currentHp;
    [SerializeField] int maxHp = 3;
    [SerializeField] float invulnerabilityAfterDamageLength = 1f;

    bool canTakeDamage = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage()
    {
        if (canTakeDamage)
        {
            StartCoroutine(DoDamage());
        }
    }

    public IEnumerator DoDamage()
    {
        canTakeDamage = false;
        currentHp -= 1;
        //cameraShakeScript.StartShake();
        if (currentHp <= 0)
        {
            //cameraShakeScript.StopShake();
            Debug.LogError("died");
            LevelStateMachine.instance.OnDeath();
            //StateController.instance.OnDeath();
        }
        yield return new WaitForSeconds(invulnerabilityAfterDamageLength);
        canTakeDamage = true;
    }
}

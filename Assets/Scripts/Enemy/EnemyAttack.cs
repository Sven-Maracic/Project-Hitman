using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] GameObject player;
    EnemyStateController stateController;
    [SerializeField] Transform spawnLocation;


    [SerializeField] float shootRange;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletSpread;
    [SerializeField] float bulletCooldownBetween;
    [SerializeField] GameObject prefab_Bullet;
    EnemyMovement movement;

    private bool lookingForPlayer = false;
    private bool isOnCooldown = false;





    private void Start()
    {
        movement = GetComponent<EnemyMovement>();
        stateController = GetComponent<EnemyStateController>();
    }

    public void OnPlayerSpotted()
    {
        movement.StopAllCoroutines();
        movement.MoveUrgent(transform, player.transform.position);
    }

    public void ResetValues()
    {
        lookingForPlayer = false;
        movement.doneLookingForPlayer = false;
    }

    public void OnPlayerLost()
    {
        if (!lookingForPlayer)
        {
            lookingForPlayer = true;
            movement.StopAllCoroutines();
            movement.StartLookingForPlayer(gameObject.transform, player.transform.position);
        }
        else if (movement.doneLookingForPlayer)
        {
            lookingForPlayer = false;
            movement.StopAllCoroutines();
            stateController.UpdateState(EnemyStateController.AvailableStates.Patrol);
        }
    }

    public bool CheckIfInRange()
    {
        if (Physics2D.Raycast(transform.position, transform.right, shootRange, LayerMask.GetMask("VisibleObject")))
        { 
            if(!isOnCooldown)
                StartCoroutine(Shoot());
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator Shoot()
    {
        //create a basic bullet Attack
        isOnCooldown = true;
        GameObject bullet = Instantiate(prefab_Bullet, spawnLocation.position, new Quaternion(0, 0, 0, 0));
        bullet.GetComponent<EnemyBulletScript>().GetValues(bulletSpeed); // this is here just so we can easily edit the bullet speed from the enemy's inspector
        bullet.transform.rotation = Quaternion.FromToRotation(bullet.transform.forward, player.transform.position + new Vector3(UnityEngine.Random.Range(-bulletSpread, bulletSpread), UnityEngine.Random.Range(-bulletSpread, bulletSpread), 0) - bullet.transform.position);
        yield return new WaitForSeconds(bulletCooldownBetween);
        isOnCooldown = false;
    }
}

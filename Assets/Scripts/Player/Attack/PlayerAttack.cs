using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool canShoot = true;

    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletCooldownBetween;
    [SerializeField] GameObject prefab_Bullet;
    [SerializeField] Transform spawnLocation;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (canShoot)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        GameObject bullet = Instantiate(prefab_Bullet, spawnLocation.position, new Quaternion(0, 0, 0, 0));
        bullet.GetComponent<PlayerBulletScript>().GetValues(bulletSpeed); // this is here just so we can easily edit the bullet speed from the enemy's inspector
        bullet.transform.rotation = Quaternion.FromToRotation(bullet.transform.forward, transform.right);
        yield return new WaitForSeconds(bulletCooldownBetween);
        canShoot = true;
    }

    
}

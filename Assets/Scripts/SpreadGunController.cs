using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadGunController : MonoBehaviour
{

    public Bullet bulletPrefab;
    public Transform[] firePoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        foreach (Transform firePoint in firePoints)
        {
            Bullet bullet = Instantiate(this.bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.Project(firePoint.transform.up);
        }
    }
}

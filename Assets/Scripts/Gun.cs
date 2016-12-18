using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MuzzleFlash))]
public class Gun : MonoBehaviour {

    public Transform muzzle;
    public Transform shell;
    public Transform shellEjection;
    public Projectile projectile;
    public float msBetweenShots = 100f;
    public float projectileVelocity = 35f;

    float nextShotTime;
    MuzzleFlash muzzleFlash;

    private void Start()
    {
        muzzleFlash = GetComponent<MuzzleFlash>();
    }

    public void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            newProjectile.SetSpeed(projectileVelocity);

            Instantiate(shell, shellEjection.position, shellEjection.rotation);
            muzzleFlash.Activate();
        }
    }
}

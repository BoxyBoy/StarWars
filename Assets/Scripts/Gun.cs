using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MuzzleFlash))]
public class Gun : MonoBehaviour {

    public enum FireMode { Auto, Burst, Single };
    public FireMode fireMode;

    public Transform[] projectileSpawn;
    public Projectile projectile;

    [Header("Gun Main Settings")]
    public float msBetweenShots = 100f;
    public float projectileVelocity = 35f;
    public float gunReloadTime = .3f; 
    public int burstCount;
    public int projectilesPerMagazine;

    [Header("Gun Effects")]
    public Transform shell;
    public Transform shellEjection;

    [Header("Gun Recoil Settings")]
    public Vector2 kickMinMax = new Vector2(.05f, .2f);
    public Vector2 recoilAngleMinMax = new Vector2(5, 10);
    public float recoilPositionTime = .1f;
    public float recoilRotationTime = .1f;

    [Header("Gun Audio Settings")]
    public AudioClip shootAudio;
    public AudioClip reloadAudio;

    float nextShotTime;
    bool triggerReleasedSinceLastShot;
    bool isReloading;

    int shotsRemainingInBurst;
    int projectilesRemainingInMagazine;

    MuzzleFlash muzzleFlash;

    Vector3 recoilSmoothDampVelocity;
    float recoilRotationSmoothDampVelocity;
    float recoilAngle;

    private void Start()
    {
        muzzleFlash = GetComponent<MuzzleFlash>();

        shotsRemainingInBurst = burstCount;
        projectilesRemainingInMagazine = projectilesPerMagazine;
    }

    private void LateUpdate()
    {
        // Animate gun recoil
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, recoilPositionTime);

        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotationSmoothDampVelocity, recoilRotationTime);
        transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle;

        if (!isReloading && projectilesRemainingInMagazine == 0)
        {
            Reload();
        }
    }

    private void Shoot()
    {
        if (!isReloading && Time.time > nextShotTime && projectilesRemainingInMagazine > 0)
        {
            if (fireMode == FireMode.Burst)
            {
                if (shotsRemainingInBurst == 0)
                {
                    return;
                }
                shotsRemainingInBurst--;
            }
            else if (fireMode == FireMode.Single)
            {
                if (!triggerReleasedSinceLastShot)
                {
                    return;
                }
            }

            for (int i = 0; i < projectileSpawn.Length; i++)
            {
                if (projectilesRemainingInMagazine == 0)
                {
                    break;
                }

                // Remove projectile from gun magazine
                projectilesRemainingInMagazine--;

                nextShotTime = Time.time + msBetweenShots / 1000;
                Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                newProjectile.SetSpeed(projectileVelocity);
            }

            // Shell and muzzle flash should only happen once (don't want to be too excesive)
            Instantiate(shell, shellEjection.position, shellEjection.rotation);
            muzzleFlash.Activate();

            // Gun recoil
            transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
            recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0f, 30f);

            // Audio Manager
            AudioManager.instance.PlaySound(shootAudio, transform.position);
        }
    }

    public void Aim(Vector3 aimPoint)
    {
        if (!isReloading)
        {
            transform.LookAt(aimPoint);
        }
    }

    public void Reload()
    {
        if (!isReloading && projectilesRemainingInMagazine != projectilesPerMagazine)
        {
            StartCoroutine(AnimateReload());

            // Audio Manager
            AudioManager.instance.PlaySound(reloadAudio, transform.position);
        }
    }

    IEnumerator AnimateReload()
    {
        isReloading = true;

        yield return new WaitForSeconds(.2f);

        float gunReloadSpeed = 1f / gunReloadTime;
        float percent = 0f;
        float maxReloadAngle = 30f;
        Vector3 initialRotation = transform.localEulerAngles;

        while (percent < 1)
        {
            percent += Time.deltaTime * gunReloadSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initialRotation + Vector3.left * reloadAngle;

            yield return null;
        }

        isReloading = false;
        projectilesRemainingInMagazine = projectilesPerMagazine;
    }

    public void OnTriggerHold()
    {
        Shoot();
        triggerReleasedSinceLastShot = false;

    }

    public void OnTriggerRelease()
    {
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = burstCount;
    }
}

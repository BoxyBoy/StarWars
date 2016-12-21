using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (GunController))]
public class Player : GameEntity {

    public float moveSpeed = 5f;
    public ParticleSystem deathEffect;

    Camera viewCamera;
    PlayerController playerController;
    GunController gunController;

    protected override void Start ()
    {
        base.Start();

        playerController = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        viewCamera = Camera.main;
    }

    public override void TakeDamage(float damage)
    {
        if (damage >= health)
        {
            Destroy(Instantiate(deathEffect.gameObject, transform.position, Quaternion.FromToRotation(Vector3.forward, transform.position.normalized)) as GameObject, deathEffect.main.startLifetimeMultiplier);
        }
        base.TakeDamage(damage);
    }

    void Update ()
    {
        // Movement Input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;

        playerController.Move(moveVelocity);

        // Look At Input
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 pointOfIntersection = ray.GetPoint(rayDistance);
            // Debug.DrawLine(ray.origin, pointOfIntersection, Color.red);

            playerController.LookAt(pointOfIntersection);
        }

        // Weapon Input
        if (Input.GetMouseButton(0))
        {
            gunController.OnTriggerHold();
        }

        if (Input.GetMouseButtonUp(0))
        {
            gunController.OnTriggerRelease();
        }
    }
}

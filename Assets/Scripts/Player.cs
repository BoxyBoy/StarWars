using UnityEngine;

[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (GunController))]
public class Player : GameEntity {

    public float moveSpeed = 5f;
    public Crosshairs crosshairs;
    public ParticleSystem deathEffect;
    public float aimDistanceTreshold = 1.0f;

    Camera viewCamera;
    PlayerController playerController;
    GunController gunController;
    Spawner spawner;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        spawner = FindObjectOfType<Spawner>();

        spawner.OnNewWave += OnNewWave;
    }

    protected override void Start ()
    {
        base.Start();

        viewCamera = Camera.main;        
    }

    private void OnNewWave(int waveNumber)
    {
        // Re-generate health
        health = initialHealth;

        // Equip weapon
        gunController.EquipGun(waveNumber);
    }

    public override void TakeDamage(float damage)
    {
        if (FindObjectOfType<Player>() != null && damage >= health)
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
        Plane groundPlane = new Plane(Vector3.up, Vector3.up * gunController.GunHeight);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 pointOfIntersection = ray.GetPoint(rayDistance);
            Debug.DrawLine(ray.origin, pointOfIntersection, Color.red);

            playerController.LookAt(pointOfIntersection);

            crosshairs.transform.position = pointOfIntersection;
            crosshairs.DetectTargets(ray);

            // Aim precision
            float distanceBetweenPlayerAndPointOfIntersection = (new Vector2(pointOfIntersection.x, pointOfIntersection.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude;
            if (distanceBetweenPlayerAndPointOfIntersection > aimDistanceTreshold)
            {
                gunController.Aim(pointOfIntersection);
            }
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            gunController.Reload();
        }

        // Player Death Plummet
        if (transform.position.y < -10f)
        {
            TakeDamage(health);
        }
    }

    public override void Die()
    {
        AudioManager.instance.PlaySound("Player Death", transform.position);
        base.Die();
    }
}

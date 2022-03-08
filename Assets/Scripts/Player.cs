using UnityEngine;

[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (GunController))]
public class Player : GameEntity {

    public float moveSpeed = 5f;
    public Crosshairs crosshairs;
    public ParticleSystem deathEffect;
    public float aimDistanceTreshold = 1.0f;

    Camera viewCamera;
    public PlayerController playerController;
    public GunController gunController;
    Spawner spawner;
    int i;
    int c;

    public RectTransform healthBar;
    public RectTransform shieldBar;

    //[SerializeField] public PlayerUI playerUI;

    private void Awake()
    {
       
        playerController = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        spawner = FindObjectOfType<Spawner>();
        healthBar.gameObject.SetActive(true);
        shieldBar.gameObject.SetActive(true);
        //playerUI.SetPlayer(this);

        spawner.OnNewWave += OnNewWave;
    }

    protected override void Start ()
    {
        base.Start();

        viewCamera = Camera.main;

        i = gunController.weapons.Length - 1;

        //if (playerUI != null)
        //{
        //    //GameObject _uiGo = Instantiate(playerUI);
        //    //_uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        //    Debug.Log($"{playerUI.player.GetType().Name}");
        //    Debug.Log("I should be displaying");
        //}
        //else
        //{
        //    Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
        //}
    }

    private void SwitchWeapon()
    {
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0) //scrolling up
        {
            c++;
            if(c <= i)
            {
                gunController.EquipGun(c);
            }
            else
            {
                c = 0;
                gunController.EquipGun(c);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            c--;
            if (c <= i && c != 0)
            {
                gunController.EquipGun(c);
            }
            else
            {
                c = i;
                gunController.EquipGun(c);
            }
        }

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

    void FixedUpdate ()
    {
        SwitchWeapon();

        // Movement Input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;

        playerController.Move(moveVelocity);

        //Update Health
        float healthPercent = 0f;
        healthPercent = health / initialHealth;
        healthBar.localScale = new Vector3(healthPercent, 1f, 1f);

        float shieldPercent = 0f;
        shieldPercent = shield / maxShield;
        shieldBar.localScale = new Vector3(shieldPercent, 1f, 1f);

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

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            GameObject.FindObjectOfType<SquadController>().CollisionDetected(this);
        }
    }

    
}

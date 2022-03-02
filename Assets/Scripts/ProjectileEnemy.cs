using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class ProjectileEnemy : GameEntity
{
    public float attackDistance = 3f;
    public float movementSpeed = 4f;
    
    public static event System.Action OnDeathStatic;

    //How much damage will npc deal to the player
    public float npcDamage = 5;
    public float attackRate = 0.5f;
    public Transform firePoint;

    public Projectile projectile;
    public Transform[] projectileSpawn;

    [HideInInspector]
    public GameEntity targetEntity;
    [HideInInspector]
    public Transform playerTransform;
    [HideInInspector]
    public ParticleSystem deathEffect;
    [HideInInspector]
    public Spawner es;
    [HideInInspector]
    NavMeshAgent agent;
    [HideInInspector]
    float nextAttackTime = 0;
    [HideInInspector]
    Transform target;


    private void Awake()
    { 
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {

            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<GameEntity>();
        }
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackDistance;
        agent.speed = movementSpeed;

        targetEntity = target.GetComponent<GameEntity>();

        //Set Rigidbody to Kinematic to prevent hit register bug
       
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance - attackDistance < 0.01f)
        {
            if (Time.time > nextAttackTime)
            {
                nextAttackTime = Time.time + attackRate;

                //Attack
                for (int i = 0; i < projectileSpawn.Length; i++)
                {
                    Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                    newProjectile.SetSpeed(0.2f);
                    
                    targetEntity.TakeDamage(npcDamage);
                }

                 
            }
        }
        //Move towardst he player
        agent.destination = playerTransform.position;
        //Always look at player
        transform.LookAt(new Vector3(playerTransform.transform.position.x, transform.position.y, playerTransform.position.z));
    }


    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        AudioManager.instance.PlaySound("Impact", transform.position);

        if (damage >= health && !dead)
        {
            if (OnDeathStatic != null)
            {
                OnDeathStatic();
            }
            AudioManager.instance.PlaySound("Enemy Death", transform.position);
            Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)) as GameObject, deathEffect.main.startLifetimeMultiplier);
        }
        base.TakeHit(damage, hitPoint, hitDirection);
    }

}
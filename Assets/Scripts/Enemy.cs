using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
[RequireComponent (typeof (Animator))]
public class Enemy : GameEntity {

    public enum State {Idle, Chasing, Attacking};
    public ParticleSystem deathEffect;
    public static event System.Action OnDeathStatic;

    [Header("Enemy Main Settings")]
    public bool canAttackPlayer = true;


    [Header("Enemy Navigation Settings")]
    public float agentPathRefreshRate = 1.0f;
    public float agentStoppingDistance = 1.0f;

    NavMeshAgent pathFinder;
    Animator myAnimator;
    Transform target;
    Vector3 agentDestination;
    GameEntity targetEntity;
    Material skinMaterial;
    Color originalColor;

    State currentState;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1f;
    float damage = 1f;
    float nextAttackTime = 0;
    float myCollisionRadius;
    float targetCollisionRadius;
    bool hasTarget;



    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        myAnimator = GetComponent<Animator>();

        //if (GameObject.FindGameObjectWithTag("Player") != null)
        //{
        //    hasTarget = true;

        //    target = GameObject.FindGameObjectWithTag("Player").transform;
        //    targetEntity = target.GetComponent<GameEntity>();

        //    myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        //    targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
        //}
        GetTarget();
    }

    protected override void Start ()
    {
        base.Start();

        if (hasTarget)
        {
            currentState = State.Chasing;
            targetEntity.OnDeath += OnTargetDeath;
            agentDestination = pathFinder.destination;

            StartCoroutine(UpdatePath());
        } 
	}

    IEnumerator UpdatePath()
    {
        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - directionToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);

                if (!dead && !pathFinder.pathPending && Vector3.Distance(agentDestination, targetPosition) > agentStoppingDistance)
                {
                    // Update agent destination
                    agentDestination = targetPosition;
                    pathFinder.destination = agentDestination;
                }
            }

            yield return new WaitForSeconds(agentPathRefreshRate);
        }
    }

    private void Update()
    {
        canAttackPlayer = true;

        if (targetEntity == null)
        {
            GetTarget();
        }

        if (hasTarget && Time.time > nextAttackTime)
        {
            float sqrDistanceToTarget = (target.position - transform.position).sqrMagnitude;
            if (sqrDistanceToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
            {
                nextAttackTime = Time.time + timeBetweenAttacks;
                if (canAttackPlayer)
                { 
                    AudioManager.instance.PlaySound("Enemy Attack", transform.position);
                    StartCoroutine(Attack());
                    
                }
            }
        }
    }

    public void GetTarget()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            hasTarget = true;

            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<GameEntity>();

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
        }
    }

    IEnumerator Attack()
    {
        currentState = State.Attacking;
        pathFinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - directionToTarget * (myCollisionRadius);

        float percent = 0f;
        float attackSpeed = 3f;

        skinMaterial.color = Color.red;

        bool hasAppliedDamage = false;
        while (percent <= 1f)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }
            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;

            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        skinMaterial.color = originalColor;

        currentState = State.Chasing;
        pathFinder.enabled = true;
    }

    public void SetCharacteristics(float moveSpeed, int hitsToKillPlayer, float enemyHealth, Color skinColor)
    {
        pathFinder.speed = moveSpeed;

        if (hasTarget)
        {
            damage = Mathf.Ceil(targetEntity.initialHealth / hitsToKillPlayer);
        }
        initialHealth = enemyHealth;

        var mainModuleParticleSystem = deathEffect.main;
        mainModuleParticleSystem.startColor = new Color(skinColor.r, skinColor.g, skinColor.b, 1);

        skinMaterial = GetComponent<Renderer>().material;
        skinMaterial.color = skinColor;

        originalColor = skinMaterial.color;
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

    private void OnTargetDeath()
    {
        hasTarget = false;
        targetEntity = null;
        currentState = State.Idle;        
    }
}

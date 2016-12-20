using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
[RequireComponent (typeof (Animator))]
public class MovementNavMeshAgent : MonoBehaviour {
    Animator myAnimator;
    NavMeshAgent myNavMeshAgent;

    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        myNavMeshAgent = GetComponent<NavMeshAgent>();

        myNavMeshAgent.updatePosition = false;
        myAnimator.SetBool("move", true);
    }

    private void Update()
    {
        Vector3 worldDeltaPosition = myNavMeshAgent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaPosition
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
        {
            velocity = smoothDeltaPosition / Time.deltaTime;
        }

        // bool shouldMove = velocity.magnitude > 0.5f && myNavMeshAgent.remainingDistance > myNavMeshAgent.radius;
        // bool shouldMove = myNavMeshAgent.remainingDistance > myNavMeshAgent.radius;

        // Update animation parameters
        // myAnimator.SetBool("move", shouldMove);
        myAnimator.SetFloat("velX", velocity.x);
        myAnimator.SetFloat("velY", velocity.y);

        LookAtTarget lookAtTarget = GetComponent<LookAtTarget>();
        if (lookAtTarget)
        {
            // Debug.Log("Change lookAtTargetPosition");
            lookAtTarget.lookAtTargetPosition = myNavMeshAgent.steeringTarget + transform.forward;
        }

        // Pull character towards agent
        if (worldDeltaPosition.magnitude > myNavMeshAgent.radius)
        {
            transform.position = myNavMeshAgent.nextPosition - 0.9f * worldDeltaPosition;
        }
    }

    private void OnAnimatorMove()
    {
        if (transform == null || myNavMeshAgent == null) return;

        // Update position to agent position
        transform.position = myNavMeshAgent.nextPosition;        
    }
}

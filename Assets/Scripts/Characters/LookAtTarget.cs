using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class LookAtTarget : MonoBehaviour {

    public Transform head = null;
    public Vector3 lookAtTargetPosition;
    public float lookAtCoolTime = 0.2f;
    public float lookAtHeadTime = 0.2f;
    public bool looking = true;

    private Vector3 lookAtPosition;
    private Animator myAnimator;
    private float lookAtWeight = 0.0f;

    private void Start()
    {
        // Debug.Log("LookAtTarget script start");
        if (!head)
        {
            Debug.Log("No head transform - LookAt disabled");
            enabled = false;
            return;
        }

        myAnimator = GetComponent<Animator>();
        lookAtTargetPosition = head.position + transform.forward;
        lookAtPosition = lookAtTargetPosition;
    }

    void OnAnimatorIK(int layerIndex)
    {
        lookAtTargetPosition.y = head.position.y;
        float lookAtTargetWeight = looking ? 1.0f : 0.0f;

        Vector3 currentDirection = lookAtPosition - head.position;
        Vector3 futureDirection = lookAtTargetPosition - head.position;

        currentDirection = Vector3.RotateTowards(currentDirection, futureDirection, 6.28f * Time.deltaTime, float.PositiveInfinity);
        lookAtPosition = head.position + currentDirection;

        float blendTime = lookAtTargetWeight > lookAtWeight ? lookAtHeadTime : lookAtCoolTime;
        lookAtWeight = Mathf.MoveTowards(lookAtWeight, lookAtTargetWeight, Time.deltaTime / blendTime);

        myAnimator.SetLookAtWeight(lookAtWeight, 0.2f, 0.5f, 0.7f, 0.5f);
        myAnimator.SetLookAtPosition(lookAtPosition);
    }
}

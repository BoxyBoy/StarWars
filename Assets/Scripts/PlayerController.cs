using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public Rigidbody myRigidbody;
    public Vector3 myVelocity;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 velocity)
    {
        myVelocity = velocity;
    }

    public void LookAt(Vector3 lookAtPoint)
    {
        Vector3 heightCorrectionPoint = new Vector3(lookAtPoint.x, transform.position.y, lookAtPoint.z);
        transform.LookAt(heightCorrectionPoint);
    }

    void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + myVelocity * Time.fixedDeltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour {

    public float moveSpeed = 5f;

    Camera viewCamera;
    PlayerController playerController;

	void Start () {
        playerController = GetComponent<PlayerController>();
        viewCamera = Camera.main;
	}
	
	void Update () {
        float rayDistance;

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;

        playerController.Move(moveVelocity);
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 pointOfIntersection = ray.GetPoint(rayDistance);
            Debug.DrawLine(ray.origin, pointOfIntersection, Color.red);

            playerController.LookAt(pointOfIntersection);
        }
	}
}

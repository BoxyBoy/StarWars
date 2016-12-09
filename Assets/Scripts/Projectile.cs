using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;

    float mySpeed = 10f;

    public void SetSpeed(float speed)
    {
        mySpeed = speed;
    }

	void Update () {
        float moveDistance = mySpeed * Time.deltaTime;

        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
	}

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit)
    {
        Debug.Log(hit.collider.gameObject.name);
        GameObject.Destroy(gameObject);
    }
}

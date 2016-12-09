using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;

    float mySpeed = 10f;
    float myDamage = 1f;

    public void SetSpeed(float speed)
    {
        mySpeed = speed;
    }

    private void Update ()
    {
        float moveDistance = mySpeed * Time.deltaTime;

        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
	}

    private void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }

    private void OnHitObject(RaycastHit hit)
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(myDamage, hit);
        }
        GameObject.Destroy(gameObject);
    }
}

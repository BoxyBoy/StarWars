using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (TrailRenderer))]
public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;
    public Color trailColor;

    private ScoreManager scoreManager = new ScoreManager();

    float mySpeed = 10f;
    float myDamage = 1f;
    float lifetime = 3f;
    public void SetSpeed(float speed)
    {
        mySpeed = speed;
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);

        /*ollider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
        if (initialCollisions.Length > 0)
        {
            OnHitObject(initialCollisions[0], transform.position);
        }
*/
        GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailColor);
    }

    private void Update ()
    {
        float moveDistance = mySpeed * Time.deltaTime;

       // CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
	}

/*    private void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit.collider, hit.point);
        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if("Wall" == collision.gameObject.tag)
        {
            Destroy(this.gameObject);
        }
        if ("Enemy" == collision.gameObject.tag)
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
            scoreManager.OnEnemyKilled();
        }
    }

    /*private void OnHitObject(Collider collider, Vector3 hitPoint)
    {
        IDamageable damageableObject = collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(myDamage, hitPoint, transform.forward);
        }
        GameObject.Destroy(gameObject);
    }*/
}

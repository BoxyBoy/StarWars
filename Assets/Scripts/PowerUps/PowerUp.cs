using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    protected Collider modelCollider;
    protected Vector3 initialLocation;
    protected Player player;

    public float respawnTimer = 5.0f;
    public float currentTime = 0.0f;

    public GameObject pickUpModel;

    bool isPickedUp = false;

    void Start()
    {
        //set RigidBody, initialLocation, and player
        modelCollider = pickUpModel.GetComponent<Collider>();
        initialLocation = this.transform.position;
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        //if power up is picked up, start timer
        if (isPickedUp)
        {
            currentTime += Time.deltaTime;
        }

        //when timer reaches respawn time, re-enable model and set current timer to 0
        if(currentTime >= respawnTimer)
        {
            isPickedUp = false;
            pickUpModel.SetActive(true);
            currentTime = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            Debug.Log("Heal Collision");
            pickUpModel.SetActive(false);
            ApplyPowerUp();
            isPickedUp = true;
        }
    }

    protected abstract void ApplyPowerUp();
}

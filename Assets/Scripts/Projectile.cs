using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    float mySpeed = 10f;

    public void SetSpeed(float speed)
    {
        mySpeed = speed;
    }

	void Update () {
        transform.Translate(Vector3.forward * Time.deltaTime * mySpeed);
	}
}

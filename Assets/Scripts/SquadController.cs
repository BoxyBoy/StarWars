using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadController : MonoBehaviour
{
    public List<Player> squadies;
    
    void FixedUpdate()
    {
        //this.transform.position = squadies[0].transform.position;

        if (Input.GetMouseButton(0))
        {
            foreach(var squadie in squadies)
            {
                squadie.gunController.OnTriggerHold();
            }
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            foreach(var squadie in squadies)
            {
                squadie.gunController.OnTriggerRelease();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (var squadie in squadies)
            {
                squadie.gunController.Reload();
            }
        }
    }
}

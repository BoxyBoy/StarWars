using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadController : MonoBehaviour
{
    public List<Player> squadies;
    public Player focusPlayer;
    public int focusIndex;

    private void Start()
    {
        focusPlayer = squadies[0];
        focusIndex = 0;
    }

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

    public void NextFocusPlayer()
    {
        //for(int i = 0; i < squadies.Count; i++)
        //{
        //    if(squadies[i] != null)
        //    {
        //        focusPlayer = squadies[i];
        //    }
        //}
        int currentIndex = 0;
        squadies[focusIndex] = null;
        focusPlayer = null;

        while(focusPlayer == null)
        {
            
            if(squadies[currentIndex] != null)
            {
                focusPlayer = squadies[currentIndex];
                focusIndex = currentIndex;
            }
            else
            {
                currentIndex++;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadController : MonoBehaviour
{
    public List<Player> squadies;
    public Player focusPlayer;
    public int focusIndex;

    public int numSquadies;

    Vector3 focusPlayerPosition;

    Vector3 TLOffset = new Vector3(-30, 0, 20);
    Vector3 TROffset = new Vector3(30, 0, 20);
    Vector3 BLOffset = new Vector3(-30, 0, -20);
    Vector3 BROffset = new Vector3(30, 0, -20);

    private void Start()
    {
        focusPlayer = squadies[0];
        focusIndex = 0;
        numSquadies = squadies.Count;
        focusPlayerPosition = focusPlayer.transform.position;
    }

    void FixedUpdate()
    {
        //this.transform.position = squadies[0].transform.position;
        focusPlayerPosition = focusPlayer.transform.position;

        if (focusPlayer == squadies[0])
        {
            if (squadies[1].transform.position != (focusPlayerPosition + TLOffset))
            {
                squadies[1].GetComponent<PlayerController>().myRigidbody.MovePosition(focusPlayerPosition + (TLOffset * squadies[1].moveSpeed) * Time.fixedDeltaTime);
            }
            if (squadies[2].transform.position != (focusPlayerPosition + TROffset))
            {
                squadies[2].GetComponent<PlayerController>().myRigidbody.MovePosition(focusPlayerPosition + (TROffset * squadies[2].moveSpeed) * Time.fixedDeltaTime);
            }
            if (squadies[3].transform.position != (focusPlayerPosition + BLOffset))
            {
                squadies[3].GetComponent<PlayerController>().myRigidbody.MovePosition(focusPlayerPosition + (BLOffset * squadies[3].moveSpeed) * Time.fixedDeltaTime);
            }
            if (squadies[4].transform.position != (focusPlayerPosition + BROffset))
            {
                squadies[4].GetComponent<PlayerController>().myRigidbody.MovePosition(focusPlayerPosition + (BROffset * squadies[4].moveSpeed) * Time.fixedDeltaTime);
            }
        }
        else if(focusPlayer == squadies[1])
        {
            if (squadies[2].transform.position != (focusPlayerPosition + TROffset))
            {
                squadies[2].GetComponent<PlayerController>().myRigidbody.MovePosition(focusPlayerPosition + (TROffset * squadies[2].moveSpeed) * Time.fixedDeltaTime);
            }
            if (squadies[3].transform.position != (focusPlayerPosition + BLOffset))
            {
                squadies[3].GetComponent<PlayerController>().myRigidbody.MovePosition(focusPlayerPosition + (BLOffset * squadies[3].moveSpeed) * Time.fixedDeltaTime);
            }
            if (squadies[4].transform.position != (focusPlayerPosition + BROffset))
            {
                squadies[4].GetComponent<PlayerController>().myRigidbody.MovePosition(focusPlayerPosition + (BROffset * squadies[4].moveSpeed) * Time.fixedDeltaTime);
            }
        }
        else if (focusPlayer == squadies[2])
        {
            if (squadies[3].transform.position != (focusPlayerPosition + BLOffset))
            {
                squadies[3].GetComponent<PlayerController>().myRigidbody.MovePosition(focusPlayerPosition + (BLOffset * squadies[3].moveSpeed) * Time.fixedDeltaTime);
            }
            if (squadies[4].transform.position != (focusPlayerPosition + BROffset))
            {
                squadies[4].GetComponent<PlayerController>().myRigidbody.MovePosition(focusPlayerPosition + (BROffset * squadies[4].moveSpeed) * Time.fixedDeltaTime);
            }
        }
        else if (focusPlayer == squadies[3])
        {
            if (squadies[4].transform.position != (focusPlayerPosition + BROffset))
            {
                squadies[4].GetComponent<PlayerController>().myRigidbody.MovePosition(focusPlayerPosition + (BROffset * squadies[4].moveSpeed) * Time.fixedDeltaTime);
            }
        }


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
        numSquadies--;

        while(focusPlayer == null)
        {
            
            if(squadies[currentIndex] != null)
            {
                focusPlayer = squadies[currentIndex];
                focusIndex = currentIndex;
                focusPlayerPosition = focusPlayer.transform.position;
            }
            else
            {
                currentIndex++;
            }
        }
    }
}

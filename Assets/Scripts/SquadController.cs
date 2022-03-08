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

    Vector3 TLOffset = new Vector3(-3, 0, 2);
    Vector3 TROffset = new Vector3(3, 0, 2);
    Vector3 BLOffset = new Vector3(-3, 0, -2);
    Vector3 BROffset = new Vector3(3, 0, -2);

    private void Start()
    {
        focusPlayer = squadies[0];
        focusIndex = 0;
        numSquadies = squadies.Count;
        focusPlayerPosition = focusPlayer.playerController.myRigidbody.position;
    }

    void FixedUpdate()
    {
        if(focusPlayer != null)
        {
            //this.transform.position = squadies[0].transform.position;
            focusPlayerPosition = focusPlayer.playerController.myRigidbody.position;

            if (focusPlayer == squadies[0])
            {
                if (squadies[1].transform.position != (focusPlayerPosition + TLOffset))
                {
                    Vector3 moveVelocity = TLOffset.normalized * squadies[1].moveSpeed;
                    squadies[1].playerController.Move(moveVelocity);

                    squadies[1].playerController.myRigidbody.MovePosition((focusPlayerPosition + TLOffset) + squadies[1].playerController.myVelocity * Time.fixedDeltaTime);
                }
                if (squadies[2].transform.position != (focusPlayerPosition + TROffset))
                {
                    Vector3 moveVelocity = TROffset.normalized * squadies[2].moveSpeed;
                    squadies[2].playerController.Move(moveVelocity);

                    squadies[2].playerController.myRigidbody.MovePosition((focusPlayerPosition + TROffset) + squadies[2].playerController.myVelocity * Time.fixedDeltaTime);
                }
                if (squadies[3].transform.position != (focusPlayerPosition + BLOffset))
                {
                    Vector3 moveVelocity = BLOffset.normalized * squadies[3].moveSpeed;
                    squadies[3].playerController.Move(moveVelocity);

                    squadies[3].playerController.myRigidbody.MovePosition((focusPlayerPosition + BLOffset) + squadies[3].playerController.myVelocity * Time.fixedDeltaTime);
                }
                if (squadies[4].transform.position != (focusPlayerPosition + BROffset))
                {
                    Vector3 moveVelocity = BROffset.normalized * squadies[4].moveSpeed;
                    squadies[4].playerController.Move(moveVelocity);

                    squadies[4].playerController.myRigidbody.MovePosition((focusPlayerPosition + BROffset) + squadies[4].playerController.myVelocity * Time.fixedDeltaTime);
                }
            }
            else if (focusPlayer == squadies[1])
            {
                //if (squadies[2].transform.position != (focusPlayerPosition + TROffset))
                //{
                //    squadies[2].GetComponent<PlayerController>().myRigidbody.MovePosition(focusPlayerPosition + (TROffset * squadies[2].moveSpeed) * Time.fixedDeltaTime);
                //}
                //if (squadies[3].transform.position != (focusPlayerPosition + BLOffset))
                //{
                //    squadies[3].GetComponent<PlayerController>().myRigidbody.MovePosition(focusPlayerPosition + (BLOffset * squadies[3].moveSpeed) * Time.fixedDeltaTime);
                //}
                //if (squadies[4].transform.position != (focusPlayerPosition + BROffset))
                //{
                //    squadies[4].GetComponent<PlayerController>().myRigidbody.MovePosition(focusPlayerPosition + (BROffset * squadies[4].moveSpeed) * Time.fixedDeltaTime);
                //}

                if (squadies[2].transform.position != (focusPlayerPosition + TROffset))
                {
                    Vector3 moveVelocity = TROffset.normalized * squadies[2].moveSpeed;
                    squadies[2].playerController.Move(moveVelocity);

                    squadies[2].GetComponent<PlayerController>().myRigidbody.MovePosition((focusPlayerPosition + TROffset) + squadies[2].playerController.myVelocity * Time.fixedDeltaTime);
                }
                if (squadies[3].transform.position != (focusPlayerPosition + BLOffset))
                {
                    Vector3 moveVelocity = BLOffset.normalized * squadies[3].moveSpeed;
                    squadies[3].playerController.Move(moveVelocity);

                    squadies[3].GetComponent<PlayerController>().myRigidbody.MovePosition((focusPlayerPosition + BLOffset) + squadies[3].playerController.myVelocity * Time.fixedDeltaTime);
                }
                if (squadies[4].transform.position != (focusPlayerPosition + BROffset))
                {
                    Vector3 moveVelocity = BROffset.normalized * squadies[4].moveSpeed;
                    squadies[4].playerController.Move(moveVelocity);

                    squadies[4].GetComponent<PlayerController>().myRigidbody.MovePosition((focusPlayerPosition + BROffset) + squadies[4].playerController.myVelocity * Time.fixedDeltaTime);
                }
            }
            else if (focusPlayer == squadies[2])
            {
                //if (squadies[3].transform.position != (focusPlayerPosition + BLOffset))
                //{
                //    squadies[3].GetComponent<PlayerController>().myRigidbody.MovePosition(focusPlayerPosition + (BLOffset * squadies[3].moveSpeed) * Time.fixedDeltaTime);
                //}
                //if (squadies[4].transform.position != (focusPlayerPosition + BROffset))
                //{
                //    squadies[4].GetComponent<PlayerController>().myRigidbody.MovePosition(focusPlayerPosition + (BROffset * squadies[4].moveSpeed) * Time.fixedDeltaTime);
                //}

                if (squadies[3].transform.position != (focusPlayerPosition + BLOffset))
                {
                    Vector3 moveVelocity = BLOffset.normalized * squadies[3].moveSpeed;
                    squadies[3].playerController.Move(moveVelocity);

                    squadies[3].GetComponent<PlayerController>().myRigidbody.MovePosition((focusPlayerPosition + BLOffset) + squadies[3].playerController.myVelocity * Time.fixedDeltaTime);
                }
                if (squadies[4].transform.position != (focusPlayerPosition + BROffset))
                {
                    Vector3 moveVelocity = BROffset.normalized * squadies[4].moveSpeed;
                    squadies[4].playerController.Move(moveVelocity);

                    squadies[4].GetComponent<PlayerController>().myRigidbody.MovePosition((focusPlayerPosition + BROffset) + squadies[4].playerController.myVelocity * Time.fixedDeltaTime);
                }
            }
            else if (focusPlayer == squadies[3])
            {
                //if (squadies[4].transform.position != (focusPlayerPosition + BROffset))
                //{
                //    squadies[4].GetComponent<PlayerController>().myRigidbody.MovePosition(focusPlayerPosition + (BROffset * squadies[4].moveSpeed) * Time.fixedDeltaTime);
                //}

                if (squadies[4].transform.position != (focusPlayerPosition + BROffset))
                {
                    Vector3 moveVelocity = BROffset.normalized * squadies[4].moveSpeed;
                    squadies[4].playerController.Move(moveVelocity);

                    squadies[4].GetComponent<PlayerController>().myRigidbody.MovePosition((focusPlayerPosition + BROffset) + squadies[4].playerController.myVelocity * Time.fixedDeltaTime);
                }
            }


            if (Input.GetMouseButton(0))
            {
                foreach (var squadie in squadies)
                {
                    squadie.gunController.OnTriggerHold();
                }

            }

            if (Input.GetMouseButtonUp(0))
            {
                foreach (var squadie in squadies)
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

        if(numSquadies > 1)
        {
            numSquadies--;
        }
        
        while(focusPlayer == null && focusIndex < squadies.Count)
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

    public void CollisionDetected(Player player)
    {
        Debug.Log($"{player.name} collided");
    }
}

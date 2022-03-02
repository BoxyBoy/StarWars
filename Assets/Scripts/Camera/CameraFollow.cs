using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public SquadController squad;
        public int currentSquadieIndex;
        public float smoothing = 5f;

        Vector3 offset;

        void Start()
        {
            currentSquadieIndex = 0;
            //target = squad.squadies[currentSquadieIndex].transform;
            target = squad.focusPlayer.transform;
            offset = transform.position - target.position;
        }

        void FixedUpdate()
        {
            if (target == null && squad.squadies.Count == 0) return;

            if (target == null)
            {
                //for(int i = 0; i < squad.squadies.Count; i++)
                //{
                //    if(squad.squadies[i] != null)
                //    {
                //        currentSquadieIndex = i;
                //    }
                //}
                //target = squad.squadies[currentSquadieIndex].transform;
                target = squad.focusPlayer.transform;
            }

            Vector3 targetCamPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
}

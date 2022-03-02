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
            target = squad.squadies[currentSquadieIndex].transform;
            offset = transform.position - target.position;
        }

        void FixedUpdate()
        {
            if (target == null && squad.squadies.Count == 0) return;

            if (target == null)
            {
                currentSquadieIndex++;
                target = squad.squadies[currentSquadieIndex].transform;
            }

            Vector3 targetCamPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
}

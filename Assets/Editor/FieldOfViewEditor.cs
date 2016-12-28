using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor {

    private void OnSceneGUI()
    {
        if (target == null) return;

        FieldOfView fieldOfView = (FieldOfView)target;

        Handles.color = Color.white;
        Handles.DrawWireArc(fieldOfView.transform.position, Vector3.up, Vector3.forward, 360f, fieldOfView.viewRadius);

        Vector3 viewAngleA = fieldOfView.DirectionFromAngle(-fieldOfView.viewAngle / 2, false);
        Vector3 viewAngleB = fieldOfView.DirectionFromAngle(+fieldOfView.viewAngle / 2, false);

        Handles.DrawLine(fieldOfView.transform.position, fieldOfView.transform.position + viewAngleA * fieldOfView.viewRadius);
        Handles.DrawLine(fieldOfView.transform.position, fieldOfView.transform.position + viewAngleB * fieldOfView.viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fieldOfView.visibleTargets)
        {
            if (visibleTarget == null || fieldOfView.transform == null) continue;
            Handles.DrawLine(fieldOfView.transform.position, visibleTarget.position);
        }
    }
}

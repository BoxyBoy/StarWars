using UnityEngine;
using UnityEditor;
using UnityEditor.AI;

[CustomEditor (typeof (MapGenerator))]
public class MapEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MapGenerator map = target as MapGenerator;
        
        DrawDefaultInspector();

        if (GUI.changed)
        {
            map.GenerateMap();
            NavMeshBuilder.BuildNavMesh();
        }
    }
}

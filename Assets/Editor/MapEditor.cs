using UnityEngine;
using UnityEditor;
using UnityEditor.AI;

[CustomEditor (typeof (MapGenerator))]
public class MapEditor : Editor {

    public override void OnInspectorGUI()
    {
        MapGenerator map = target as MapGenerator;

        if (DrawDefaultInspector())
        {
            map.GenerateMap();
            NavMeshBuilder.BuildNavMesh();
        }

        if (GUILayout.Button("Generate map"))
        {
            map.GenerateMap();
        }
    }
}

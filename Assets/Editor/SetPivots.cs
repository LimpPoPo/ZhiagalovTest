using UnityEngine;
using UnityEditor;
using System.Collections;

public class SetPivots : EditorWindow
{

    public Transform oldPosition;
    public Transform newPosition;

    [MenuItem("Tools/Set Pivots")]
    public static void CreateWindow()
    {
        SetPivots window = GetWindow<SetPivots>();
        window.title = "Set Pivots";
    }

    public void OnGUI()
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Object:", GUILayout.Width(120));
        oldPosition = (Transform)EditorGUILayout.ObjectField(oldPosition, typeof(Transform), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.Label("New Position:", GUILayout.Width(120));
        newPosition = (Transform)EditorGUILayout.ObjectField(newPosition, typeof(Transform), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        if (GUILayout.Button("Edit pivot mesh"))
        {
            if (oldPosition == null)
            {
                Debug.LogError("No Object!");
                return;
            }
            if (newPosition == null)
            {
                Debug.LogError("No newPosition!");
                return;
            }
            MeshFilter mesh = oldPosition.GetComponent<MeshFilter>();
            if (mesh == null)
            {
                Debug.LogError("No MeshFilter in Object!");
                return;
            }

            Vector3 vect = new Vector3(newPosition.position.x - oldPosition.position.x, newPosition.position.y - oldPosition.position.y, newPosition.position.z - oldPosition.position.z);

            Vector3[] verts = mesh.sharedMesh.vertices;
            for (int i = 0; i < verts.Length; i++)
            {
                verts[i] += vect;
            }
            mesh.sharedMesh.vertices = verts;
            mesh.sharedMesh.RecalculateBounds();

            newPosition.position = oldPosition.position;

            Debug.Log("Edit OK.");
        }
    }
}
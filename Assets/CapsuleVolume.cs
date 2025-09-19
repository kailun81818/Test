using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class CapsuleVolume : MonoBehaviour
{
    void Start()
    {
        CalculateVolume();
    }

    void Update()
    {
        //CalculateVolume();
    }

    void CalculateVolume()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        List<Vector3> vertices = new();
        List<int> indices = new();
        meshFilter.mesh.GetVertices(vertices);
        meshFilter.mesh.GetIndices(indices, 0);

        //Vector3 vertex0 = vertices[0];
        //for (int i = 0; i < vertices.Count; i++)
        //    vertices[i] -= vertex0;

        double volume = 0;
        double area = 0;
        for (int i = 0; i < indices.Count; i += 3)
        {
            Matrix4x4 myMatrix = new(
                new Vector4(vertices[indices[i]].x, vertices[indices[i]].y, vertices[indices[i]].z, 0),
                new Vector4(vertices[indices[i + 1]].x, vertices[indices[i + 1]].y, vertices[indices[i + 1]].z, 0),
                new Vector4(vertices[indices[i + 2]].x, vertices[indices[i + 2]].y, vertices[indices[i + 2]].z, 0),
                new Vector4(0, 0, 0, 1)
            );
            volume += myMatrix.determinant / 6;
            area += Vector3.Cross(vertices[indices[i + 1]] - vertices[indices[i]], vertices[indices[i + 2]] - vertices[indices[i]]).magnitude / 2;
        }

        Debug.Log("膠囊體:");
        Debug.Log("正確體積為: " + (System.Math.PI * 0.5 * 0.5 * 0.5 * 4 / 3 + System.Math.PI * 0.5 * 0.5 * 1));
        Debug.Log("計算體積為: " + volume);
        Debug.Log("正確表面積為: " + (4 * System.Math.PI * 0.5 * 0.5 + 2 * System.Math.PI * 0.5 * 1));
        Debug.Log("計算表面積為: " + area);
        Debug.Log("-----------------------------------------------");
    }
}

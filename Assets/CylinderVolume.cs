using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class CylinderVolume : MonoBehaviour
{
    private int lastNumberOfSegments;
    [SerializeField] private int numberOfSegments = 20;

    void Start()
    {
        lastNumberOfSegments = numberOfSegments;

        CreateCylinder();
        CalculateVolume();
    }

    void Update()
    {
        if (lastNumberOfSegments == numberOfSegments)
            return;
        lastNumberOfSegments = numberOfSegments;

        CreateCylinder();
        CalculateVolume();
    }

    void CreateCylinder()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        List<Vector3> vertices = new();
        List<int> indices = new();
        List<Vector3> normals = new();
        meshFilter.mesh = new Mesh
        {
            indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
        };
        for (int i = 0; i < numberOfSegments; i++)
        {
            float angleSegment = (float)(2 * System.Math.PI / numberOfSegments);
            float angle = angleSegment * i;
            vertices.Add(new Vector3(math.cos(angle) / 2, -1, math.sin(angle) / 2));
            vertices.Add(new Vector3(math.cos(angle) / 2, 1, math.sin(angle) / 2));
            normals.Add(new Vector3(math.cos(angle), 0, math.sin(angle)));
            normals.Add(new Vector3(math.cos(angle), 0, math.sin(angle)));
            indices.Add(i * 2);
            indices.Add(i * 2 + 1);
            indices.Add(i * 2 + 2);
            indices.Add(i * 2 + 3);
            indices.Add(i * 2 + 2);
            indices.Add(i * 2 + 1);
        }
        vertices.Add(new Vector3(.5f, -1, 0));
        vertices.Add(new Vector3(.5f, 1, 0));
        normals.Add(new Vector3(1, 0, 0));
        normals.Add(new Vector3(1, 0, 0));
        for (int i = 0; i < numberOfSegments; i++)
        {
            float angleSegment = (float)(2 * System.Math.PI / numberOfSegments);
            float angle = angleSegment * i;
            vertices.Add(new Vector3(math.cos(angle) / 2, -1, math.sin(angle) / 2));
            vertices.Add(new Vector3(math.cos(angle) / 2, 1, math.sin(angle) / 2));
            normals.Add(new Vector3(0, -1, 0));
            normals.Add(new Vector3(0, 1, 0));
            indices.Add(numberOfSegments * 4 + 4);
            indices.Add(numberOfSegments * 2 + i * 2 + 2);
            indices.Add(numberOfSegments * 2 + i * 2 + 4);
            indices.Add(numberOfSegments * 4 + 5);
            indices.Add(numberOfSegments * 2 + i * 2 + 5);
            indices.Add(numberOfSegments * 2 + i * 2 + 3);
        }
        vertices.Add(new Vector3(.5f, -1, 0));
        vertices.Add(new Vector3(.5f, 1, 0));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, 1, 0));
        vertices.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));
        vertices.Add(new Vector3(0, 1, 0));
        normals.Add(new Vector3(0, 1, 0));

        meshFilter.mesh.SetVertices(vertices);
        meshFilter.mesh.SetNormals(normals);
        meshFilter.mesh.SetIndices(indices, MeshTopology.Triangles, 0);
    }

    void CalculateVolume()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        List<Vector3> vertices = new();
        List<Vector3> normals = new();
        List<int> indices = new();
        meshFilter.mesh.GetVertices(vertices);
        meshFilter.mesh.GetNormals(normals);
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

        Debug.Log("圓柱體:");
        Debug.Log("正確體積為: " + System.Math.PI * 0.5 * 0.5 * 2);
        Debug.Log("計算體積為: " + volume);
        Debug.Log("正確表面積為: " + (System.Math.PI * 0.5 * 0.5 * 2 + 2 * System.Math.PI * 0.5 * 2));
        Debug.Log("計算表面積為: " + area);
        Debug.Log("-----------------------------------------------");
    }
}

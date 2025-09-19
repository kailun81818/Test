using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class BallVolume : MonoBehaviour
{
    private int lastNumberOfSegments;
    [SerializeField] private int numberOfSegments = 20;

    enum SphereSide
    {
        Right,
        Left,
        Top,
        Bottom,
        Front,
        Back
    }

    private Vector3 GetNormals(int i, int j, SphereSide sphereSide)
    {
        Vector3 spotOnCubeSurface = sphereSide switch
        {
            SphereSide.Right => new Vector3(1, -1 + 2f * i / numberOfSegments, -1 + 2f * j / numberOfSegments),
            SphereSide.Left => new Vector3(-1, -1 + 2f * j / numberOfSegments, -1 + 2f * i / numberOfSegments),
            SphereSide.Top => new Vector3(-1 + 2f * j / numberOfSegments, 1, -1 + 2f * i / numberOfSegments),
            SphereSide.Bottom => new Vector3(-1 + 2f * i / numberOfSegments, -1, -1 + 2f * j / numberOfSegments),
            SphereSide.Front => new Vector3(-1 + 2f * i / numberOfSegments, -1 + 2f * j / numberOfSegments, 1),
            SphereSide.Back => new Vector3(-1 + 2f * j / numberOfSegments, -1 + 2f * i / numberOfSegments, -1),
            _ => throw new System.ArgumentOutOfRangeException(nameof(sphereSide), sphereSide, null)
        };
        return spotOnCubeSurface.normalized;
    }
    void Start()
    {
        lastNumberOfSegments = numberOfSegments;

        CreateBall();
        CalculateVolume();
    }

    void Update()
    {
        if (lastNumberOfSegments == numberOfSegments)
            return;
        lastNumberOfSegments = numberOfSegments;

        CreateBall();
        CalculateVolume();
    }

    void CreateBall()
    {

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        List<Vector3> vertices = new();
        List<Vector3> normals = new();
        List<int> indices = new();
        meshFilter.mesh = new Mesh
        {
            indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
        };
        for (SphereSide sphereSide = SphereSide.Right; sphereSide <= SphereSide.Back; sphereSide++)
        {
            for (int i = 0; i <= numberOfSegments; i++)
            {
                for (int j = 0; j <= numberOfSegments; j++)
                {
                    Vector3 normal = GetNormals(i, j, sphereSide);
                    vertices.Add(normal * .5f);
                    normals.Add(normal);
                    if (i < numberOfSegments && j < numberOfSegments)
                    {
                        int index = vertices.Count - 1;
                        indices.Add(index);
                        indices.Add(index + numberOfSegments + 1);
                        indices.Add(index + numberOfSegments + 2);
                        indices.Add(index);
                        indices.Add(index + numberOfSegments + 2);
                        indices.Add(index + 1);
                    }
                }
            }
        }
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

        Debug.Log("球體:");
        Debug.Log("正確體積為: " + System.Math.PI * 0.5 * 0.5 * 0.5 * 4 / 3);
        Debug.Log("計算體積為: " + volume);
        Debug.Log("正確表面積為: " + 4 * System.Math.PI * 0.5 * 0.5);
        Debug.Log("計算表面積為: " + area);
        Debug.Log("-----------------------------------------------");
    }
}

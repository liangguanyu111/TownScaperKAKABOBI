using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Edge 
{
    public readonly HashSet<vertex_Hex> hexes;


    public HashSet<Triangle> adjacentedTriangles;
    public readonly int edgeID = 0;


    public readonly vertex_Mid vertexMid;

    

    public Edge(vertex_Hex a,vertex_Hex b,Triangle triangle)
    {
        hexes = new HashSet<vertex_Hex> {a, b};
        adjacentedTriangles = new HashSet<Triangle> { triangle };

        vertexMid = new vertex_Mid(this);
        edgeID = Grid.edgeIdCount++;
    }


    public List<vertex_Hex> ReturnVertices()
    {
        List<vertex_Hex> vertices = new List<vertex_Hex>();
        foreach (var vertex in hexes)
        {
            vertices.Add(vertex);
        }

        return vertices;
    }

    public List<Triangle> ReturnTriangles()
    {
        List<Triangle> triangles = new List<Triangle>();
        foreach (var triangle in triangles)
        {
            triangles.Add(triangle);
        }

        return triangles;
    }

    public bool CheckReapeatEdge(vertex_Hex vertexA, vertex_Hex vertexB)
    {
        return hexes.Contains(vertexA) && hexes.Contains(vertexB);
    }

    public void FindAdjacentedTriangle(Triangle triangle)
    {
        if(adjacentedTriangles.Count<2)
        {
            adjacentedTriangles.Add(triangle);
        }
        else
        {
            Debug.Log("边的创建出现了异常");
        }
    }

    public bool CheckReapeatEdge(Edge newEdge)
    {
        return newEdge.hexes.IsProperSubsetOf(hexes);
    }

    public Quad Merge(List<Triangle> triangles)
    {
        if(CanMerge())
        {
            List<vertex_Hex> quadVertices = new List<vertex_Hex>();
            foreach (var triangle in adjacentedTriangles)
            {
                triangles.Remove(triangle);
                triangle.isMerged = true;

                vertex_Hex vertexA = triangle.GetSingleVertex(this);
                quadVertices.Add(vertexA); 
                quadVertices.Add(triangle.vertices[(Array.IndexOf(triangle.vertices, vertexA) + 1) % 3]);
            }

            return new Quad(quadVertices);
        }
        return null;
    }
    

    public bool CanMerge()
    {
        foreach (var triganle in adjacentedTriangles)
        {
            if(triganle.isMerged)
            {
                return false;
            }
        }
        return adjacentedTriangles.Count==2;
    }
}

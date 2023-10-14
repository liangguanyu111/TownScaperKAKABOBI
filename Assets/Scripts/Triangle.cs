using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Triangle
{
    public readonly vertex_Hex a;
    public readonly vertex_Hex b;
    public readonly vertex_Hex c;

    public readonly vertex_Hex[] vertices;
    public readonly Edge ab;
    public readonly Edge bc;
    public readonly Edge ac;

    public readonly Edge[] edges;

    public readonly vertex_triangleCenter triangelCenter;

    public bool isMerged = false;
    public Triangle(vertex_Hex a, vertex_Hex b, vertex_Hex c)
    {
        this.a = a;
        this.c = c;
        this.b = b;

        ab = Grid.GetEdge(a, b, this);
        bc = Grid.GetEdge(b, c, this);
        ac = Grid.GetEdge(a, c, this);
        edges = new Edge[] { ab, bc, ac };
        vertices = new vertex_Hex[] { a, b, c };

        triangelCenter = new vertex_triangleCenter(this);
    }

    //获取除了相邻边之外剩下哪个顶点
    public vertex_Hex GetSingleVertex(Edge edge)
    {
        HashSet<vertex_Hex> exception = new HashSet<vertex_Hex>(vertices);
        exception.ExceptWith(edge.hexes);
        return exception.Single();
    }
    public static List<Triangle> TrigleRing(int radius,List<vertex_Hex> vertices)
    {
        List<Triangle> triangles = new List<Triangle>();
        List<vertex_Hex> inner = vertex_Hex.GrabRing(radius - 1, vertices);
        List<vertex_Hex> outer = vertex_Hex.GrabRing(radius, vertices);

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < radius; j++)
            {
                vertex_Hex a = outer[i * radius + j];
                vertex_Hex b = outer[(i * radius + j + 1) % outer.Count];
                vertex_Hex c = inner[(i * (radius - 1) + j) % inner.Count];
                triangles.Add(new Triangle(a, b, c));

                if(j>0)
                {
                    vertex_Hex d = inner[i * (radius - 1) + j - 1];
                    triangles.Add(new Triangle(a, c, d));
                }
            }
        }

        return triangles;
    }
    
    public List<SubQuad> dividedToSubQuad()
    {

        List<SubQuad> subquads = new List<SubQuad>();
        ab.vertexMid.isSp = true;
        ab.vertexMid.isSp = true;
        ac.vertexMid.isSp = true;
        subquads.Add(new SubQuad(a, ab.vertexMid, triangelCenter, ac.vertexMid));
        subquads.Add(new SubQuad(b, bc.vertexMid, triangelCenter, ab.vertexMid));
        subquads.Add(new SubQuad(c, ac.vertexMid, triangelCenter, bc.vertexMid));
        return subquads;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Grid
{
    public static int radius;
    public static float cellSize;
    public static float cellHeight;
    public static int height;
    public static int idCount = 0;
    public static int edgeIdCount = 0;

    public int smoothTime;

    public readonly List<vertex_Hex> hexes = new List<vertex_Hex>();

    public readonly List<Triangle> triganles = new List<Triangle>();

    public static List<Edge>  edges = new List<Edge>();

    public readonly List<Quad> quads = new List<Quad>();

    public readonly List<SubQuad> subquads = new List<SubQuad>();

    public readonly List<vertex> allVetices = new List<vertex>();
    public readonly List<vertex_center> centerVertices = new List<vertex_center>();
    public readonly List<vertex_Mid> midVertices = new List<vertex_Mid> ();
    public readonly List<SubQuad_Cube> subQuadCubes = new List<SubQuad_Cube>();
    public Grid(int radius, float cellSize,float cellHeight,int height,int smoothTime)
    {
        Grid.radius = radius;
        Grid.cellSize = cellSize;
        this.smoothTime = smoothTime;
        Grid.cellHeight = cellHeight;
        Grid.height = height;

        Hex();   //六边形点阵
        Triganle_Hex();  //三角形

        MergeTriangles();

        SubDividedQuadAndTriangle();

        SmoothVertex();

        Build3dGrid();

        BuildCube();
    }

    //初始化六边形网格矩阵
    public void Hex()
    {
        foreach (Coord coord in Coord.Coord_Hex())
        {
            hexes.Add(new vertex_Hex(coord));
        }

        allVetices.AddRange(hexes);
    }

    //初始化三角形
    public void Triganle_Hex()
    {
        for (int i = 1; i <= Grid.radius; i++)
        {
            triganles.AddRange(Triangle.TrigleRing(i, hexes));
        }
    }


    public static Edge GetEdge(vertex_Hex vertexA,vertex_Hex vertexB,Triangle triangle = null)
    {
        foreach (var edge in edges)
        {
            if(edge.CheckReapeatEdge(vertexA,vertexB))
            {
                if(triangle!=null)
                //？查找到重复边意味着找到了相邻三角形
                edge.FindAdjacentedTriangle(triangle);
                return edge;
            }
        }
        Edge newEdge = new Edge(vertexA, vertexB, triangle);
        edges.Add(newEdge);
        return newEdge;
    }

    //为添加的新边检查是否重复
    public void AddEdge( Edge newEdge,Triangle triangle)
    {
        if(edges.Count<=0)
        {
            edges.Add(newEdge);
            return;
        }

        foreach (var edge in edges)
        {
            if(newEdge.CheckReapeatEdge(edge))
            {
                edge.FindAdjacentedTriangle(triangle);
                return;
            }
        }
        edges.Add(newEdge);
    }

    public Edge FindEdge(vertex_Hex a,vertex_Hex b)
    {
        foreach (var edge in edges)
        {
            if(edge.hexes.Contains(a)&&edge.hexes.Contains(b))
            {
                if(edge.vertexMid.isSp)
                {
                    
                }
                return  edge;
            }
        }

        return null;
    }

    public void MergeTriangles()
    {
       var random = new System.Random();
       var resultList = new List<Edge>();

       for (int i = 0; i < edges.Count; i++)
       {
           resultList.Add(edges[i]);
        }

       while(resultList.Count>0)
        {
            int index = UnityEngine.Random.Range(0, resultList.Count);

            if (resultList[index].CanMerge())
            {
                quads.Add(resultList[index].Merge(triganles));
                edges.Remove(resultList[index]);
            }
            resultList.Remove(resultList[index]);
        }

        foreach (var edge in edges)
        {
            allVetices.Add(edge.vertexMid);
        }
    }
     

    public void SubDividedQuadAndTriangle()
    {

        foreach (var triganle in triganles)
        {
            subquads.AddRange(triganle.dividedToSubQuad());
            //centerVertices.Add(triganle.triangelCenter);
            allVetices.Add(triganle.triangelCenter);
        }

        foreach (var quad in quads)
        {
            subquads.AddRange(quad.dividedToSubQuad());
            //centerVertices.Add(quad.quadCenter);
            allVetices.Add(quad.quadCenter);
        }
    }

    public void SubQuadSmooth()
    {
        foreach (var subquad in subquads)
        {
            subquad.CaculateSmooth();
        }
    }

    public void SmoothVertex()
    {
        for (int i = 0; i < smoothTime; i++)
        {
            SubQuadSmooth();
        }

        foreach (var vertex in allVetices)
        {
            vertex.Smooth();
        }
    }

    public void Build3dGrid()
    {
        List<vertex_Y> vertexYList = new List<vertex_Y>();
        for (int i = 0; i < height; i++)
        {
            foreach (var vertex in allVetices)
            {
                vertex_Y newVertexY = new vertex_Y(vertex, i);
                vertex.vertex_Ys.Add(newVertexY);
                vertexYList.Add(newVertexY);
            }
        }
        allVetices.AddRange(vertexYList);
    }

    public void BuildCube()
    {
        for (int i = 0; i < height-1; i++)
        {
            foreach (var subQuad in subquads)
            {
                SubQuad_Cube newCube = new SubQuad_Cube(subQuad, i);
                subQuadCubes.Add(newCube);
                newCube.UpdateBitValue();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class vertex
{
    public Vector3 initPos;

    public Vector3 currentPosition;

    public Vector3 offset;

    public List<vertex_Y> vertex_Ys = new List<vertex_Y>();

    public int id  = 0;

    public bool isActive = false;
    public void Smooth()
    {
        // Debug.Log("位移差 :" + offset);
        currentPosition = initPos + offset;
    }
    public Action OnVertexStatusChange;
    
    public void SetVertexStatus(bool value)
    {
        if(value!=isActive)
        {
            isActive = value;
            OnVertexStatusChange?.Invoke();
        }
    }
}
public class Coord
{
    public readonly int q;
    public readonly int r;
    public readonly int s;

    public readonly Vector3 worldPos;
    public Coord(int q,int r,int s)
    {
        this.q = q;
        this.r = r;
        this.s = s;
        worldPos = worldPosition();
    }

    static public Coord[] directions = new Coord[]
    {
        new Coord(0,1,-1),
        new Coord(-1,1,0),
        new Coord(-1,0,1),
        new Coord(0,-1,1),
        new Coord(1,-1,0),
        new Coord(1,0,-1)
    };

    static public Coord Direction(int direction)
    {
        return directions[direction];
    }

    public Coord Add(Coord coord)
    {
        return new Coord(q + coord.q, r + coord.r, s + coord.s);
    }

    public Coord Scale(int k)
    {
        return new Coord(q * k, r * k, s * k);
    }

    public Coord Neighbor(int direction)
    {
        return Add(Direction(direction));
    }

    public Vector3 worldPosition()
    {
        return new Vector3(q * Mathf.Sqrt(3) / 2, 0, -(float)r - ((float)q / 2)) *2*Grid.cellSize;
    }

    //计算单一环
    public static List<Coord> GetCoordRing(int radius)
    {
        List<Coord> rings = new List<Coord>();

        if(radius==0)
        {
            rings.Add(new Coord(0, 0, 0));
        }
        else
        {
            Coord coord = Coord.Direction(4).Scale(radius);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    rings.Add(coord);
                    coord = coord.Neighbor(i);
                }
            }
        }
        return rings;
    }

    //计算六边形网格

    public static List<Coord> Coord_Hex()
    {
        List<Coord> hex = new List<Coord>();

        for (int i = 0; i <= Grid.radius; i++)
        {
            hex.AddRange(GetCoordRing(i));
        }

        return hex;
    }
}

public class vertex_Hex :vertex
{
    public readonly Coord coord;

    public vertex_Hex(Coord coord)
    {
        this.coord = coord;
        initPos = coord.worldPos;
        currentPosition = initPos;
    }


    public static List<vertex_Hex> GrabRing(int radius, List<vertex_Hex> vertices)
    {
        if(radius<=0)
        {
            return vertices.GetRange(0, 1);
        }
        return vertices.GetRange(radius * (radius - 1) * 3 + 1, radius * 6);
    }

}
public class vertex_Mid : vertex
{
    public bool isSp = false;
    public vertex_Mid(Edge edge)
    {
        Vector3 pos = Vector3.zero;
        foreach (var vertex in edge.hexes)
        {
            pos += vertex.initPos;
        }
        initPos = pos / 2;

        currentPosition = initPos;
        id = Grid.idCount++;
    }
}

public class vertex_center: vertex { }

public class vertex_triangleCenter :vertex_center
{
     public  vertex_triangleCenter(Triangle triangle)
     {
        initPos = ((triangle.a.initPos + triangle.b.initPos + triangle.c.initPos)) / 3;
        currentPosition = initPos;
     }
}

public class vertex_quadCenter: vertex_center
{
    public vertex_quadCenter(Quad quad)
    {
        initPos = ((quad.a.initPos +quad.b.initPos+quad.c.initPos+quad.d.initPos)) / 4;
        currentPosition = initPos;
    }
}

public class vertex_Y: vertex
{
    public vertex vertex;
    public float Height;
    public Vector3 worldPostion;

    public vertex_Y(vertex vertex,float height)
    {
        worldPostion = vertex.currentPosition + new Vector3(0, Grid.cellHeight * height, 0);
        currentPosition = worldPostion;
    }
}
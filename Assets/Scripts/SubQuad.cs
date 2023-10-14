using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubQuad 
{
    public readonly vertex_Hex a;
    public readonly vertex_Mid b;
    public readonly vertex_center c;
    public readonly vertex_Mid d;

    public SubQuad(vertex_Hex a,vertex_Mid b,vertex_center c, vertex_Mid d)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
    }
    public void CaculateSmooth()
    {
        Vector3 center = (a.currentPosition + b.currentPosition + c.currentPosition + d.currentPosition) / 4;

        Vector3 vectorNew_a = (a.currentPosition
            + Quaternion.AngleAxis(-90, Vector3.up) * (b.currentPosition - center) + center
            + Quaternion.AngleAxis(-180, Vector3.up) * (c.currentPosition - center) + center
               + Quaternion.AngleAxis(-270, Vector3.up) * (d.currentPosition - center) + center
            ) / 4;
        Vector3 vectorNew_b = Quaternion.AngleAxis(90, Vector3.up) * (vectorNew_a - center) + center;
        Vector3 vectorNew_c = Quaternion.AngleAxis(180, Vector3.up) * (vectorNew_a - center) + center;
        Vector3 vectorNew_d = Quaternion.AngleAxis(270, Vector3.up) * (vectorNew_a - center) + center;

        a.offset += (vectorNew_a - a.currentPosition) * 0.1f;
        b.offset += (vectorNew_b - b.currentPosition) * 0.1f;
        c.offset += (vectorNew_c - c.currentPosition) * 0.1f;
        d.offset += (vectorNew_d - d.currentPosition) * 0.1f;

        //Debug.Log("Æ½»¬Êý¾Ý: a:" + a.offset + "b:" + b.offset + "c:" + c.offset + "d:" + d.offset);

    }
}

public class SubQuad_Cube
{
    public SubQuad subQuad;
    public int y;

    public vertex[] vertices = new vertex[8];
    public Vector3 centerPos;
    public string bitValue;
    public SubQuad_Cube(SubQuad subQuad,int y)
    {
        this.subQuad = subQuad;
        this.y = y;

        vertices[0] = subQuad.a.vertex_Ys[y + 1];
        vertices[1] = subQuad.b.vertex_Ys[y + 1];
        vertices[2] = subQuad.c.vertex_Ys[y + 1];
        vertices[3] = subQuad.d.vertex_Ys[y + 1];
        vertices[4] = subQuad.a.vertex_Ys[y];
        vertices[5] = subQuad.b.vertex_Ys[y];
        vertices[6] = subQuad.c.vertex_Ys[y];
        vertices[7] = subQuad.d.vertex_Ys[y];

        centerPos = GetCenterPos();
    }

    public Vector3 GetCenterPos()
    {
        Vector3 tempPos = Vector3.zero;
        foreach (var item in vertices)
        {
            tempPos += item.currentPosition;
        }
        return tempPos / 8;
    }

    public void UpdateBitValue()
    {
        bitValue = string.Empty;
        foreach (var vertex in vertices)
        {
            if(vertex.isActive)
            {
                bitValue+="1";
            }
            else
            {
                bitValue += "0";
            }
        }
    }
}
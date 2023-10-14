using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Quad 
{
    public readonly vertex_Hex a;
    public readonly vertex_Hex b;
    public readonly vertex_Hex c;
    public readonly vertex_Hex d;

    public  Edge ab;
    public  Edge bc;
    public  Edge cd;
    public  Edge ad;

    public vertex_quadCenter quadCenter;

    public Quad(List<vertex_Hex> vertices)
    {
        this.a = vertices[0];
        this.b = vertices[1];
        this.c = vertices[2];
        this.d = vertices[3];

        this.ab = Grid.GetEdge(this.a, this.b);
        this.bc = Grid.GetEdge(this.b, this.c);
        this.cd = Grid.GetEdge(this.c, this.d);
        this.ad = Grid.GetEdge(this.a, this.d);


        quadCenter = new vertex_quadCenter(this);
    }


    public List<SubQuad> dividedToSubQuad()
    {
        List<SubQuad> subquads = new List<SubQuad>();
        subquads.Add(new SubQuad(a, ab.vertexMid, quadCenter, ad.vertexMid));
        subquads.Add(new SubQuad(b, bc.vertexMid, quadCenter, ab.vertexMid));
        subquads.Add(new SubQuad(c, cd.vertexMid, quadCenter, bc.vertexMid));
        subquads.Add(new SubQuad(d, ad.vertexMid, quadCenter, cd.vertexMid));
        return subquads;
    }
}

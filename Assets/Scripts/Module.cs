using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Module
{
    public string name;
    public Mesh mesh;
    public int rotation;
    public bool filp;
    public Module(string name, Mesh mesh, int rotation, bool flip)
    {
        this.name = name;
        this.mesh = mesh;
        this.rotation = rotation;
        this.filp = flip;
    }
}

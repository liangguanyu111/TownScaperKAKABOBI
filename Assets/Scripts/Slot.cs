using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Slot : MonoBehaviour
{
    public List<Module> possibleModules;

    public SubQuad_Cube SubQuad_Cube;

    [SerializeField]
    public GameObject module;

    public Material material;

    private void Awake()
    {
        module = new GameObject("Module", typeof(MeshFilter), typeof(MeshRenderer));

        module.transform.SetParent(transform);

        module.transform.localPosition = Vector3.zero;
        //module.transform.localRotation = Quaternion.identity;
    }

    public void Intialize(SubQuad_Cube subQuad_Cube,Material material)
    {
        this.SubQuad_Cube = subQuad_Cube;
        this.possibleModules = ModuleLibrary.GetMoudlByBit(subQuad_Cube.bitValue);
        this.material = material;
    }

    public void ReSetPossibleModules()
    {
        this.possibleModules = ModuleLibrary.GetMoudlByBit(SubQuad_Cube.bitValue);
        if(this.possibleModules.Count<=0)
        {
            throw new System.Exception("Module重组时没有找到对应模块");
        }
    }
    public void RotateModule(Mesh mesh,int rotation)
    {      
       Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = Quaternion.AngleAxis(90 * (rotation), Vector3.up) * vertices[i];
        }
        mesh.vertices = vertices;
    }

    public void FlipModule(Mesh mesh,bool flip)
    {
        if(flip)
        {
            Vector3[] vertices = mesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(-vertices[i].x, vertices[i].y, vertices[i].z);
            }
            mesh.vertices = vertices;
            mesh.triangles = mesh.triangles.Reverse().ToArray();
        }
    }

    public void DeformModule(Mesh mesh,SubQuad_Cube subQuadCube)
    {
        Vector3[] vertices = mesh.vertices;

        SubQuad subQuad = subQuadCube.subQuad;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 ad_x = Vector3.Lerp(subQuad.a.currentPosition, subQuad.d.currentPosition, (vertices[i].x + 0.5f));
            Vector3 bc_x =Vector3.Lerp(subQuad.b.currentPosition,subQuad.c.currentPosition, (vertices[i].x + 0.5f));
            vertices[i] = Vector3.Lerp(ad_x, bc_x, (vertices[i].z + 0.5f)) + Vector3.up * vertices[i].y * Grid.cellHeight - subQuad.GetCenterPos();
        }
        mesh.vertices = vertices;
    }

    public void UpdateModule(Module module)
    {
        this.module.GetComponent<MeshFilter>().mesh = module.mesh;
        Mesh mesh = this.module.GetComponent<MeshFilter>().mesh;
        FlipModule(mesh, module.filp);
        RotateModule(mesh, module.rotation);
        DeformModule(mesh, SubQuad_Cube);

        this.module.GetComponent<MeshRenderer>().material = material;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}

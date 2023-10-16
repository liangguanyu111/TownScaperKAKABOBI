using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class GridGenerator : MonoBehaviour
{
    public int radius;
    public float cellSize;
    public float cellHight;
    public int height;
    public int smoothTime;

    private Grid grid;

    [Header("Debug")]
    public GameObject testCube;
    [Header("Module")]
    [SerializeField]
    public ModuleLibrary moduleLibrary;


    private void Awake()
    {
        moduleLibrary = Instantiate(moduleLibrary);
        moduleLibrary.ImportedModule();
        grid = new Grid(radius, cellSize, cellHight, height, smoothTime);
    }


    private void Update()
    {
        foreach (var vertex in grid.vertexYList)
        {
            if(Vector3.Distance(testCube.transform.position,vertex.currentPosition)<2f)
            {
                vertex.SetVertexStatus(true);
            }
            else if(vertex.isActive && Vector3.Distance(testCube.transform.position, vertex.currentPosition) > 2f)
            {
                vertex.SetVertexStatus(false);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (grid != null)
        {
            Gizmos.color = Color.white;

            //foreach (var item in grid.subquads)
            //{
            //    Gizmos.DrawLine(item.a.currentPosition, item.b.currentPosition);
            //    Gizmos.DrawLine(item.b.currentPosition, item.c.currentPosition);
            //    Gizmos.DrawLine(item.c.currentPosition, item.d.currentPosition);
            //    Gizmos.DrawLine(item.a.currentPosition, item.d.currentPosition);

            //    //Handles.Label(item.ab.vertexMid.initPos, item.ab.edgeID.ToString());
            //    //Handles.Label(item.bc.vertexMid.initPos, item.bc.edgeID.ToString());
            //    //Handles.Label(item.cd.vertexMid.initPos, item.cd.edgeID.ToString());
            //    //Handles.Label(item.ad.vertexMid.initPos, item.ad.edgeID.ToString());
            //}


            //foreach (var item in grid.allVetices)
            //{
            //    foreach (var vertexY in item.vertex_Ys)
            //    {
            //        if (vertexY.isActive)
            //        {
            //            Gizmos.color = Color.green;
            //        }
            //        else
            //        {
            //            Gizmos.color = Color.white;
            //        }
            //        Gizmos.DrawSphere(vertexY.currentPosition, 0.1f);
            //    }
            //}

            foreach (var item in grid.subQuadCubes)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(item.vertices[0].currentPosition, item.vertices[1].currentPosition);
                Gizmos.DrawLine(item.vertices[1].currentPosition, item.vertices[2].currentPosition);
                Gizmos.DrawLine(item.vertices[2].currentPosition, item.vertices[3].currentPosition);
                Gizmos.DrawLine(item.vertices[0].currentPosition, item.vertices[3].currentPosition);

                Gizmos.DrawLine(item.vertices[4].currentPosition, item.vertices[5].currentPosition);
                Gizmos.DrawLine(item.vertices[5].currentPosition, item.vertices[6].currentPosition);
                Gizmos.DrawLine(item.vertices[6].currentPosition, item.vertices[7].currentPosition);
                Gizmos.DrawLine(item.vertices[4].currentPosition, item.vertices[7].currentPosition);

                Gizmos.DrawLine(item.vertices[0].currentPosition, item.vertices[4].currentPosition);
                Gizmos.DrawLine(item.vertices[1].currentPosition, item.vertices[5].currentPosition);
                Gizmos.DrawLine(item.vertices[2].currentPosition, item.vertices[6].currentPosition);
                Gizmos.DrawLine(item.vertices[3].currentPosition, item.vertices[7].currentPosition);

                //GUI.color = Color.red;
                //Handles.Label(item.centerPos, item.bitValue);


                //Gizmos.color = Color.blue;
                //Gizmos.DrawSphere(item.vertices[0].currentPosition, 0.15f);
            }

     

            //foreach (var item in grid.subquads)
            //{
            //    Gizmos.color = Color.green;
            //    DrawMyDirection(item.arrowPos[0], item.arrowPos[1]);
            //    DrawMyDirection(item.arrowPos[1], item.arrowPos[2]);
            //    DrawMyDirection(item.arrowPos[2], item.arrowPos[3]);
            //    DrawMyDirection(item.arrowPos[3], item.arrowPos[0]);
            //}

        }
    }

    public void DrawMyDirection(Vector3 posFrom,Vector3 posTo)
    {
        Gizmos.DrawLine(posFrom, posTo);

        Vector3 lineDirection = (posFrom - posTo).normalized;
        Vector3 arrowDirection = Quaternion.Euler(0,90,0) * lineDirection;

        Vector3 midPos = (posFrom + posTo) / 2;

        Vector3 arrowUp = midPos + (lineDirection * Vector3.Distance(posTo, posFrom) * 0.15f) + arrowDirection * 0.15f;
        Vector3 arrowDown = midPos + (lineDirection * Vector3.Distance(posTo, posFrom) * 0.15f) - arrowDirection * 0.15f;

        Gizmos.DrawLine(midPos, arrowUp);
        Gizmos.DrawLine(midPos, arrowDown);
    }
}

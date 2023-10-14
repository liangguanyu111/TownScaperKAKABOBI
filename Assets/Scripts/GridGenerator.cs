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
    private void Start()
    {
        grid = new Grid(radius,cellSize,cellHight,height,smoothTime);
    }


    void OnDrawGizmos()
    {
        if (grid != null)
        {
            Gizmos.color = Color.white;

            foreach (var item in grid.subquads)
            {
                Gizmos.DrawLine(item.a.currentPosition, item.b.currentPosition);
                Gizmos.DrawLine(item.b.currentPosition, item.c.currentPosition);
                Gizmos.DrawLine(item.c.currentPosition, item.d.currentPosition);
                Gizmos.DrawLine(item.a.currentPosition, item.d.currentPosition);

                //Handles.Label(item.ab.vertexMid.initPos, item.ab.edgeID.ToString());
                //Handles.Label(item.bc.vertexMid.initPos, item.bc.edgeID.ToString());
                //Handles.Label(item.cd.vertexMid.initPos, item.cd.edgeID.ToString());
                //Handles.Label(item.ad.vertexMid.initPos, item.ad.edgeID.ToString());
            }

            Gizmos.color = Color.blue;

            foreach (var item in grid.allVetices)
            {
                Gizmos.DrawSphere(item.currentPosition, 0.2f);
            }

            Gizmos.color = Color.green;

            foreach (var item in grid.subQuadCubes)
            {
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

                GUI.color = Color.white;
                Handles.Label(item.centerPos, item.bitValue);
            }

        }
    }
}

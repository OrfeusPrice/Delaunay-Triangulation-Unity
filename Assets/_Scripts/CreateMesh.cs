using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Versioning;
using Unity.VisualScripting;
using UnityEngine;
using Color = UnityEngine.Color;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CreateMesh : MonoBehaviour
{
    private Mesh _mesh;
    private MeshFilter MF;
    private MeshRenderer MR;
    private MakeLineFromMesh MLFM;
    public int X, Y, Z;
    public List<GameObject> _points = new List<GameObject>();
    public List<Vector3> _pointsPos = new List<Vector3>();
    public Vector2[] _pointsPos2;
    public GameObject _pref;
    public Color color;
    public Triangulator tr;
    //public static Dictionary<PointF, float> CIRCLES;

    void Start()
    {
        color = Color.yellow;
        _mesh = new Mesh();
        //CIRCLES = new Dictionary<PointF, float>();

        MF = GetComponent<MeshFilter>();
        MR = GetComponent<MeshRenderer>();
        MLFM = GetComponent<MakeLineFromMesh>();

        // for (int i = 0; i < X; i++)
        // {
        //     for (int j = 0; j < Y; j++)
        //     {
        //         for (int k = 0; k < Z; k++)
        //         {
        //             GameObject temp = Instantiate(_pref);
        //             temp.transform.position = new Vector3(i, j, k);
        //             temp.name = $"{temp.transform.position.x};{temp.transform.position.y}";
        //             _points.Add(temp);
        //         }
        //     }
        // }

        foreach (var item in _points)
        {
            _pointsPos.Add(item.transform.position);
        }

        _pointsPos2 = new Vector2[_points.Count];
        for (int i = 0; i < _points.Count; i++)
        {
            _pointsPos2[i] = _points[i].transform.position;
        }

        tr = new Triangulator();
        MF.mesh = tr.CreatePolygon(_pointsPos2);
        StartCoroutine(IEChange());
    }

    // private void Update()
    // {
    //     Change();
    // }

    private void OnDrawGizmos()
    {
        // Gizmos.color = color;
        // foreach (var item in CIRCLES)
        // {
        //     Gizmos.DrawWireSphere(new Vector3(item.Key.X, item.Key.Y, 0), item.Value);
        // }

        //     Gizmos.color = color;
        //     if (_pointsPos.Count > 1)
        //         for (int i = 0; i < MF.mesh.triangles.Length - 2; i++)
        //         {
        //             Gizmos.DrawLine(_pointsPos[MF.mesh.triangles[i]], _pointsPos[MF.mesh.triangles[i + 1]]);
        //             Gizmos.DrawLine(_pointsPos[MF.mesh.triangles[i]], _pointsPos[MF.mesh.triangles[i + 2]]);
        //             Gizmos.DrawLine(_pointsPos[MF.mesh.triangles[i + 1]], _pointsPos[MF.mesh.triangles[i + 2]]);
        //         }
    }

    public void Change()
    {
        _pointsPos.Clear();
        foreach (var item in _points)
        {
            _pointsPos.Add(item.transform.position);
            item.name = $"{item.transform.position.x};{item.transform.position.y}";
        }

        _pointsPos2 = new Vector2[_points.Count];
        for (int i = 0; i < _points.Count; i++)
        {
            _pointsPos2[i] = _points[i].transform.position;
        }

        MF.mesh = tr.CreatePolygon(_pointsPos2);
        MLFM.updateMesh();
    }

    IEnumerator IEChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            if (_points.Count >= 1)
                //CIRCLES.Clear();
                Change();
        }
    }
}
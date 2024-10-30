using UnityEngine;
using System.Collections.Generic;

struct Triangle
{
    public int P1 { get; }
    public int P2 { get; }
    public int P3 { get; }

    public Triangle(int point1, int point2, int point3)
    {
        P1 = point1;
        P2 = point2;
        P3 = point3;
    }
}

struct Edge
{
    public int P1 { get; }
    public int P2 { get; }

    public Edge(int point1, int point2)
    {
        P1 = point1;
        P2 = point2;
    }

    public bool Equals(Edge other) => (P1 == other.P2 && P2 == other.P1) || (P1 == other.P1 && P2 == other.P2);
}

public class Triangulator
{
    private bool IsPointInCircle(Vector2 point, Vector2 point1, Vector2 point2, Vector2 point3)
    {
        if (Mathf.Abs(point1.y - point2.y) < float.Epsilon && Mathf.Abs(point2.y - point3.y) < float.Epsilon)
        {
            return false;
        }

        float m1, m2, mx1, mx2, my1, my2, xc, yc;
        if (Mathf.Abs(point2.y - point1.y) < float.Epsilon)
        {
            m2 = -(point3.x - point2.x) / (point3.y - point2.y);
            mx2 = (point2.x + point3.x) / 2;
            my2 = (point2.y + point3.y) / 2;
            xc = (point2.x + point1.x) / 2;
            yc = m2 * (xc - mx2) + my2;
        }
        else if (Mathf.Abs(point3.y - point2.y) < float.Epsilon)
        {
            m1 = -(point2.x - point1.x) / (point2.y - point1.y);
            mx1 = (point1.x + point2.x) / 2;
            my1 = (point1.y + point2.y) / 2;
            xc = (point3.x + point2.x) / 2;
            yc = m1 * (xc - mx1) + my1;
        }
        else
        {
            m1 = -(point2.x - point1.x) / (point2.y - point1.y);
            m2 = -(point3.x - point2.x) / (point3.y - point2.y);
            mx1 = (point1.x + point2.x) / 2;
            mx2 = (point2.x + point3.x) / 2;
            my1 = (point1.y + point2.y) / 2;
            my2 = (point2.y + point3.y) / 2;
            xc = (m1 * mx1 - m2 * mx2 + my2 - my1) / (m1 - m2);
            yc = m1 * (xc - mx1) + my1;
        }

        float dx = point2.x - xc;
        float dy = point2.y - yc;
        float rsqr = dx * dx + dy * dy;
        dx = point.x - xc;
        dy = point.y - yc;
        double drsqr = dx * dx + dy * dy;
        return (drsqr <= rsqr);
    }

    public Mesh CreatePolygon(Vector2[] xyOfVertices)
    {
        Vector3[] vertices = new Vector3[xyOfVertices.Length];
        for (int i = 0; i < xyOfVertices.Length; i++)
        {
            vertices[i] = new Vector3(xyOfVertices[i].x, xyOfVertices[i].y, 0);
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = xyOfVertices;
        mesh.triangles = TriangulatePolygon(xyOfVertices);
        mesh.RecalculateNormals();

        return mesh;
    }

    private int[] TriangulatePolygon(Vector2[] xyOfVertices)
    {
        int vertexCount = xyOfVertices.Length;

        float xmin = xyOfVertices[0].x;
        float ymin = xyOfVertices[0].y;
        float xmax = xmin;
        float ymax = ymin;
        for (int i = 1; i < vertexCount; i++)
        {
            xmin = Mathf.Min(xmin, xyOfVertices[i].x);
            xmax = Mathf.Max(xmax, xyOfVertices[i].x);
            ymin = Mathf.Min(ymin, xyOfVertices[i].y);
            ymax = Mathf.Max(ymax, xyOfVertices[i].y);
        }

        float dx = xmax - xmin;
        float dy = ymax - ymin;
        float dmax = Mathf.Max(dx, dy);
        float xmid = (xmax + xmin) / 2;
        float ymid = (ymax + ymin) / 2;
        Vector2[] expandedXY = new Vector2[3 + vertexCount];
        for (int i = 0; i < vertexCount; i++)
        {
            expandedXY[i] = xyOfVertices[i];
        }

        expandedXY[vertexCount] = new Vector2(xmid - 2 * dmax, ymid - dmax);
        expandedXY[vertexCount + 1] = new Vector2(xmid, ymid + 2 * dmax);
        expandedXY[vertexCount + 2] = new Vector2(xmid + 2 * dmax, ymid - dmax);

        List<Triangle> triangleList = new List<Triangle>()
            {new Triangle(vertexCount, vertexCount + 1, vertexCount + 2)};

        for (int i = 0; i < vertexCount; i++)
        {
            List<Edge> edges = new List<Edge>();
            for (int j = 0; j < triangleList.Count; j++)
            {
                if (IsPointInCircle(expandedXY[i], expandedXY[triangleList[j].P1], expandedXY[triangleList[j].P2],
                    expandedXY[triangleList[j].P3]))
                {
                    edges.Add(new Edge(triangleList[j].P1, triangleList[j].P2));
                    edges.Add(new Edge(triangleList[j].P2, triangleList[j].P3));
                    edges.Add(new Edge(triangleList[j].P3, triangleList[j].P1));

                    triangleList.RemoveAt(j);
                    j--;
                }
            }

            for (int j = edges.Count - 2; j >= 0; j--)
            {
                for (int k = edges.Count - 1; k >= j + 1; k--)
                {
                    if (edges[j].Equals(edges[k]))
                    {
                        edges.RemoveAt(k);
                        edges.RemoveAt(j);
                        k--;
                    }
                }
            }

            for (int j = 0; j < edges.Count; j++)
            {
                triangleList.Add(new Triangle(edges[j].P1, edges[j].P2, i));
            }

            edges.Clear();
        }

        for (int i = triangleList.Count - 1; i >= 0; i--)
        {
            if (triangleList[i].P1 >= vertexCount || triangleList[i].P2 >= vertexCount ||
                triangleList[i].P3 >= vertexCount)
            {
                triangleList.RemoveAt(i);
            }
        }

        int[] triangles = new int[3 * triangleList.Count];
        for (int i = 0; i < triangleList.Count; i++)
        {
            triangles[3 * i] = triangleList[i].P1;
            triangles[3 * i + 1] = triangleList[i].P2;
            triangles[3 * i + 2] = triangleList[i].P3;
        }

        return triangles;
    }
}
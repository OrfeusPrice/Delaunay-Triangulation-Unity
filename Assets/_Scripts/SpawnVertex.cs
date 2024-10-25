using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVertex : MonoBehaviour
{
    private CreateMesh CM;
    private MeshController MC;

    void Start()
    {
        CM = this.GetComponent<CreateMesh>();
        MC = this.GetComponent<MeshController>();
    }

    public void CreateV()
    {
            GameObject temp = Instantiate(CM._pref);
            temp.transform.position = new Vector3(MC._mouse_pos.x, MC._mouse_pos.y, 0);
            temp.name = $"{temp.transform.position.x};{temp.transform.position.y}";
            CM._points.Add(temp);
    }

    public void DeleteV(GameObject v)
    {
        CM._points.Remove(v);
        Destroy(v);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform t;
    public Vector3 _mouse_pos;

    private void Start()
    {
        t = this.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            t.position = new Vector3(t.position.x, t.position.y, -t.position.z);
            t.rotation = Quaternion.Euler(0, t.position.z < 0 ? 0 : 180, 0);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            t.position = new Vector3(t.position.x, t.position.y,
                Mathf.Abs(t.position.z) == 10 ? t.position.z / 2 : t.position.z * 2);
        }

        if (Input.GetKey(KeyCode.Mouse2))
        {
            _mouse_pos = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)) / 100 / 4;
            Camera.main.transform.position = new Vector3(_mouse_pos.x, _mouse_pos.y, Camera.main.transform.position.z);
        }
    }
}
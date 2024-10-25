using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeshController : MonoBehaviour
{
    public Vector3 _mouse_pos;
    private RaycastHit _hit;
    private GameObject grabedObj;
    private bool isMoving;

    private void Start()
    {
        isMoving = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit))
            {
                if (_hit.collider.tag == "Point")
                {
                    grabedObj = _hit.collider.GameObject();
                    this.GetComponent<SpawnVertex>().DeleteV(grabedObj);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _mouse_pos = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)) / 100;
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit))
            {
                if (_hit.collider.tag == "Point")
                {
                    grabedObj = _hit.collider.GameObject();
                    isMoving = true;
                }
            }
            else this.GetComponent<SpawnVertex>().CreateV();
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (isMoving)
            {
                _mouse_pos = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)) / 100;
                grabedObj.transform.position = _mouse_pos;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isMoving = false;
        }
    }
}
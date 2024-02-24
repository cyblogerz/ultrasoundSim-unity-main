// handles user input - while debugging allow users to use mouse to use as a probe.

using System;
using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour
{
    private Vector3 mousePosition;

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMousePos();
    }

    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
    }
}


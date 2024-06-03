using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PointsSelector : MonoBehaviour
{
    [HideInInspector]
    public GameObject startPoint;
    
    [HideInInspector]
    public GameObject endPoint;

    void FindGameObject()
    {
        int layer = 5;
        
        var mousePosition3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        var origin = new Vector2(mousePosition3D.x, mousePosition3D.y);
        
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, 0f, layer);
        
        if (hit.collider != null)
        {
            GameObject clickedObject = hit.collider.gameObject;

            if (startPoint == null)
            {
                startPoint = clickedObject;

                startPoint.GetComponent<SpriteRenderer>().color = Color.blue;
            }

            else if (endPoint == null)
            {
                endPoint = clickedObject;

                endPoint.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FindGameObject();

        }
    }
}

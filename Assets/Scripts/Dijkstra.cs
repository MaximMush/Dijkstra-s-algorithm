using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{
    private int pointsCount;

    private int[,] graph;

    private float speed = 7f;

    private List<int>[] adjacencyList;

    private GameObject mousPointHandler;

    private Vector2 startPosition;

    private void Awake()
    {
        startPosition = transform.position;

        graph = ManagerJsonFile.DeserializeObjectForJsonmFile();
        
        mousPointHandler = GameObject.Find("PointMouseHandler");
        
        pointsCount = graph.GetLength(0);

        adjacencyList = new List<int>[pointsCount];

        for (int i = 0; i < pointsCount; i++)
        {
            adjacencyList[i] = new List<int>();
        }
    }

    public void Click()
    {
        transform.position = mousPointHandler.GetComponent<PointsSelector>().startPoint.transform.position;
        
        List<int> shortestPathIndices;
        
        int start = int.Parse(mousPointHandler.GetComponent<PointsSelector>().startPoint.name.Substring(5));

        int end = int.Parse(mousPointHandler.GetComponent<PointsSelector>().endPoint.name.Substring(5));

        DijkstraAlgorithm(graph, start, end, out shortestPathIndices);

        string path;
        
        List<int> newList = shortestPathIndices.Select(x => x + 1).ToList();
        
        path = string.Join("->", newList);
       
        GameObject.Find("Path").GetComponent<TextMeshProUGUI>().text = path;

        StartCoroutine(MoveToPoints(shortestPathIndices));
    }

    public void ResetClick()
    {
        transform.position = startPosition;
    
        if (mousPointHandler.GetComponent<PointsSelector>().startPoint != null)
        {
            mousPointHandler.GetComponent<PointsSelector>().startPoint.GetComponent<SpriteRenderer>().color = Color.yellow;

            mousPointHandler.GetComponent<PointsSelector>().startPoint = null;
        }
        else if (mousPointHandler.GetComponent<PointsSelector>().endPoint != null)
        {
            mousPointHandler.GetComponent<PointsSelector>().endPoint.GetComponent<SpriteRenderer>().color = Color.yellow;

            mousPointHandler.GetComponent<PointsSelector>().endPoint = null;
        }
    }

    public int DijkstraAlgorithm(int[,] graph, int source, int target, out List<int> shortestPathIndices)
    {
        int[] distance = new int[pointsCount];
        
        bool[] shortestPathSet = new bool[pointsCount];
        
        int[] path = new int[pointsCount];

        for (int i = 0; i < pointsCount; i++)
        {
            distance[i] = int.MaxValue;
            
            path[i] = -1;
            
            shortestPathSet[i] = false;
        }

        distance[source] = 0;

        for (int count = 0; count < pointsCount - 1; count++)
        {
            int u = MinDistance(distance, shortestPathSet);
            
            shortestPathSet[u] = true;

            if (u == target)
            {
                shortestPathIndices = GetPath(path, source, target);
                
                return distance[target];
            }

            for (int v = 0; v < pointsCount; v++)
            {
                
                if (!shortestPathSet[v] && Convert.ToBoolean(graph[u, v]) && distance[u] != int.MaxValue && distance[u] + graph[u, v] < distance[v])
                {
                    distance[v] = distance[u] + graph[u, v];
                    
                    path[v] = u;
                }
            }
        }

        shortestPathIndices = new List<int>();
        return distance[target];
    }
     
    private List<int> GetPath(int[] previos, int start, int target) 
    {
        List<int> path = new List<int>();

        int current = target;

        while (current != start)
        {
            path.Insert(0, current);

            current = previos[current];

        }
        path.Insert(0, start);
        
        return path;
    }
    private int MinDistance(int[] distance, bool[] shortestPathSet)
    {
        int min = int.MaxValue;
        
        int minIndex = -1;

        for (int v = 0; v < pointsCount; v++)
        {
            if (shortestPathSet[v] == false && distance[v] <= min)
            {
                min = distance[v];
                
                minIndex = v;
            }
        }

        return minIndex;
    }

    public IEnumerator MoveToPoints(List<int> points)
    {
        foreach (var point in points)
        {
            GameObject currentTarget = GameObject.Find("Point" + point);

            while (Vector2.Distance(transform.position, currentTarget.transform.position) > 0.1f)
            {
                gameObject.transform.position = Vector2.MoveTowards(transform.position, currentTarget.transform.position, speed * Time.deltaTime);
                
                yield return null;
            }
        }
        
        yield return null;
    }
}


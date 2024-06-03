using Newtonsoft.Json;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using System.Reflection;
using UnityEngine;

public class ManagerJsonFile
{
    public static int[,] DeserializeObjectForJsonmFile()
    {
        string jsonContent = Resources.Load<TextAsset>("Connections").text;

        var array = JsonConvert.DeserializeObject<Graph>(jsonContent);

        return array.graph;
    }
}

public class Graph
{   
    public int[,] graph { get; set; }
}
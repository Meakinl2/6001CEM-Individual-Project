using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class BezierControl : MonoBehaviour
{

    public string id { get; private set; }
    private HashSet<string> parentNodes = new HashSet<string>();

    private void Awake()
    {
        id = Guid.NewGuid().ToString();
        NodeManager.Instance.RegisterBezierControl(this);
    }

    public void setParentNodes(string parent1ID, string parent2ID) {
        parentNodes.Add(parent1ID);
        parentNodes.Add(parent2ID);
    }
}

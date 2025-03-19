using UnityEngine;
using System;
// using NodeManager;

public class Node : MonoBehaviour
{
    public string id {get; private set;}
    private void Awake() 
    {
        id = Guid.NewGuid().ToString();
        NodeManager.Instance.RegisterNode(this);
    } 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] Connections = {};
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

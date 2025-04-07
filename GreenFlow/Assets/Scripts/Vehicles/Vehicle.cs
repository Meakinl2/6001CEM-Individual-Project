using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;

public class Vehicle : MonoBehaviour
{
    public float speed;
    public int targetPositionIndex;
    public List<Vector3> currentLanePositions;
    public int laneIndex;
    public string bezierCurveId;

    private void Update() 
    {
        if (transform.position == currentLanePositions[^1]) VehicleManager.Instance.GetNextLane(this);

        else if (transform.position == currentLanePositions[targetPositionIndex])
        {
            targetPositionIndex += 1;
            // transform.LookAt(currentLanePositions[targetPositionIndex]);
        } 

        else transform.position = Vector3.MoveTowards(transform.position, currentLanePositions[targetPositionIndex], speed * Time.deltaTime);

    }

    

}

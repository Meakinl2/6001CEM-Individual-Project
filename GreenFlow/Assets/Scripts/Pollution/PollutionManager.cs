using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;

public class PollutionManager : MonoBehaviour
{
    public Dictionary<(int,int), float> pollutionGrid = new Dictionary<(int,int), float>();
    public Dictionary<(int,int), float> noiseGrid = new Dictionary<(int,int), float>();

    public float vehiclePollutionAmount = 1;
    public float noisePollutionAmount = 1;

    

    

    // Update is called once per frame
    void Update()
    {
        foreach (Vehicle vehicle in VehicleManager.Instance.spawnedVehicles)
        {
            int x = Mathf.FloorToInt(vehicle.transform.position.x / 10);
            int y = Mathf.FloorToInt(vehicle.transform.position.y / 10);

            if (!pollutionGrid.ContainsKey((x,y))) pollutionGrid.Add((x,y), 0);
            if (!noiseGrid.ContainsKey((x,y))) noiseGrid.Add((x,y), 0);

            pollutionGrid[(x,y)] += vehiclePollutionAmount;
            noiseGrid[(x,y)] += noisePollutionAmount;

        }

    }
}

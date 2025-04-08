using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Burst;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Collections;

public class VehicleManager : MonoBehaviour
{

    public static VehicleManager Instance { get; private set;}

    public GameObject vehiclePrefab;
    public int numVehicles = 10;
    public float customSpeed = 10f;

    public bool isActive = false;
    public List<Vehicle> spawnedVehicles = new List<Vehicle>();
    
    private float timer = 0f;
    public float spawnDelay = 1f;

    private string filePath;
    private float startTime;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        string dateTime = System.DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
        filePath = "Assets/Logs/Vehicle_Logs/" + dateTime + "-vehicle_log.txt";
    }

    private IEnumerator Start() 
    {
        startTime = Time.time;
        

        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            
            try
            {
                float currentTime = Time.time;     
                float milliseconds = (currentTime - startTime) * 1000;
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(milliseconds + ", " + spawnedVehicles.Count());
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error writing to file: " + e.Message);
            }

        }
    }

    private void Update()
    {
        if (!isActive) return;

        if (spawnedVehicles.Count >= numVehicles) return;

        timer += Time.deltaTime;

        if (timer < spawnDelay) {return;}

        BezierControl randomBezierControl = NodeManager.Instance.GetRandomBezierControl();
        BlobAssetReference<BlobArray<float3>> lanePoints = LaneManager.Instance.GetLaneBlob(randomBezierControl.id, 1);
        float3 startPoint = lanePoints.Value[0];


        GameObject newSpawn = Instantiate(vehiclePrefab, new Vector3(startPoint.x,startPoint.y,startPoint.z), Quaternion.identity);
        Vehicle newVehicle = newSpawn.GetComponent<Vehicle>();

        newVehicle.bezierCurveId = randomBezierControl.id;
        newVehicle.laneIndex = 1;

        newVehicle.speed = customSpeed;
        newVehicle.targetPositionIndex = 1;
        
        for (int i = 0; i < lanePoints.Value.Length; i++)
        {
            float3 pos = lanePoints.Value[i];
            newVehicle.currentLanePositions.Add(new Vector3(pos.x, pos.y, pos.z));
        }
        

        spawnedVehicles.Add(newVehicle);

        timer = 0f;

    }

    public void StartSpawning()
    {
        Debug.Log("Turning Vehicle Spawner On.");
        isActive = true;
    }

    public void StopSpawning()
    {
        Debug.Log("Turning Vehicle Spawner Off.");
        isActive = false;

        foreach (Vehicle vehicle in spawnedVehicles) Destroy(vehicle.gameObject);
        spawnedVehicles.Clear();
    }

    public void GetNextLane(Vehicle vehicle)
    {
        List<LaneKey> connectedLanes = LaneManager.Instance.GetLaneConnections(vehicle.bezierCurveId, vehicle.laneIndex);
        LaneKey nextLane = connectedLanes[UnityEngine.Random.Range(0, connectedLanes.Count)];


        vehicle.laneIndex = nextLane.laneIndex;
        vehicle.bezierCurveId = nextLane.bezierParentId;

        BlobAssetReference<BlobArray<float3>> lanePoints = LaneManager.Instance.GetLaneBlob(nextLane.bezierParentId, nextLane.laneIndex);
        
        vehicle.currentLanePositions.Clear();
        for (int i = 0; i < lanePoints.Value.Length; i++)
        {
            float3 pos = lanePoints.Value[i];
            vehicle.currentLanePositions.Add(new Vector3(pos.x, pos.y, pos.z));
        }

        vehicle.targetPositionIndex = 0;

    }
}

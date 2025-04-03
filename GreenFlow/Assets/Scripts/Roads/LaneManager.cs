using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Burst;
using System.Collections.Generic;

public class LaneManager : MonoBehaviour
{
    public static LaneManager Instance { get; private set; }

    // Structure for lanes that is built from the parent beziercontrol and the lane's index.
    public struct LaneKey
    {
        public string bezierParentId;
        public int laneIndex; 

        public LaneKey(string bezierParentId1, int laneIndex1)
        {
            bezierParentId = bezierParentId1;
            laneIndex = laneIndex1;
        }
    }

    // Dictionary containing all lanes specified by bezier curve parent node and lane position
    private Dictionary<LaneKey, BlobAssetReference<BlobArray<float3>>> laneBlobsRegistry;
    private Dictionary<LaneKey, List<LaneKey>> laneConnections;
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        laneBlobsRegistry = new Dictionary<LaneKey, BlobAssetReference<BlobArray<float3>>>();
        laneConnections = new Dictionary<LaneKey, List<LaneKey>>();
    }

    public void TryRegisterLane(string bezierParentId, int laneIndex)
    {
        Debug.Log("Trying to Register Lane: " + laneIndex + "|" + bezierParentId);
        LaneKey laneKey = new LaneKey(bezierParentId, laneIndex);
        if (laneBlobsRegistry.ContainsKey(laneKey)) 
        {
            Debug.Log("Lane: " + laneIndex + "|" + bezierParentId + "already registered.");
            return;
        }
        laneBlobsRegistry.Add(laneKey, default);
        Debug.Log("Succefully Registered Lane: " + laneIndex + "|" + bezierParentId);
    }


    public BlobAssetReference<BlobArray<float3>> GetLaneBlob(LaneKey laneKey)
    {
        return laneBlobsRegistry.TryGetValue(laneKey, out BlobAssetReference<BlobArray<float3>> laneBlob) ? laneBlob : default;
    }


    public BlobAssetReference<BlobArray<float3>> GetLaneBlob(string bezierParentId, int laneIndex)
    {
        LaneKey laneKey = new LaneKey(bezierParentId, laneIndex);
        return laneBlobsRegistry.TryGetValue(laneKey, out BlobAssetReference<BlobArray<float3>> laneBlob) ? laneBlob : default;
    }


    public void UpdateLaneBlob(string bezierParentId, int laneIndex, List<Vector3> lanePointsList)
    {
        Debug.Log("Trying to Update Blob for Lane: " + laneIndex + "|" + bezierParentId);   
        LaneKey laneKey = new LaneKey(bezierParentId, laneIndex);
        if (laneBlobsRegistry.ContainsKey(laneKey)) return;
        if (GetLaneBlob(laneKey) != null) laneBlobsRegistry[laneKey].Dispose();

        // Blob are used for entities they are more memory and resource efficient for the entity modules large paralleism
        using (BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp))
        {
            ref BlobArray<float3> lanePointsBlob = ref blobBuilder.ConstructRoot<BlobArray<float3>>();
            BlobBuilderArray<float3> points = blobBuilder.Allocate(ref lanePointsBlob, lanePointsList.Count);

            for (int i = 0; i < lanePointsList.Count; i++)
            {
                points[i] = new float3(lanePointsList[i].x, lanePointsList[i].y, lanePointsList[i].z);
            }

            laneBlobsRegistry[laneKey] = blobBuilder.CreateBlobAssetReference<BlobArray<float3>>(Allocator.Persistent);
        }
        Debug.Log("Successully Updated Blob for Lane: " + laneIndex + "|" + bezierParentId);
    }

    public void DeregisterLane(string bezierParentId, int laneIndex)
    {
        
        LaneKey laneKey = new LaneKey(bezierParentId, laneIndex);
        if (!laneBlobsRegistry.ContainsKey(laneKey)) return;
        
        foreach (LaneKey laneKey2 in laneConnections[laneKey]) {
            RemoveLaneConnection(laneKey, laneKey2);
        }
        
        laneBlobsRegistry[laneKey].Dispose();
        laneBlobsRegistry.Remove(laneKey);
    }


    public void AddLaneConnection(string bezierParentId, int laneIndex)
    {
        LaneKey laneKey = new LaneKey(bezierParentId, laneIndex);

    }

    public void RemoveLaneConnection(LaneKey laneKey1, LaneKey laneKey2)
    {
        if (laneConnections[laneKey1].Contains(laneKey2)) {laneConnections[laneKey1].Remove(laneKey2);}
        if (laneConnections[laneKey2].Contains(laneKey1)) {laneConnections[laneKey2].Remove(laneKey1);}
    }


    public void RemoveLaneConnection(string bezierParentId1, int laneIndex1, string bezierParentId2, int laneIndex2)
    {
        LaneKey laneKey1 = new LaneKey(bezierParentId1, laneIndex1);
        LaneKey laneKey2 = new LaneKey(bezierParentId2, laneIndex2);

        if (laneConnections[laneKey1].Contains(laneKey2)) {laneConnections[laneKey1].Remove(laneKey2);}
        if (laneConnections[laneKey2].Contains(laneKey1)) {laneConnections[laneKey2].Remove(laneKey1);}
    }

}

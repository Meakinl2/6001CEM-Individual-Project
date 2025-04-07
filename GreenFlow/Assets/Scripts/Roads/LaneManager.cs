using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Burst;
using System.Collections.Generic;

public class LaneManager : MonoBehaviour
{
    public static LaneManager Instance { get; private set; }

    


    // Dictionary containing all lanes specified by bezier curve parent node and lane position
    private Dictionary<LaneKey, BlobAssetReference<BlobArray<float3>>> laneBlobsRegistry;
    // Dictionary of all connection by laneKey, using a bool to determine if it's the start of the lane or the end of the lane.
    public Dictionary<LaneKey, List<LaneKey>> laneConnections;
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        laneBlobsRegistry = new Dictionary<LaneKey, BlobAssetReference<BlobArray<float3>>>();
        laneConnections = new Dictionary<LaneKey, List<LaneKey>>();
    }


    private void OnDestroy()
    {
        foreach (BlobAssetReference<BlobArray<float3>> blob in laneBlobsRegistry.Values) blob.Dispose();
    }

    public void TryRegisterLane(string bezierParentId, int laneIndex)
    {
        Debug.Log("Trying to Register Lane: " + laneIndex + "|" + bezierParentId);
        LaneKey laneKey = new LaneKey(bezierParentId, laneIndex);
        if (laneBlobsRegistry.ContainsKey(laneKey)) return;
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
        if (!laneBlobsRegistry.ContainsKey(laneKey)) return;
        if (GetLaneBlob(laneKey).IsCreated) laneBlobsRegistry[laneKey].Dispose();

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
        
        LaneKey laneKey1 = new LaneKey(bezierParentId, laneIndex);
        if (!laneBlobsRegistry.ContainsKey(laneKey1)) return;

        foreach (LaneKey laneKey2 in laneConnections[laneKey1]) {RemoveLaneConnection(laneKey1, laneKey2);}
        
        laneBlobsRegistry[laneKey1].Dispose();
        laneBlobsRegistry.Remove(laneKey1);
    }


    public void AddLaneConnection(string bezierParentId1, int laneIndex1, 
                                    string bezierParentId2, int laneIndex2)
    {
        Debug.Log("Attemping to add Connection between Lane: " + laneIndex1 + "|" + bezierParentId1
                    + " and Lane: "+ laneIndex2 + "|" + bezierParentId2);

        LaneKey laneKey1 = new LaneKey(bezierParentId1, laneIndex1);
        LaneKey laneKey2 = new LaneKey(bezierParentId2, laneIndex2);

        if (!laneConnections.ContainsKey(laneKey1)) laneConnections.Add(laneKey1, new List<LaneKey>());
        if (!laneConnections.ContainsKey(laneKey2)) laneConnections.Add(laneKey2, new List<LaneKey>());

        if (!laneConnections[laneKey1].Contains(laneKey2)) laneConnections[laneKey1].Add(laneKey2);
        if (!laneConnections[laneKey2].Contains(laneKey1)) laneConnections[laneKey2].Add(laneKey1);

        Debug.Log("Successfully added Connection between Lane: " + laneIndex1 + "|" + bezierParentId1
                    + " and Lane: " + laneIndex2 + "|" + bezierParentId2);

    }

    public void RemoveLaneConnection(LaneKey laneKey1, LaneKey laneKey2)
    {
        Debug.Log("Attemping to remove Connection between Lane: " + laneKey1.laneIndex + "|" + laneKey1.bezierParentId
                    + " and Lane: " + laneKey2.laneIndex + "|" + laneKey2.bezierParentId);

        if (laneConnections[laneKey1].Contains(laneKey2)) {laneConnections[laneKey1].Remove(laneKey2);}
        if (laneConnections[laneKey2].Contains(laneKey1)) {laneConnections[laneKey2].Remove(laneKey1);}

        Debug.Log("Successfully removed Connection between Lane: " + laneKey1.laneIndex + "|" + laneKey1.bezierParentId
                    + " and Lane: " + laneKey2.laneIndex + "|" + laneKey2.bezierParentId);
    }


    public void RemoveLaneConnection(string bezierParentId1, int laneIndex1,
                                    string bezierParentId2, int laneIndex2)
    {
        Debug.Log("Attemping to remove Connection between Lane: " + laneIndex1 + "|" + bezierParentId1
                    + " and Lane: "+ laneIndex2 + "|" + bezierParentId2);

        LaneKey laneKey1 = new LaneKey(bezierParentId1, laneIndex1);
        LaneKey laneKey2 = new LaneKey(bezierParentId2, laneIndex2);

        if (laneConnections[laneKey1].Contains(laneKey2)) {laneConnections[laneKey1].Remove(laneKey2);}
        if (laneConnections[laneKey2].Contains(laneKey1)) {laneConnections[laneKey2].Remove(laneKey1);}

        Debug.Log("Successfully removed Connection between Lane: " + laneIndex1 + "|" + bezierParentId1
                    + " and Lane: "+ laneIndex2 + "|" + bezierParentId2);
    }


    public List<LaneKey> GetLaneConnections(string bezierParentId, int laneIndex) 
    {
        LaneKey laneKey = new LaneKey(bezierParentId, laneIndex);
        if (!laneConnections.ContainsKey(laneKey)) return null;
        return laneConnections[laneKey];
    }

}


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

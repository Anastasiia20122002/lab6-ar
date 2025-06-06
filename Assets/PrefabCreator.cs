using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PrefabCreator : MonoBehaviour
{
    [SerializeField] private GameObject dragonPrefab;
    [SerializeField] private GameObject castlePrefab;
    [SerializeField] private GameObject thirdPrefab;

    [SerializeField] private Vector3 dragonPrefabOffset;
    [SerializeField] private Vector3 castlePrefabOffset;
    [SerializeField] private Vector3 thirdPrefabOffset;

    private ARTrackedImageManager arTrackedImageManager;
    private Dictionary<TrackableId, List<GameObject>> spawnedObjects = new();

    private void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        if (arTrackedImageManager != null)
        {
            arTrackedImageManager.trackablesChanged.AddListener(OnTrackablesChanged);
        }
    }

    private void OnDisable()
    {
        if (arTrackedImageManager != null)
        {
            arTrackedImageManager.trackablesChanged.RemoveListener(OnTrackablesChanged);
        }
    }

    private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (ARTrackedImage image in eventArgs.added)
        {
            CreatePrefabs(image);
        }

        foreach (ARTrackedImage image in eventArgs.updated)
        {
            UpdatePrefabsPosition(image);
        }
    }

    private void CreatePrefabs(ARTrackedImage image)
    {
        if (!spawnedObjects.ContainsKey(image.trackableId))
        {
            spawnedObjects[image.trackableId] = new List<GameObject>();
        }

        // Spawn Dragon
        GameObject dragon = Instantiate(dragonPrefab, image.transform.position + dragonPrefabOffset, image.transform.rotation);
        dragon.transform.SetParent(image.transform);
        spawnedObjects[image.trackableId].Add(dragon);

        // Spawn Castle
        GameObject castle = Instantiate(castlePrefab, image.transform.position + castlePrefabOffset, image.transform.rotation);
        castle.transform.SetParent(image.transform);
        spawnedObjects[image.trackableId].Add(castle);

        // Spawn Third Object
        GameObject third = Instantiate(thirdPrefab, image.transform.position + thirdPrefabOffset, image.transform.rotation);
        third.transform.SetParent(image.transform);
        spawnedObjects[image.trackableId].Add(third);
    }

    private void UpdatePrefabsPosition(ARTrackedImage image)
    {
        if (spawnedObjects.TryGetValue(image.trackableId, out List<GameObject> existingObjects))
        {
            // Update Dragon
            if (existingObjects.Count > 0)
            {
                existingObjects[0].transform.position = image.transform.position + dragonPrefabOffset;
                existingObjects[0].transform.rotation = image.transform.rotation;
            }

            // Update Castle
            if (existingObjects.Count > 1)
            {
                existingObjects[1].transform.position = image.transform.position + castlePrefabOffset;
                existingObjects[1].transform.rotation = image.transform.rotation;
            }

            // Update Third Object
            if (existingObjects.Count > 2)
            {
                existingObjects[2].transform.position = image.transform.position + thirdPrefabOffset;
                existingObjects[2].transform.rotation = image.transform.rotation;
            }
        }
    }
}

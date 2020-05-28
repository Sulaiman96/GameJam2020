using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentGameobjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject persistentGameobjectPrefab = default;

    static bool hasSpawned = false;

    private void Awake()
    {
        if (hasSpawned)
            return;

        SpawnPersistentObject();
        hasSpawned = true;
    }

    private void SpawnPersistentObject()
    {
        GameObject persistentObject = Instantiate(persistentGameobjectPrefab);
        DontDestroyOnLoad(persistentObject);
    }

}

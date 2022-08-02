using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoleculeSpawner : MonoBehaviour
{
    public GameObject moleculePrefab;

    GameObject spawnedMolecule;
    bool spawning;
    float spawnAtTime;

    void Update()
    {
        if (spawning)
        {
            if (Time.time > spawnAtTime)
            {
                spawnedMolecule = Instantiate(moleculePrefab, transform);
                spawning = false;
            }
        }
        else if (spawnedMolecule == null)
        {
            spawning = true;
            spawnAtTime = Time.time + 3f;
        }
    }
}

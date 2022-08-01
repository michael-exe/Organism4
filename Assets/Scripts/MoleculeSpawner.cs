using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoleculeSpawner : MonoBehaviour
{
    public GameObject moleculePrefab;
    GameObject spawnedMolecule;

    void Update()
    {
        SpawnIfNull();    
    }

    void SpawnIfNull()
    {
        if (spawnedMolecule == null)
        {
            spawnedMolecule = Instantiate(moleculePrefab, transform);
        }

    }
}

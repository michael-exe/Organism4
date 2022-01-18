using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeInstantiator : MonoBehaviour
{
    public GameObject molecule;
    //public Transform moleculeLocation;
    
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(molecule, new Vector2(-1, 6), Quaternion.identity);
        Instantiate(molecule, new Vector2(-11, 2), Quaternion.identity);
        Instantiate(molecule, new Vector2(9, -6), Quaternion.identity);
        Instantiate(molecule, new Vector2(14, 3), Quaternion.identity);
        Instantiate(molecule, new Vector2(-14, -7), Quaternion.identity);
        Instantiate(molecule, new Vector2(-17, 6), Quaternion.identity);
        Instantiate(molecule, new Vector2(17, -9), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

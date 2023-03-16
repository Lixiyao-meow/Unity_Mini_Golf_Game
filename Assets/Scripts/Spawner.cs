using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject itemPrefab;

    public void Spawn(GameObject item)
    {
        Instantiate(item, new Vector3(0, 0, 0), Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    public GameObject item;
    public Spawner spawner;

    private void OnEnable()
    {
        spawner.Spawn(item);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapManager : MonoBehaviour
{
    public List<SnappingScript> snapComponents = new List<SnappingScript>();

    private void Awake() 
    {
        SnappingScript[] allSnappingScripts = FindObjectsOfType<SnappingScript>();
        snapComponents = new List<SnappingScript>(allSnappingScripts);
    }

    private void Update() 
    {
        foreach(SnappingScript snapComponent in snapComponents)
        {
            foreach(SnappingScript otherSnapComponent in snapComponents)
            {
                if (snapComponent != otherSnapComponent)
                {
                    snapComponent.SnapToTarget(otherSnapComponent);
                }
            }
        }
    }
}

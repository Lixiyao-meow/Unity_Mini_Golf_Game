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

    private void LateUpdate() 
    {
        SnappingScript[] allSnappingScripts = FindObjectsOfType<SnappingScript>();

        bool changed = false;

        if (allSnappingScripts.Length != snapComponents.Count)
        {
            changed = true;
        }

        snapComponents = new List<SnappingScript>(allSnappingScripts);

        if (changed)
        {
            foreach(SnappingScript snapComponent in snapComponents)
            {
                snapComponent.notSnappable = true;
            }
            
            snapComponents[0].notSnappable = false;
        }

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

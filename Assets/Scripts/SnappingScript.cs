using System;
using UnityEngine;

public class SnappingScript : MonoBehaviour
{
    public Collider myCollider;
    public float snapDistance = 0.2f;

    private void Start() {
        myCollider = GetComponent<Collider>();
    }

    public Collider getMyCollider()
    {
        return myCollider;
    }

    public void SnapToTarget(SnappingScript otherSnapComponent)
    {
        Vector3 myClosestPoint = myCollider.ClosestPoint(otherSnapComponent.transform.position);
        Vector3 targetClosestPoint = otherSnapComponent.getMyCollider().ClosestPoint(myClosestPoint);
        Vector3 offset = targetClosestPoint - myClosestPoint;

        if (offset.magnitude < snapDistance)
        {
            transform.position += offset;
            Vector3 newPos = transform.position;
            if (Math.Abs(otherSnapComponent.transform.position.x - transform.position.x) < Math.Abs(otherSnapComponent.transform.position.z - transform.position.z))
                newPos.x = otherSnapComponent.transform.position.x;
            else
                newPos.z = otherSnapComponent.transform.position.z;
            newPos.y = 0.0f;
            transform.position = newPos;
        }
    }
}


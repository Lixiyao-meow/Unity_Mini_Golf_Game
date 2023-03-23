using System;
using UnityEngine;

public class SnappingScript : MonoBehaviour
{
    public Collider myCollider;
    public float snapDistance = 1.25f;
    public bool notSnappable = false;

    private void Start() {
        myCollider = GetComponent<Collider>();
        snapDistance = 1.25f;
    }

    public Collider getMyCollider()
    {
        return myCollider;
    }

    public void SnapToTarget(SnappingScript otherSnapComponent)
    {
        if (notSnappable) return;

        Vector3 myClosestPoint = myCollider.ClosestPoint(otherSnapComponent.transform.position);
        Vector3 targetClosestPoint = otherSnapComponent.getMyCollider().ClosestPoint(myClosestPoint);
        float offset = Vector3.Distance(otherSnapComponent.transform.position, transform.position);

        if (offset < snapDistance)
        {
            Vector3 newPos = transform.position;
            newPos.y = 0;
            Quaternion newRot = transform.rotation;
            newRot.x = 0;
            newRot.z = 0;

            float valy = 0f;

            if (newRot.eulerAngles.y > 45 && newRot.eulerAngles.y <= 135)
            {
                valy = 90;
            }
            else if (newRot.eulerAngles.y > 135 && newRot.eulerAngles.y <= 225)
            {
                valy = 180;
            }
            else if (newRot.eulerAngles.y > 225 && newRot.eulerAngles.y <= 315)
            {
                valy = 270;
            }
            else
            {
                valy = 0;
            }

            transform.rotation = Quaternion.Euler(0f, valy, 0f); ;

            if (Math.Abs(otherSnapComponent.transform.position.x - transform.position.x) < Math.Abs(otherSnapComponent.transform.position.z - transform.position.z))
                newPos.x = otherSnapComponent.transform.position.x;
            else
                newPos.z = otherSnapComponent.transform.position.z;
            newPos.y = 0.0f;
            transform.position = newPos;
        }
    }
}


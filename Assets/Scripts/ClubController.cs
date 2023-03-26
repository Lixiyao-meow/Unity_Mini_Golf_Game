using Ubiq.XR;
using UnityEngine;

public class ClubController : GraspBehaviour
{
    public Transform visual;
    public BallController myBall;
    
    void Start()
    {
    }

    internal override void Awake()
    {
        base.Awake();
    }

    public override void Grasp(Hand controller)
    {
        base.Grasp(controller);
        // Vector (A -> B) => (B - A)
        Vector3 relativePosition = transform.position - controller.transform.position;
        visual.position += relativePosition;
    }

    public override void Release(Hand controller)
     {
        base.Release(controller);
     }


    internal override void Update()
    {
        base.UpdateOwnership(myBall);
        base.Update();
    }

}

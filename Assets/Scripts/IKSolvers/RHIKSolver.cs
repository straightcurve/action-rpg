using UnityEngine;

public class RHIKSolver {
    private Vector3 rhRayDirection;
    private RaycastHit rhHit;
    private bool rhIK;
    private Transform owner;
    private Transform head;
    private float range;
    private Animator animator;

    public RHIKSolver(Transform owner, Transform head, float range) {
        this.owner = owner;
        this.head = head;
        this.range = range;
        this.animator = owner.GetComponent<Animator>();
    }

    public void Update() {
        rhRayDirection = owner.forward + Vector3.down * 0.5f;
        rhIK = Physics.Raycast(head.position, rhRayDirection, out rhHit, range);
        if (rhIK) {
            Debug.DrawRay(head.position, rhRayDirection, Color.green);
        } else {
            Debug.DrawRay(head.position, rhRayDirection, Color.red);
        }
    }

    public void UpdateIK() {
        if (!rhIK)
            return;

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rhHit.point);

        var no_idea = rhRayDirection;
        no_idea.y = Vector3.up.y;

        //  ^ from direction of hand but always pointed up

        var projection = Vector3.ProjectOnPlane(no_idea, rhHit.normal);

        Debug.DrawRay(rhHit.point, projection, Color.cyan);

        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        animator.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.LookRotation(projection));
    }
}
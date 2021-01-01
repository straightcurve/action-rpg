using UnityEngine;

public class LHIKSolver {
    private Vector3 lhRayDirection;
    private RaycastHit lhHit;
    private bool lhIK;
    private Transform owner;
    private Transform head;
    private float range;
    private Animator animator;

    public LHIKSolver(Transform owner, Transform head, float range) {
        this.owner = owner;
        this.head = head;
        this.range = range;
        this.animator = owner.GetComponent<Animator>();
    }

    public void Update() {
        lhRayDirection = owner.forward + Vector3.down * 0.5f;
        lhIK = Physics.Raycast(head.position, lhRayDirection, out lhHit, range);
        if (lhIK) {
            Debug.DrawRay(head.position, lhRayDirection * range, Color.green);
        } else {
            Debug.DrawRay(head.position, lhRayDirection * range, Color.red);
        }
    }

    public void UpdateIK() {
        if (!lhIK)
            return;

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, lhHit.point);

        var no_idea = lhRayDirection;
        no_idea.y = Vector3.up.y;

        //  ^ from direction of hand but always pointed up

        var projection = Vector3.ProjectOnPlane(no_idea, lhHit.normal);

        Debug.DrawRay(lhHit.point, projection, Color.cyan);

        var rotation = Quaternion.LookRotation(projection);
        var euler = rotation.eulerAngles;
        var angle = Vector3.Angle(owner.transform.forward, Vector3.forward);
        euler.y = angle;
        rotation.eulerAngles = euler;

        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, rotation);
    }
}
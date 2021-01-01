using UnityEngine;

public class LHIKSolver
{
    private Vector3 rayDirection;
    private RaycastHit hit;
    private bool active;
    private Transform owner;
    private Transform head;
    private float range;
    private Animator animator;
    public float colliderEdgeRange;
    public float verticalIKPositionOffset;
    private CharacterCollision collision;

    public LHIKSolver(Transform owner, Transform head, float range)
    {
        this.owner = owner;
        this.head = head;
        this.range = range;
        this.animator = owner.GetComponent<Animator>();
        this.collision = owner.GetComponent<CharacterCollision>();
    }

    public void Update()
    {
        rayDirection = owner.forward;
        active = Physics.Raycast(head.position, rayDirection, out hit, range);
        if (active)
        {
            Debug.DrawRay(head.position, rayDirection * range, Color.green);
        }
        else
        {
            Debug.DrawRay(head.position, rayDirection * range, Color.red);
        }
    }

    public void UpdateIK()
    {
        if (!active)
            return;

        //  set weight based on distance
        SetIKPositionWeightBasedOnDistance(hit.distance);

        var target = hit.point;
        target.y += verticalIKPositionOffset;
        target = hit.collider.ClosestPoint(target);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, target);

        var no_idea = rayDirection;
        no_idea.y = Vector3.up.y;

        var projection = Vector3.ProjectOnPlane(no_idea, hit.normal);

        Debug.DrawRay(hit.point, projection, Color.cyan);

        var rotation = Quaternion.LookRotation(projection);
        var euler = rotation.eulerAngles;
        var angle = Vector3.Angle(owner.transform.forward, Vector3.forward);
        euler.y = angle;
        rotation.eulerAngles = euler;

        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, rotation);
    }

    private void SetIKPositionWeightBasedOnDistance(float distance) {
        // ray range...collider edge range -> 0........1
        var weight = Helpers.LinearMap(distance, range, colliderEdgeRange, 0, 1);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
    }
}
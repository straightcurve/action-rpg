
using UnityEngine;

public class WallIKSolver: MonoBehaviour {

    public bool useIK = true;
    public bool lhIK;
    public bool rhIK;

    public Transform head;
    public float range = .5f;

    private CharacterCollision collision;
    private Animator animator;

    private Vector3 lhRayDirection;
    private RaycastHit lhHit;

    private void Awake() {
        collision = GetComponent<CharacterCollision>();
        if (collision == null)
            throw new System.NullReferenceException("collision");

        animator = GetComponent<Animator>();
        if (animator == null)
            throw new System.NullReferenceException("animator");
    }

    private void FixedUpdate() {
        // lhRayDirection = transform.forward + Vector3.down * 0.5f;
        // lhIK = Physics.Raycast(head.position, lhRayDirection, out lhHit, range);
        // if (lhIK) {
        //     Debug.DrawRay(head.position, lhRayDirection, Color.green);
        // } else {
        //     Debug.DrawRay(head.position, lhRayDirection, Color.red);
        // }
    }

    private void Update() {
        lhRayDirection = transform.forward + Vector3.down * 0.5f;
        lhIK = Physics.Raycast(head.position, lhRayDirection, out lhHit, range);
        if (lhIK) {
            Debug.DrawRay(head.position, lhRayDirection, Color.green);
        } else {
            Debug.DrawRay(head.position, lhRayDirection, Color.red);
        }
    }

    public float angle = -90f;

    private void OnAnimatorIK() {
        if (!useIK)
            return;

        if (lhIK) {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, lhHit.point);

            var no_idea = lhRayDirection;
            no_idea.y = Vector3.up.y;

            //  ^ from direction of hand but always pointed up

            var projection = Vector3.ProjectOnPlane(no_idea, lhHit.normal);

            Debug.DrawRay(lhHit.point, projection, Color.cyan);

            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.LookRotation(projection));
        }
    }

}
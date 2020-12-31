
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
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            var projection = lhHit.collider.ClosestPointOnBounds(Vector3.ProjectOnPlane(lhRayDirection, lhHit.normal));
            Debug.DrawRay(lhHit.point, projection, Color.yellow);

            angle = Vector3.Angle(lhRayDirection, lhHit.normal);

            var reflection = projection.normalized;
            // if (angle > 175f) {
            //     reflection.x = -reflection.x;
            //     reflection.y = -reflection.y;
            //     reflection.z = -reflection.z;
            // }
            reflection *= projection.magnitude;

            //  TODO: calculate angle between ray dir and normal
            //  and do stuff based on that

            //  HAND POINTS IN -X DIRECTION THANKS

            Debug.DrawRay(lhHit.point, reflection, Color.cyan);


            animator.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.LookRotation(reflection));
            // animator.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.AngleAxis(angle, Vector3.Cross(lhRayDirection, lhHit.normal)));
        }
    }

}
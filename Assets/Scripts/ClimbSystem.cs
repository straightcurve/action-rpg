using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class ClimbSystem : MonoBehaviour
{
    public Animator animator;
    public AnimationClip clip;

    [Range(0.1f, 20f)] public float range;
    public LayerMask mask;
    public float displacementZ = 0.003f;
    private float displacementY;
    public Transform leftHandIK;
    public Transform rightHandIK;
    public bool ikActive;
    [Range(0f, 1f)] public float timeScale = 1f;
    public CharacterCollision collision;
    public float climbTime = 1f;
    public Transform pointWeAreAfterWeFinishClimbing;

    Character character;
    new Rigidbody rigidbody;
    private bool climbing = false;
    private float duration = 0f;
    public RaycastHit hit;
    private float shoulderWidth;
    public float ledgeY;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Awake()
    {
        if (collision == null)
            throw new UnityException("collision null");

        character = GetComponent<Character>();

        if (character == null)
            throw new UnityException("character null");

        if (animator == null)
            throw new UnityException("animator null");

        rigidbody = GetComponent<Rigidbody>();
        if (rigidbody == null)
            throw new UnityException("rigidbody null");

    }

    public float durationUntilIdle = 1f;
    private bool deactivating;

    private IEnumerator ReturnToIdle() {
        var end = animator.bodyPosition;
        // yield return null;
        var duration = durationUntilIdle;
        while (duration > 0) {
            duration -= Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, end, Time.deltaTime);
            yield return null;
        }

        collision.EnableFeet();
        collision.DisableLeftFoot();
        collision.DisableRightFoot();
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        character.canMove = true;
        deactivating = false;
    }

    private void Update() {
        if (deactivating)
            return;

        if (climbing) {
            duration -= Time.deltaTime;

            var this_frame_move_this_amount_on_z = Mathf.Lerp(0f, displacementZ, Time.deltaTime);
            var this_frame_move_this_amount_on_y = Mathf.Lerp(0f, displacementY, Time.deltaTime);
            // if (clip.length - duration > climbTime)
            //     transform.position += new Vector3(0f, 0f, this_frame_move_this_amount_on_z);
            // leftHandIK.position += new Vector3(0f, -this_frame_move_this_amount_on_y, this_frame_move_this_amount_on_z);
            // rightHandIK.position += new Vector3(0f, -this_frame_move_this_amount_on_y, this_frame_move_this_amount_on_z);

            if (duration <= 0f) {
                deactivating = true;
                climbing = false;

                animator.SetBool("Climb_Medium", climbing);


                var closest = hit.collider.bounds.ClosestPoint(new Vector3(transform.position.x, ledgeY, transform.position.z));

                pointWeAreAfterWeFinishClimbing.position = closest;
                pointWeAreAfterWeFinishPushing = closest;

                StartCoroutine(ReturnToIdle());
            }

            return;
        }

        if (!Physics.Raycast(new Vector3(transform.position.x, transform.position.y + .01f, transform.position.z), transform.TransformDirection(Vector3.forward), out hit, range, mask, QueryTriggerInteraction.Ignore)) {
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + .01f, transform.position.z), transform.TransformDirection(Vector3.forward) * range, Color.red);
            return;
        } else {
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + .01f, transform.position.z), transform.TransformDirection(Vector3.forward) * range, Color.green);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            var point = hit.point;
            var ledge = ledgeY = hit.collider.bounds.center.y + hit.collider.bounds.extents.y;
            var closest = hit.collider.bounds.ClosestPoint(new Vector3(transform.position.x, ledge, transform.position.z));

            // leftHandIK.position = new Vector3(closest.x - shoulderWidth, ledge, closest.z);
            // rightHandIK.position = new Vector3(closest.x + shoulderWidth, ledge, closest.z);

            pointWeAreAfterWeFinishRunning = hit.point;

            var after_push = closest;
            after_push.y = transform.position.y;
            pointWeAreAfterWeFinishClimbing.position = pointWeAreAfterWeFinishPushing = after_push + Vector3.forward * .25f;

            displacementY = ledge - transform.position.y;
            // transform.position += transform.up * displacementY;
            initialPosition = transform.position;

            collision.EnableLeftFoot();
            collision.EnableRightFoot();
            collision.DisableFeet();

            climbing = true;
            duration = clip.length;
            character.canMove = false;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            animator.SetBool("Climb_Medium", climbing);
        }
    }

    private void OnAnimatorIK() {
        if (climbing && ikActive) {
            var pos = animator.GetIKPosition(AvatarIKGoal.LeftHand);
            pos.y = ledgeY;
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, hit.collider.bounds.ClosestPoint(pos));

            pos = animator.GetIKPosition(AvatarIKGoal.RightHand);
            pos.y = ledgeY;
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPosition(AvatarIKGoal.RightHand, hit.collider.bounds.ClosestPoint(pos));
        } else {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
        }
    }

    private Vector3 pointWeAreAfterWeFinishRunning;
    private Vector3 pointWeAreAfterWeFinishPushing;

    private void OnAnimatorMove() {
        if (!climbing) {
            // animator.ApplyBuiltinRootMotion();
            return;
        }

        var walkTowardsWall = animator.GetFloat("WalkTowardsWall");
        if (walkTowardsWall > 0) {
            transform.position = Vector3.Lerp(transform.position, pointWeAreAfterWeFinishRunning, Time.deltaTime);
            print("wtw");
            return;
        }

        var climb = animator.GetFloat("Climb");
        if (climb > 0) {
            transform.position = Vector3.Lerp(transform.position, pointWeAreAfterWeFinishPushing, Time.deltaTime);
            print("cw");
            return;
        }
        // transform.position += new Vector3(0f, 0f, walkTowardsWall * Time.deltaTime);

        // var this_frame_move_this_amount_on_z = Mathf.Lerp(0f, pointWeAreAfterWeFinishRunning.z, Time.deltaTime);
        // if (clip.length - duration > climbTime)
        //     transform.position += new Vector3(0f, 0f, this_frame_move_this_amount_on_z);

        // Vector3 newPosition = transform.position;
        // var speed = animator.GetFloat("Movement");
        // var closest = hit.collider.bounds.ClosestPoint(initialPosition);

        // var distance = (closest - transform.position).magnitude;
        // // print(distance);
        // print(distance / range);

        // newPosition.z += (distance / range) * Time.deltaTime;
        // transform.position = newPosition;
    }

    private void OnValidate() {
        Time.timeScale = timeScale;
    }
}

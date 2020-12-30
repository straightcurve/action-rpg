using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchBehaviour : StateMachineBehaviour
{

    private RaycastHit hit;
    private bool grounded;
    private CharacterCollision collision;
    private Vector3 rightFootDestination;
    private Vector3 leftFootDestination;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //  compute feet locations

        grounded = Physics.Raycast(animator.bodyPosition, Vector3.down, out hit, 3f);
        if (!grounded)
            return;

        collision = animator.GetComponent<CharacterCollision>();

        rightFootDestination = hit.collider.ClosestPointOnBounds(collision.rightFoot.transform.position);
        leftFootDestination = hit.collider.ClosestPointOnBounds(collision.leftFoot.transform.position);

        // Debug.DrawLine(hit.point, hit.point + hit.normal, Color.magenta);

        var xz = Vector3.Cross(hit.normal, Vector3.down);

        Debug.DrawLine(hit.point, hit.point + xz, Color.magenta);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // Implement code that sets up animation IK (inverse kinematics)
        if (!grounded) {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
            return;
        }

        // animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        // animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootDestination);
        // animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        // animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootDestination);
    }
}

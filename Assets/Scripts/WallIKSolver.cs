
using UnityEngine;

public class WallIKSolver: MonoBehaviour {

    public bool useIK = true;

    public Transform head;
    public float range = .5f;

    private LHIKSolver lHIKSolver;
    private RHIKSolver rHIKSolver;
    public float colliderEdgeRange = .25f;
    public float verticalIKPositionOffset = 1f;

    private void Awake() {
        lHIKSolver = new LHIKSolver(transform, head, range);
        rHIKSolver = new RHIKSolver(transform, head, range);

        OnValidate();
    }

    private void Update() {
        lHIKSolver.Update();
        rHIKSolver.Update();
    }

    private void OnAnimatorIK() {
        if (!useIK)
            return;

        lHIKSolver.UpdateIK();
        // rHIKSolver.UpdateIK();
    }

    private void OnValidate() {
        if (lHIKSolver == null)
            return;

        lHIKSolver.verticalIKPositionOffset = verticalIKPositionOffset;
        lHIKSolver.colliderEdgeRange = colliderEdgeRange;
    }
}
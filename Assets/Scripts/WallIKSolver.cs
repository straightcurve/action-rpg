
using UnityEngine;

public class WallIKSolver: MonoBehaviour {

    public bool useIK = true;

    public Transform head;
    public float range = .5f;

    private LHIKSolver lHIKSolver;
    private RHIKSolver rHIKSolver;

    private void Awake() {
        lHIKSolver = new LHIKSolver(transform, head, range);
        rHIKSolver = new RHIKSolver(transform, head, range);
    }

    private void Update() {
        lHIKSolver.Update();
        rHIKSolver.Update();
    }

    private void OnAnimatorIK() {
        if (!useIK)
            return;

        lHIKSolver.UpdateIK();
        rHIKSolver.UpdateIK();
    }
}
using UnityEngine;

public class ThirdPersonMovementStrategy: IMovementStrategy {
    public Vector3 Compute(Vector3 input) {
        return input;
    }
}
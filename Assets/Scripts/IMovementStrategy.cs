using UnityEngine;

public interface IMovementStrategy {
    Vector3 Compute(Vector3 input);
}
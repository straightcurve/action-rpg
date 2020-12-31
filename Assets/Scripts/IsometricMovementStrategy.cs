using UnityEngine;

public class IsometricMovementStrategy: IMovementStrategy {
    //  https://en.wikipedia.org/wiki/Transformation_matrix#Rotation
    public Vector3 Compute(Vector3 input) {

        var a = Mathf.Cos(45);
        var b = Mathf.Sin(45);
        var c = -b;
        var d = a;

        var dir = input.normalized;
        return new Vector3(dir.x * a + dir.z * b, 0f, dir.x * c + dir.z * d);
    }
}
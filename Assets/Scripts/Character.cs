using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    public float speed = 10f;
    public bool canMove = true;
    new Rigidbody rigidbody;
    public Vector3 direction;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        if (rigidbody == null)
            throw new UnityException("rigidbody null");
    }

    void FixedUpdate() {
        if (direction.sqrMagnitude == 0)
            return;

        //  https://en.wikipedia.org/wiki/Transformation_matrix#Rotation

        var a = Mathf.Cos(45);
        var b = Mathf.Sin(45);
        var c = -b;
        var d = a;

        var dir = direction.normalized;
        var actualDirection = new Vector3(dir.x * a + dir.z * b, 0f, dir.x * c + dir.z * d);

        rigidbody.MovePosition(rigidbody.position + actualDirection * speed * Time.fixedDeltaTime * Time.timeScale);

        transform.rotation = Quaternion.LookRotation(actualDirection);


    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) {
            direction = Vector3.zero;
            return;
        }

        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");
    }
}

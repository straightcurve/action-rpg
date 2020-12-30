using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollision : MonoBehaviour
{
    public Collider hip;
    public Collider rightLeg;
    public Collider rightFoot;
    public Collider leftLeg;
    public Collider leftFoot;
    public Collider feet;


    public void EnableHip() => ToggleHip(true);
    public void DisableHip() => ToggleHip(false);
    public void ToggleHip(bool enabled = false) => hip.enabled = enabled;

    public void EnableRightLeg() => ToggleRightLeg(true);
    public void DisableRightLeg() => ToggleRightLeg(false);
    public void ToggleRightLeg(bool enabled = false) => rightLeg.enabled = enabled;

    public void EnableRightFoot() => ToggleRightFoot(true);
    public void DisableRightFoot() => ToggleRightFoot(false);
    public void ToggleRightFoot(bool enabled = false) => rightFoot.enabled = enabled;

    public void EnableLeftLeg() => ToggleLeftLeg(true);
    public void DisableLeftLeg() => ToggleLeftLeg(false);
    public void ToggleLeftLeg(bool enabled = false) => leftLeg.enabled = enabled;

    public void EnableLeftFoot() => ToggleLeftFoot(true);
    public void DisableLeftFoot() => ToggleLeftFoot(false);
    public void ToggleLeftFoot(bool enabled = false) => leftFoot.enabled = enabled;

    public void EnableFeet() => ToggleFeet(true);
    public void DisableFeet() => ToggleFeet(false);
    public void ToggleFeet(bool enabled = false) => feet.enabled = enabled;

    private void Awake() {
        var colliders = new List<Collider>();
        colliders.Add(hip);
        colliders.Add(rightLeg);
        colliders.Add(rightFoot);
        colliders.Add(leftLeg);
        colliders.Add(leftFoot);
        colliders.Add(feet);

        colliders.ForEach(c => {
            colliders.ForEach(i => {
                Physics.IgnoreCollision(c, i);
            });
        });
    }

}

using UnityEngine;

[CreateAssetMenu(menuName = "New/Attack", fileName = "Attack")]
public class Attack: ScriptableObject {

    public AnimationClip animation;
    public float cooldown;
    public string stateName;
}
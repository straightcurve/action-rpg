using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class AttackSystem : MonoBehaviour
{
    public Animator animator;
    public Attack[] attacks = new Attack[2];
    Character character;
    private bool attacking = false;
    private float duration = 0f;
    private int currentAttack = -1;

    // Start is called before the first frame update
    void Awake()
    {
        character = GetComponent<Character>();

        if (character == null)
            throw new UnityException("character null");

        if (animator == null)
            throw new UnityException("animator null");
    }

    private void Update() {
        if (attacking) {
            duration -= Time.deltaTime;

            if (duration <= 0f) {
                attacking = false;
                animator.SetBool(attacks[currentAttack].stateName, attacking);
                currentAttack = -1;
            }

            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            attacking = true;
            currentAttack = 0;
            duration = attacks[currentAttack].animation.length;

            animator.SetBool(attacks[currentAttack].stateName, attacking);
        } else if (Input.GetMouseButtonDown(1)) {
            attacking = true;
            currentAttack = 1;
            duration = attacks[currentAttack].animation.length;

            animator.SetBool(attacks[currentAttack].stateName, attacking);
        }
    }
}

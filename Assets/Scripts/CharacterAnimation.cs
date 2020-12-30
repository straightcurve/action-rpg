using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterAnimation : MonoBehaviour
{
    public Animator animator;
    Character character;

    // Start is called before the first frame update
    void Awake()
    {
        character = GetComponent<Character>();

        if (character == null)
            throw new UnityException("character null");

        if (animator == null)
            throw new UnityException("animator null");
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Walk", character.direction.sqrMagnitude > 0);
    }
}

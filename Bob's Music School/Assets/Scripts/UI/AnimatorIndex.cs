using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorIndex : MonoBehaviour {

    Animator animator;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("A"))
        {
            animator.SetInteger("TitleIndex", 1);

        }
        if (Input.GetButtonDown("B"))
        {
            animator.SetInteger("TitleIndex", 0);

        }

    }
}

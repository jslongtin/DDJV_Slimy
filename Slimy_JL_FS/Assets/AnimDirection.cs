using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimDirection : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rig;

    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 movement = rig.velocity.normalized;

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);

        

        
    }
}

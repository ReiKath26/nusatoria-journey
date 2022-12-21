using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof (BoxCollider))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Animator playerAnimate;

    [SerializeField] private float _movementSpeed;

    private void FixedUpdate()
    {
        _rigidBody.velocity = new Vector3(joystick.Horizontal * _movementSpeed, 0, joystick.Vertical * _movementSpeed);
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
           
            transform.rotation = Quaternion.LookRotation(_rigidBody.velocity);
            playerAnimate.SetBool("isWalking", true);
            AudioManager.instance.Play("Footsteps");
            
        }

        else
        {
            playerAnimate.SetBool("isWalking", false);
            AudioManager.instance.Stop("Footsteps");
        }
    }
}

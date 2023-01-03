using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof (BoxCollider))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Animator playerAnimate;

    [SerializeField] private CameraFollow follow;

    [SerializeField] private float _movementSpeed;

    private void FixedUpdate()
    {
        Debug.Log(joystick.Horizontal);
        Debug.Log(joystick.Vertical);
        Debug.Log(follow.getAngle());


        SaveSlots slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));

        if(slot.playerGender == 0)
        {
              if(follow.getAngle() > 0 && follow.getAngle() <= 90)
            {
                _rigidBody.velocity = new Vector3(joystick.Vertical * _movementSpeed, 0, -joystick.Horizontal * _movementSpeed);
            }

             else if(follow.getAngle() > 90 && follow.getAngle() <= 180)
            {
                _rigidBody.velocity = new Vector3(-joystick.Horizontal * _movementSpeed, 0, -joystick.Vertical * _movementSpeed);
            }

            else if(follow.getAngle() > 180 && follow.getAngle() <= 270)
            {
                _rigidBody.velocity = new Vector3(-joystick.Vertical * _movementSpeed, 0, joystick.Horizontal * _movementSpeed);
            }

             else 
            {
                _rigidBody.velocity = new Vector3(joystick.Horizontal * _movementSpeed, 0, joystick.Vertical * _movementSpeed);
            }
        }

        else
        {
              if(follow.getAngle() > 0 && follow.getAngle() <= 90)
            {
                _rigidBody.velocity = new Vector3(-joystick.Vertical * _movementSpeed, 0, joystick.Horizontal * _movementSpeed);
            }

             else if(follow.getAngle() > 90 && follow.getAngle() <= 180)
            {
                _rigidBody.velocity = new Vector3(-joystick.Horizontal * _movementSpeed, 0, -joystick.Vertical * _movementSpeed);
            }

            else if(follow.getAngle() > 180 && follow.getAngle() <= 270)
            {
                _rigidBody.velocity = new Vector3(joystick.Vertical * _movementSpeed, 0, -joystick.Horizontal * _movementSpeed);
            }

             else 
            {
                _rigidBody.velocity = new Vector3(joystick.Horizontal * _movementSpeed, 0, joystick.Vertical * _movementSpeed);
            }
        }

       

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            Vector3 defaultVelocity = new Vector3(joystick.Horizontal * _movementSpeed, 0, joystick.Vertical * _movementSpeed);
            // transform.rotation = Quaternion.LookRotation(_rigidBody.velocity);
            transform.rotation = Quaternion.AngleAxis (follow.getAngle()  + Vector3.SignedAngle (Vector3.forward, defaultVelocity.normalized + Vector3.forward * 0.001f, Vector3.up), Vector3.up);
            playerAnimate.SetBool("isWalking", true);
            AudioManager.instance.Play("Footsteps");
            
        }

        else
        {
            slot.lastPosition.x_pos = transform.position.x;
            slot.lastPosition.y_pos = transform.position.y;
            slot.lastPosition.z_pos = transform.position.z;
            SaveHandler.instance.saveSlot(slot, slot.slot);
            playerAnimate.SetBool("isWalking", false);
            AudioManager.instance.Stop("Footsteps");
        }
    }
}

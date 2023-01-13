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
        SaveSlots slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, -Vector3.up);

        if(Physics.Raycast(downRay, out hit))
        {
           
                if(follow.getAngle() >= 90 && follow.getAngle() < 180)
                {
                    
                    _rigidBody.velocity = new Vector3(joystick.Vertical * _movementSpeed, hit.distance > 0 ? -hit.distance : 0, -joystick.Horizontal * _movementSpeed);
                }

                else if(follow.getAngle() >= 180 && follow.getAngle() < 270)
                {
                    _rigidBody.velocity = new Vector3(-joystick.Horizontal * _movementSpeed, hit.distance > 0 ? -hit.distance : 0, -joystick.Vertical * _movementSpeed);
                }

                else if(follow.getAngle() >= 270 && follow.getAngle() < 360)
                {
                    _rigidBody.velocity = new Vector3(-joystick.Vertical * _movementSpeed, hit.distance > 0 ? -hit.distance : 0, joystick.Horizontal * _movementSpeed);
                }

                else 
                {
                    _rigidBody.velocity = new Vector3(joystick.Horizontal * _movementSpeed, hit.distance > 0 ? -hit.distance : 0, joystick.Vertical * _movementSpeed);
                }
            
        }

       

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
             _rigidBody.freezeRotation = false;
            Vector3 defaultVelocity = new Vector3(joystick.Horizontal * _movementSpeed, 0, joystick.Vertical * _movementSpeed);
            // transform.rotation = Quaternion.LookRotation(_rigidBody.velocity);
            transform.rotation = Quaternion.AngleAxis (follow.getAngle()  + Vector3.SignedAngle (Vector3.forward, defaultVelocity.normalized + Vector3.forward * 0.001f, Vector3.up), Vector3.up);
            playerAnimate.SetBool("isWalking", true);
            AudioManager.instance.Play("Footsteps");

            // RaycastHit hit;
            // Ray downRay = new Ray(transform.position, -Vector3.up);
            //  if(Physics.Raycast(downRay, out hit))
            // {
            //     if(hit.distance != 0)
            //     {
            //          _rigidBody.AddForce(0, -20, 0, ForceMode.Impulse);
            //     }
               
            // }
        }

        else
        {
            slot.lastPosition.x_pos = transform.position.x;
            slot.lastPosition.y_pos = transform.position.y;
            slot.lastPosition.z_pos = transform.position.z;
            SaveHandler.instance.saveSlot(slot, slot.slot);
            playerAnimate.SetBool("isWalking", false);
            AudioManager.instance.Stop("Footsteps");
             _rigidBody.freezeRotation = true;

            // RaycastHit hit;
            // Ray downRay = new Ray(transform.position, -Vector3.up);
            //  if(Physics.Raycast(downRay, out hit))
            // {
            //     Debug.Log("Hit:" + hit.distance);
            //     if(hit.distance != 0)
            //     {
            //          _rigidBody.AddForce(0, -500, 0, ForceMode.Impulse);
            //         
            //     }
               
            // }
            
        }
    }
}

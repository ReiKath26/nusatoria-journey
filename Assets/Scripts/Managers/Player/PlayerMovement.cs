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

    private void Update()
    {
         SaveSlots slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
       
    

        RaycastHit hit;
        Ray downRay = new Ray(transform.position, -Vector3.up);
          if(Physics.Raycast(downRay, out hit))
        {
            if(hit.collider.tag == "Ground")
            {
                  var input = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
                  var vel = Quaternion.AngleAxis(follow.getAngle(), Vector3.up) * input * _movementSpeed;

                  _rigidBody.velocity = new Vector3(vel.x, hit.distance > 3 ? _rigidBody.velocity.y - hit.distance : 0, vel.z);
            }


        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
             _rigidBody.freezeRotation = false;
             transform.rotation = Quaternion.LookRotation(new Vector3(_rigidBody.velocity.x, 0, _rigidBody.velocity.z));
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
             _rigidBody.freezeRotation = true;
            
        }
            
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private SaveSlots slot;

    [SerializeField] private Rigidbody _rigidBody;

    public Transform target_0;
    public Transform target_1;

    private float smoothen = 0.125f;
    public Vector3 offset;

    // void Awake()
    // {
    //     slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));

    // }

    void FixedUpdate()
    {
        // if (slot.playerGender == 0)
        // {
            Vector3 cam_pos = target_0.position + offset;
            Vector3 smooth_cam_pos = Vector3.Lerp(transform.position,cam_pos, smoothen);
            transform.position = smooth_cam_pos;
        // }

        // else
        // {
        //     Vector3 cam_pos = target_1.position + offset;
        //     Vector3 smooth_cam_pos = Vector3.Lerp(transform.position,cam_pos, smoothen);
        //     transform.position = smooth_cam_pos;
        // }
      
    }
}

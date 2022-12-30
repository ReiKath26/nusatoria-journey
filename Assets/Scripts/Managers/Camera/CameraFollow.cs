using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private SaveSlots slot;

    public GameObject target_0;
    public GameObject target_1;
    public FixedTouchField touchField;

    protected float CameraAngle;
    protected float CameraAngleSpeed = 2f;

    void Awake()
    {
        slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));

        if (slot.playerGender == 0)
        {
            target_0.transform.position = new Vector3(slot.lastPosition.x_pos, slot.lastPosition.y_pos, slot.lastPosition.z_pos);
            target_0.SetActive(true);
            target_1.SetActive(false);
        }

        else
        {
            target_1.transform.position = new Vector3(slot.lastPosition.x_pos, slot.lastPosition.y_pos, slot.lastPosition.z_pos);
            target_0.SetActive(false);
            target_1.SetActive(true);
        }

    }

    void FixedUpdate()
    {
        if (slot.playerGender == 0)
        {

              CameraAngle += touchField.TouchDist.x * CameraAngleSpeed;

        transform.position = target_0.transform.position + Quaternion.AngleAxis(CameraAngle, Vector3.up) * new Vector3(-3.546f, 54f, -66.5f);
        transform.rotation = Quaternion.LookRotation(target_0.transform.position + Vector3.up * 2f - transform.position, Vector3.up);
        }

        else
        {

            CameraAngle += touchField.TouchDist.x * CameraAngleSpeed;

        transform.position = target_1.transform.position + Quaternion.AngleAxis(CameraAngle, Vector3.up) * new Vector3(-3.546f, 54f, -66.5f);
        transform.rotation = Quaternion.LookRotation(target_1.transform.position + Vector3.up * 2f - transform.position, Vector3.up);
        }

      
      
    }
}
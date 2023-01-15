using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private SaveSlots slot;

    private GameObject target;
    public FixedTouchField touchField;

    protected float CameraAngle;
    protected float CameraAngleSpeed = 2f;

    void Start()
    {
        slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));


        if (slot.playerGender == 0)
        {
            target = FindInactiveObject.instance.find("Male MC Model");
        }

        else
        {
            target = FindInactiveObject.instance.find("Female MC Model");
        }

         target.SetActive(true);
        target.transform.position = new Vector3(slot.lastPosition.x_pos, slot.lastPosition.y_pos, slot.lastPosition.z_pos);

    }

    void FixedUpdate()
    {
        
            // while(CameraAngle < 360 && touchField.TouchDist.x != 0)
            // {
                CameraAngle += touchField.TouchDist.x * CameraAngleSpeed;
                target.transform.rotation = Quaternion.AngleAxis (CameraAngle  + Vector3.SignedAngle (Vector3.forward, Vector3.forward * 0.001f, Vector3.up), Vector3.up);
            // }

        if(slot.chapterNumber == 1)
        {
              transform.position = target.transform.position + Quaternion.AngleAxis(CameraAngle, Vector3.up) * new Vector3(-0.646f, 58.3f, -96.8f);
              transform.rotation = Quaternion.LookRotation(target.transform.position + Vector3.up * 2f - transform.position, Vector3.up);
        }

        else
        {
            transform.position = target.transform.position + Quaternion.AngleAxis(CameraAngle, Vector3.up) * new Vector3(-35.7446f, 58.06f, -70.3f);
            transform.rotation = Quaternion.LookRotation(target.transform.position + Vector3.up * 2f - transform.position, Vector3.up);
        }

      
    }

    public float getAngle()
    {
        return CameraAngle;
    }
}

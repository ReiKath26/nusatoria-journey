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
        
              CameraAngle += touchField.TouchDist.x * CameraAngleSpeed;

              if(CameraAngle > 360)
              {
                CameraAngle -= 360;
              }

              else if(CameraAngle < 0)
              {
                CameraAngle += 360;
              }

        if(slot.chapterNumber == 1)
        {
              transform.position = target.transform.position + Quaternion.AngleAxis(CameraAngle, Vector3.up) * new Vector3(-0.894f, 149.3f, -87.6095f);
              transform.rotation = Quaternion.LookRotation(target.transform.position + Vector3.up * 2f - transform.position, Vector3.up);
        }

        else
        {
            transform.position = target.transform.position + Quaternion.AngleAxis(CameraAngle, Vector3.up) * new Vector3(9.3f, 145.7f, -83.723f);
            transform.rotation = Quaternion.LookRotation(target.transform.position + Vector3.up * 2f - transform.position, Vector3.up);
        }

      
    }

    public float getAngle()
    {
        return CameraAngle;
    }
}

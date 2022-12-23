using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarker : MonoBehaviour
{
    [SerializeField] private GameObject marker;

    public void Show()
    {
        marker.SetActive(true);
    }

    public void Hide()
    {
        marker.SetActive(false);
    }
}

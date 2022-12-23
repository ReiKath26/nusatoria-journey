using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyconceptsUI : MonoBehaviour
{
    [SerializeField] private GameObject KeyconceptPlaceHolder;
    [SerializeField] private GameObject SelectedKeyconceptPlaceHolder;

    [SerializeField] private TextMeshProUGUI keyconceptText;
    [SerializeField] private TextMeshProUGUI selectedText;

    [SerializeField] private TextMeshProUGUI keyconceptTitle;
    [SerializeField] private TextMeshProUGUI keyconceptDesc;
    public int number;

    public bool selected;

    private SaveSlots slot;

    private void Update()
    {
        slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
        foreach(KeyConcepts concept in slot.player_glossary.conceptList)
        {
            if(concept.keyNumber == number)
            {
                setKeyConcept(concept);
            }
        }

        if (selected == true)
        {
            KeyconceptPlaceHolder.SetActive(false);
            SelectedKeyconceptPlaceHolder.SetActive(true);
        }

        else

        {
            KeyconceptPlaceHolder.SetActive(true);
            SelectedKeyconceptPlaceHolder.SetActive(false);
        }
    }

    public void setKeyConcept(KeyConcepts keyconcept)
    {
        if(keyconcept.unlocked == true)
        {
            keyconceptText.text = keyconcept.keyName;
        }

        else
        {
            keyconceptText.text = "???";
        }
    }

    public void selectKeyConcept()
    {
        SelectedKeyconceptManager.instance.triggerSelectKeyConcept(number);
        foreach(KeyConcepts concept in slot.player_glossary.conceptList)
        {
            if(concept.keyNumber == number)
            {
                if(concept.unlocked == true)
                {
                    keyconceptTitle.text = concept.keyName;
                    keyconceptDesc.text = concept.keyDesc;
                }
                
                else
                {
                    keyconceptTitle.text = "???";
                    keyconceptDesc.text = "Selesaikan lebih banyak misi untuk membuka konsep ini";
                }
            }
        }
        
    }
}

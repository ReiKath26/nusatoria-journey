using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyconceptsUI : MonoBehaviour
{
    [SerializeField] private GameObject [] KeyconceptPlaceHolder;
    [SerializeField] private GameObject [] SelectedKeyconceptPlaceHolder;

    [SerializeField] private TextMeshProUGUI [] keyconceptText;
    [SerializeField] private TextMeshProUGUI [] selectedText;

    [SerializeField] private TextMeshProUGUI keyconceptTitle;
    [SerializeField] private TextMeshProUGUI keyconceptDesc;
    [SerializeField] private int minIndexNum;
    [SerializeField] private int maxIndexNum;


    private SaveSlots slot;

    private void Update()
    {
        slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));
        foreach(KeyConcepts concept in slot.player_glossary.conceptList)
        {
            if(concept.keyNumber >= minIndexNum && concept.keyNumber <= maxIndexNum)
            {
                setKeyConcept(concept, concept.keyNumber);
            }
   
            
        }
    }

    public void setKeyConcept(KeyConcepts keyconcept, int number)
    {
        if(keyconcept.unlocked == true)
        {
            keyconceptText[number].text = keyconcept.keyName;
            selectedText[number].text = keyconcept.keyName;
        }

        else
        {
            keyconceptText[number].text = "???";
            selectedText[number].text = "???";
        }
    }

    public void selectKeyConcept(int number)
    {
        for(int i = 0; i < KeyconceptPlaceHolder.Length; i++)
        {
            if(i == number)
            {
                KeyconceptPlaceHolder[i].SetActive(false);
                SelectedKeyconceptPlaceHolder[i].SetActive(true);
            }

            else
            {
                KeyconceptPlaceHolder[i].SetActive(true);
                SelectedKeyconceptPlaceHolder[i].SetActive(false);
            }
        }

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

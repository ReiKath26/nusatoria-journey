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
    [SerializeField] private KeyConceptUISection[] sections;

    [SerializeField] private GameObject upButton;
    [SerializeField] private GameObject downButton;

    private int currentSection;


    void Awake()
    {
        currentSection = 0;

        if(downButton != null && upButton != null)
        {
            downButton.SetActive(true);
            upButton.SetActive(false);
        }
   
    }


    private void Update()
    {
        if(currentSection < sections.Length)
        {
            for(int i=sections[currentSection].minNumber;i<=sections[currentSection].maxNumber;i++)
            {
                KeyConcepts concept = SaveHandler.instance.loadKeyconcepts(PlayerPrefs.GetInt("choosenSlot"), i);

                SaveSlots slot = SaveHandler.instance.loadSlot(PlayerPrefs.GetInt("choosenSlot"));

                if(slot.chapterNumber == 1)
                {
                    setKeyConcept(concept, concept.keyNumber);
                }
                else
                {
                    setKeyConcept(concept, (concept.keyNumber - sections[currentSection].minNumber));
                }
                
            }
        }
        
    }

    public void goDown()
    {
       if(currentSection + 1 < sections.Length)
       {
            currentSection += 1;
            if(currentSection == sections.Length -1)
            {
                downButton.SetActive(false);
            }
            upButton.SetActive(true);
       }
      
    }

    public void goUp()
    {
       if(currentSection - 1 >= 0)
       {
            currentSection -= 1;

            if(currentSection == 0)
            {
                 upButton.SetActive(false);
            }
            downButton.SetActive(true);
       }
    }

    public void setKeyConcept(KeyConcepts keyconcept, int number)
    {
        if(keyconcept.unlocked == true)
        {
            keyconceptText[number].text = keyconcept.keyName.Split(' ')[0] + " " + keyconcept.keyName.Split(' ')[1];
            selectedText[number].text = keyconcept.keyName.Split(' ')[0] + " " + keyconcept.keyName.Split(' ')[1];
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

    
        KeyConcepts concept = SaveHandler.instance.loadKeyconcepts(PlayerPrefs.GetInt("choosenSlot"), (sections[currentSection].minNumber + number));

        if(concept.keyNumber == sections[currentSection].minNumber + number)
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

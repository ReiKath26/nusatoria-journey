using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnboardingManager : MonoBehaviour
{
    private int genderChoose = 0;
    private int slotNumber = 1;
    private string chooseName;

    [SerializeField] private GameObject onboardPanelOne;
    [SerializeField] private GameObject onboardPanelTwo;
    [SerializeField] private GameObject onboardPanelThree;

    [SerializeField] private GameObject female_picture_highlight;
    [SerializeField] private GameObject male_picture_highlight;
    [SerializeField] private GameObject userNameInput;

    [SerializeField] private GameObject female_picture;
    [SerializeField] private GameObject male_picture;
    [SerializeField] private TextMeshProUGUI player_name_text;

    void Awake()
    {
        slotNumber = PlayerPrefs.GetInt("choosenSlot");
    }

    public void choosePicture(int choosen)
    {
        if (choosen == 0)
        {
            male_picture_highlight.SetActive(true);
            female_picture_highlight.SetActive(false);
        }

        else
        {
            male_picture_highlight.SetActive(false);
            female_picture_highlight.SetActive(true);
        }

        genderChoose = choosen;
    }

    public void ReadStringInput(string s)
    {
        chooseName = s;
    }

    public void saveName()
    {

        if (chooseName == "")
        {
            return;
        }
        SceneManage.instance.loadPopUp(onboardPanelThree);
        SceneManage.instance.closePopUp(onboardPanelTwo);
        displayData();
    }

    public void displayData()
    {
        if (genderChoose == 0)
        {
            male_picture.SetActive(true);
            female_picture.SetActive(false);
        }

        else
        {
            male_picture.SetActive(false);
            female_picture.SetActive(true);
        }

        player_name_text.text = chooseName;
    }

    public void initGame()
    {
        Glossary glossary = new Glossary();

        glossary.conceptList.Add(new KeyConcepts() {keyNumber = 0, keyName = "", keyDesc = ""});

        Inventory inventory = new Inventory();
        inventory.slotList.Add(new InventorySlots() {slotNumber = 0, itemSaved = null});
        inventory.slotList.Add(new InventorySlots() {slotNumber = 1, itemSaved = null});
        inventory.slotList.Add(new InventorySlots() {slotNumber = 2, itemSaved = null});
        inventory.slotList.Add(new InventorySlots() {slotNumber = 3, itemSaved = null});
        inventory.slotList.Add(new InventorySlots() {slotNumber = 4, itemSaved = null});

        PlayerPosition initialPosition = new PlayerPosition() {x_pos = 0, y_pos = 0, z_pos = 0};
        SaveSlots slot = new SaveSlots() {slot = slotNumber, playerName = chooseName, playerGender = genderChoose,
        time = 0, chapterNumber = 0, lastPosition = initialPosition, understandingLevel = 0, missionNumber = 0, storyNumber = 0, 
        player_glossary = glossary, player_inventory = inventory};
        SaveHandler.instance.saveSlot(slot, slotNumber);
        SceneManage.instance.LoadScene(4);
    }

    public void resetData()
    {
        genderChoose = 0;
        chooseName = "";
        SceneManage.instance.loadPopUp(onboardPanelOne);
        SceneManage.instance.closePopUp(onboardPanelThree);
    }

}
